using GeradorSenhas.Api.Models.Domain;

namespace GeradorSenhas.Tests.Models;

/*
 * Testes unitários das regras de negócio RN02, RN03 e RN04 no GeradorSenha.
 * Como a geração é aleatória, cada regra é verificada sobre um grande número de
 * senhas geradas: uma implementação correta passa em todas as execuções,
 * mantendo os testes repetíveis e reprodutíveis.
 */
public class GeradorSenhaTests
{
    private const int QUANTIDADE_GERACOES = 1000;
    private const int TAMANHO_SENHA = 16;
    private const int TAMANHO_SEQUENCIA = 3;
    private const string EXEMPLO_SENHA_1 = "gy3PCre/?2S071-p";
    private const string EXEMPLO_SENHA_2 = "p-170S2?/erCP3yg";

    private readonly GeradorSenha _geradorSenha;

    /*
     * Instancia o gerador de senhas real.
     */
    public GeradorSenhaTests()
    {
        _geradorSenha = new GeradorSenha();
    }

    /*
     * RN02: não deve ser gerada nenhuma senha igual.
     */
    [Fact]
    public void Rn02_SenhasGeradas_NaoDevemSeRepetir()
    {
        HashSet<string> senhas = new HashSet<string>();

        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            Assert.True(senhas.Add(senha), $"A senha '{senha}' foi gerada mais de uma vez.");
        }
    }

    /*
     * RN03: a senha não pode seguir um padrão de letras, números ou símbolos iguais,
     * ou seja, não pode ter caracteres iguais em sequência (ex.: "aa", "11", "!!").
     */
    [Fact]
    public void Rn03_SenhaGerada_NaoDeveTerCaracteresIguaisEmSequencia()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            for (int posicao = 1; posicao < senha.Length; posicao++)
            {
                Assert.True(
                    senha[posicao] != senha[posicao - 1],
                    $"A senha '{senha}' possui o caractere '{senha[posicao]}' repetido em sequência.");
            }
        }
    }

    /*
     * RN03: o exemplo da especificação deve ser identificado como padrão coincidente:
     * a senha 2 ("p-170S2?/erCP3yg") é a senha 1 ("gy3PCre/?2S071-p") em ordem inversa,
     * mesmo sem caracteres repetidos ou valores sequenciais em nenhuma das duas.
     */
    [Fact]
    public void Rn03_ExemploDaEspecificacao_DeveSerIdentificadoComoPadraoCoincidente()
    {
        Assert.DoesNotContain(ExtrairSequencias(EXEMPLO_SENHA_1), EhSequencial);
        Assert.DoesNotContain(ExtrairSequencias(EXEMPLO_SENHA_2), EhSequencial);
        Assert.True(CoincidemEmAlgumaSequencia(EXEMPLO_SENHA_1, EXEMPLO_SENHA_2));
    }

    /*
     * RN03: duas senhas não podem coincidir em nenhuma sequência de valores,
     * nem na ordem inversa (ex.: "gy3PCre/?2S071-p" e "p-170S2?/erCP3yg").
     * Uma sequência é considerada a partir de TAMANHO_SEQUENCIA caracteres:
     * com 2 caracteres o número de combinações possíveis se esgotaria após
     * poucas centenas de senhas, tornando a regra impossível de cumprir.
     */
    [Fact]
    public void Rn03_SenhasGeradas_NaoDevemCoincidirEmNenhumaSequencia()
    {
        Dictionary<string, string> sequenciasUsadas = new Dictionary<string, string>();

        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();
            List<string> sequencias = ExtrairSequencias(senha);

            foreach (string sequencia in sequencias)
            {
                Assert.False(
                    sequenciasUsadas.TryGetValue(sequencia, out string? senhaAnterior),
                    $"A senha '{senha}' coincide na sequência '{sequencia}' com a senha '{senhaAnterior}'.");
            }

            foreach (string sequencia in sequencias)
            {
                sequenciasUsadas[sequencia] = senha;
                sequenciasUsadas[Inverter(sequencia)] = senha;
            }
        }
    }

    /*
     * RN03: a senha não pode ter valores sequenciais, crescentes ou decrescentes
     * (ex.: "123", "abc", "cba", "321").
     */
    [Fact]
    public void Rn03_SenhaGerada_NaoDeveTerValoresSequenciais()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            foreach (string sequencia in ExtrairSequencias(senha))
            {
                Assert.False(
                    EhSequencial(sequencia),
                    $"A senha '{senha}' possui os valores sequenciais '{sequencia}'.");
            }
        }
    }

    /*
     * RN03: a senha não pode ser composta por um único tipo de caractere,
     * devendo misturar letras, números e símbolos.
     */
    [Fact]
    public void Rn03_SenhaGerada_DeveMisturarLetrasNumerosESimbolos()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            Assert.True(senha.Any(char.IsLetter), $"A senha '{senha}' não possui letras.");
            Assert.True(senha.Any(char.IsDigit), $"A senha '{senha}' não possui números.");
            Assert.True(senha.Any(EhSimbolo), $"A senha '{senha}' não possui símbolos.");
        }
    }

    /*
     * RN04: a senha tem que ter caracteres especiais.
     */
    [Fact]
    public void Rn04_SenhaGerada_DeveTerCaracteresEspeciais()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            Assert.True(senha.Any(EhSimbolo), $"A senha '{senha}' não possui caracteres especiais.");
        }
    }

    /*
     * RN04: a senha tem que ter 16 caracteres.
     */
    [Fact]
    public void Rn04_SenhaGerada_DeveTer16Caracteres()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            Assert.True(
                senha.Length == TAMANHO_SENHA,
                $"A senha '{senha}' possui {senha.Length} caracteres, diferente de {TAMANHO_SENHA}.");
        }
    }

    /*
     * RN04: a senha não pode incluir espaços em branco.
     */
    [Fact]
    public void Rn04_SenhaGerada_NaoDeveTerEspacosEmBranco()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            Assert.False(senha.Any(char.IsWhiteSpace), $"A senha '{senha}' possui espaços em branco.");
        }
    }

    /*
     * Considera símbolo (caractere especial) todo caractere que não é letra,
     * número nem espaço em branco.
     */
    private static bool EhSimbolo(char caractere)
    {
        return !char.IsLetterOrDigit(caractere) && !char.IsWhiteSpace(caractere);
    }

    /*
     * Extrai todas as sequências contíguas de TAMANHO_SEQUENCIA caracteres da senha.
     */
    private static List<string> ExtrairSequencias(string senha)
    {
        List<string> sequencias = new List<string>();

        for (int inicio = 0; inicio + TAMANHO_SEQUENCIA <= senha.Length; inicio++)
        {
            sequencias.Add(senha.Substring(inicio, TAMANHO_SEQUENCIA));
        }

        return sequencias;
    }

    /*
     * Indica se duas senhas coincidem em alguma sequência de valores,
     * considerando também a ordem inversa.
     */
    private static bool CoincidemEmAlgumaSequencia(string primeiraSenha, string segundaSenha)
    {
        List<string> sequenciasPrimeira = ExtrairSequencias(primeiraSenha);

        return ExtrairSequencias(segundaSenha).Any(sequencia =>
            sequenciasPrimeira.Contains(sequencia) || sequenciasPrimeira.Contains(Inverter(sequencia)));
    }

    /*
     * Retorna a sequência com os caracteres em ordem inversa.
     */
    private static string Inverter(string sequencia)
    {
        char[] caracteres = sequencia.ToCharArray();
        Array.Reverse(caracteres);

        return new string(caracteres);
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
