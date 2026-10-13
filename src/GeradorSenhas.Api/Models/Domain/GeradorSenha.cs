using System.Security.Cryptography;
using GeradorSenhas.Api.Models.Exceptions;
using GeradorSenhas.Api.Models.Interfaces;

namespace GeradorSenhas.Api.Models.Domain;

/*
 * Implementação do gerador de senhas, responsável pelas regras de negócio:
 * - RN02: nenhuma senha gerada se repete;
 * - RN03: a senha tem caracteres especiais, 16 caracteres e nenhum espaço em branco.
 * O histórico de senhas fica em memória, por isso a instância deve ser
 * registrada como singleton. A geração é protegida por trava para uso concorrente.
 */
public class GeradorSenha : IGeradorSenha
{
    private const int TAMANHO_SENHA = 16;

    private const string LETRAS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
    private const string NUMEROS = "0123456789";
    private const string SIMBOLOS = "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~";
    private const string ALFABETO = LETRAS + NUMEROS + SIMBOLOS;
    private const int MAXIMO_TENTATIVAS = 1000;

    private readonly Lock _trava = new Lock();
    private readonly HashSet<string> _senhasGeradas = new HashSet<string>();
    private readonly Func<int, int> _sortearIndice;

    /*
     * Cria o gerador usando um sorteio criptograficamente seguro.
     */
    public GeradorSenha() : this(RandomNumberGenerator.GetInt32)
    {
    }

    /*
     * Cria o gerador com uma função de sorteio que recebe o tamanho do alfabeto
     * e devolve um índice. Usado pelos testes para tornar a geração determinística.
     */
    internal GeradorSenha(Func<int, int> sortearIndice)
    {
        _sortearIndice = sortearIndice;
    }

    /*
     * Gera uma nova senha que atende às regras RN02 e RN03 e a registra
     * para que nenhuma senha futura seja igual a ela.
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
                    _senhasGeradas.Add(candidata);

                    return candidata;
                }
            }

            throw new SenhaInvalidaException(
                "Não foi possível gerar uma senha diferente das senhas já existentes.");
        }
    }

    /*
     * Monta a senha sorteando TAMANHO_SENHA caracteres do alfabeto,
     * que não contém espaços em branco (RN03).
     */
    private string MontarCandidata()
    {
        char[] senha = new char[TAMANHO_SENHA];

        for (int posicao = 0; posicao < TAMANHO_SENHA; posicao++)
        {
            senha[posicao] = ALFABETO[_sortearIndice(ALFABETO.Length)];
        }

        return new string(senha);
    }

    /*
     * Valida a senha completa: presença de caracteres especiais (RN03)
     * e inexistência de senha igual já gerada (RN02).
     */
    private bool EhValida(string senha)
    {
        return senha.Any(SIMBOLOS.Contains)
            && !_senhasGeradas.Contains(senha);
    }
}
