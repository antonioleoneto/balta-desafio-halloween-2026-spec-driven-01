using GeradorSenhas.Api.Models.Domain;

namespace GeradorSenhas.Tests.Models;

/*
 * Testes unitários da regra de negócio RN01 na entidade Senha:
 * "Todas as senhas criadas devem retornar um GUID de identificação".
 */
public class SenhaTests
{
    private const string VALOR_SENHA = "Valor-De-Teste!1";

    /*
     * RN01: toda senha criada deve possuir um GUID de identificação preenchido.
     */
    [Fact]
    public void Rn01_SenhaCriada_DevePossuirGuidDeIdentificacao()
    {
        Senha senha = new Senha(VALOR_SENHA);

        Assert.NotEqual(Guid.Empty, senha.Id);
    }

    /*
     * RN01: cada senha criada deve possuir um GUID próprio, que a identifique de forma única.
     */
    [Fact]
    public void Rn01_SenhasCriadas_DevemPossuirGuidsDistintos()
    {
        Senha primeira = new Senha(VALOR_SENHA);
        Senha segunda = new Senha(VALOR_SENHA);

        Assert.NotEqual(primeira.Id, segunda.Id);
    }
}
