using GeradorSenhas.Api.Models.Interfaces;

namespace GeradorSenhas.Tests.Fakes;

/*
 * Gerador de senhas falso para testes.
 * Retorna sempre o valor configurado, tornando os testes repetíveis e reprodutíveis.
 */
public class GeradorSenhaFake : IGeradorSenha
{
    private readonly string _valor;

    /*
     * Cria o gerador falso com o valor que será retornado em Gerar.
     */
    public GeradorSenhaFake(string valor)
    {
        _valor = valor;
    }

    /*
     * Retorna o valor configurado no construtor.
     */
    public string Gerar()
    {
        return _valor;
    }
}
