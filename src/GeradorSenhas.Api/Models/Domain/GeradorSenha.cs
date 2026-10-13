using System.Security.Cryptography;
using GeradorSenhas.Api.Models.Exceptions;
using GeradorSenhas.Api.Models.Interfaces;

namespace GeradorSenhas.Api.Models.Domain;

/*
 * Implementação do gerador de senhas, responsável pelas regras de negócio:
 * - RN02: nenhuma senha gerada se repete;
 * - RN03: a senha não tem caracteres iguais em sequência, nem valores sequenciais
 *   (ex.: "abc", "321"), mistura letras, números e símbolos e não coincide com
 *   nenhuma senha já gerada em qualquer sequência de TAMANHO_SEQUENCIA caracteres,
 *   nem na ordem inversa (ex.: "gy3PCre/?2S071-p" e "p-170S2?/erCP3yg");
 * - RN04: a senha tem caracteres especiais, 16 caracteres e nenhum espaço em branco.
 * O histórico de senhas e sequências fica em memória, por isso a instância deve
 * ser registrada como singleton. A geração é protegida por trava para uso concorrente.
 */
public class GeradorSenha : IGeradorSenha
{
    private const int TAMANHO_SENHA = 16;
    private const int TAMANHO_SEQUENCIA = 3;

    private const string LETRAS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUMEROS = "0123456789";
    private const string SIMBOLOS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
    private const string ALFABETO = LETRAS + NUMEROS + SIMBOLOS;
    private const int MAXIMO_TENTATIVAS = 1000;

    private readonly Lock _trava = new Lock();
    private readonly HashSet<string> _senhasGeradas = new HashSet<string>();
    private readonly HashSet<string> _sequenciasUsadas = new HashSet<string>();

    /*
     * Gera uma nova senha que atende às regras RN02, RN03 e RN04 e registra
     * suas sequências para que nenhuma senha futura coincida com ela.
     * Lança SenhaInvalidaException quando não é possível gerar uma senha válida.
     */
    public string Gerar()
    {
        lock (_trava)
        {
            for (int tentativa = 0; tentativa < MAXIMO_TENTATIVAS; tentativa++)
            {
                string candidata = MontarCandidata();

                if (EhValida(candidata))
                {
                    Registrar(candidata);

                    return candidata;
                }
            }

            throw new SenhaInvalidaException(
                "Não foi possível gerar uma senha que não coincida com as senhas já existentes.");
        }
    }

    /*
     * Monta a senha caractere a caractere, sorteando apenas entre os caracteres
     * que não criam repetição em sequência, valores sequenciais ou sequências
     * já usadas por outras senhas. Retorna antes de completar o tamanho quando
     * nenhum caractere é permitido na posição, o que invalida a candidata.
     */
    private string MontarCandidata()
    {
        char[] senha = new char[TAMANHO_SENHA];

        for (int posicao = 0; posicao < TAMANHO_SENHA; posicao++)
        {
            List<char> permitidos = ALFABETO
                .Where(caractere => EhPermitido(senha, posicao, caractere))
                .ToList();

            if (permitidos.Count == 0)
            {
                return new string(senha, 0, posicao);
            }

            senha[posicao] = permitidos[RandomNumberGenerator.GetInt32(permitidos.Count)];
        }

        return new string(senha);
    }

    /*
     * Indica se o caractere pode ocupar a posição da senha em construção (RN03).
     */
    private bool EhPermitido(char[] senha, int posicao, char caractere)
    {
        if (posicao > 0 && senha[posicao - 1] == caractere)
        {
            return false;
        }

        if (posicao < TAMANHO_SEQUENCIA - 1)
        {
            return true;
        }

        string sequencia = new string(senha, posicao - (TAMANHO_SEQUENCIA - 1), TAMANHO_SEQUENCIA - 1) + caractere;

        return !EhSequencial(sequencia) && !_sequenciasUsadas.Contains(sequencia);
    }

    /*
     * Valida a senha completa: 16 caracteres (RN04), mistura de letras, números
     * e símbolos (RN03 e RN04) e inexistência de senha igual já gerada (RN02).
     */
    private bool EhValida(string senha)
    {
        return senha.Length == TAMANHO_SENHA
            && senha.Any(LETRAS.Contains)
            && senha.Any(NUMEROS.Contains)
            && senha.Any(SIMBOLOS.Contains)
            && !_senhasGeradas.Contains(senha);
    }

    /*
     * Registra a senha e suas sequências, nas ordens direta e inversa,
     * para que nenhuma senha futura coincida com ela (RN02 e RN03).
     */
    private void Registrar(string senha)
    {
        _senhasGeradas.Add(senha);

        for (int inicio = 0; inicio + TAMANHO_SEQUENCIA <= senha.Length; inicio++)
        {
            string sequencia = senha.Substring(inicio, TAMANHO_SEQUENCIA);
            char[] inversa = sequencia.ToCharArray();
            Array.Reverse(inversa);

            _sequenciasUsadas.Add(sequencia);
            _sequenciasUsadas.Add(new string(inversa));
        }
    }

    /*
     * Indica se os caracteres da sequência são consecutivos na tabela de caracteres,
     * em ordem crescente (ex.: "abc") ou decrescente (ex.: "321").
     */
    private static bool EhSequencial(string sequencia)
    {
        int passo = sequencia[1] - sequencia[0];

        if (passo != 1 && passo != -1)
        {
            return false;
        }

        for (int posicao = 2; posicao < sequencia.Length; posicao++)
        {
            if (sequencia[posicao] - sequencia[posicao - 1] != passo)
            {
                return false;
            }
        }

        return true;
    }
}
