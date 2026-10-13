using GeradorSenhas.Api.Models.Domain;

namespace GeradorSenhas.Tests.Models;

/*
 * Testes unitários das regras de negócio RN02 e RN03 no GeradorSenha.
 * Como a geração é aleatória, cada regra é verificada sobre um grande número de
 * senhas geradas: uma implementação correta passa em todas as execuções,
 * mantendo os testes repetíveis e reprodutíveis.
 */
public class GeradorSenhaTests
{
    private const int QUANTIDADE_GERACOES = 1000;
    private const int TAMANHO_SENHA = 16;

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
     * RN03: a senha tem que ter caracteres especiais.
     */
    [Fact]
    public void Rn03_SenhaGerada_DeveTerCaracteresEspeciais()
    {
        for (int i = 0; i < QUANTIDADE_GERACOES; i++)
        {
            string senha = _geradorSenha.Gerar();

            Assert.True(senha.Any(EhSimbolo), $"A senha '{senha}' não possui caracteres especiais.");
        }
    }

    /*
     * RN03: a senha tem que ter 16 caracteres.
     */
    [Fact]
    public void Rn03_SenhaGerada_DeveTer16Caracteres()
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
     * RN03: a senha não pode incluir espaços em branco.
     */
    [Fact]
    public void Rn03_SenhaGerada_NaoDeveTerEspacosEmBranco()
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
}
