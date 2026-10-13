using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Models.Exceptions;
using GeradorSenhas.Api.Services.Interfaces;

namespace GeradorSenhas.Tests.Fakes;

/*
 * Serviço de senhas falso para testes do controller.
 * Permite configurar a senha retornada e se a criação deve falhar,
 * isolando o controller do banco de dados e do gerador.
 */
public class SenhaServiceFake : ISenhaService
{
    public Senha? Senha { get; set; }

    public string? MensagemErroCriacao { get; set; }

    /*
     * Retorna a senha configurada ou lança SenhaInvalidaException
     * quando MensagemErroCriacao estiver preenchida.
     */
    public Task<Senha> CriarAsync(CancellationToken cancellationToken)
    {
        if (MensagemErroCriacao is not null)
        {
            throw new SenhaInvalidaException(MensagemErroCriacao);
        }

        return Task.FromResult(Senha ?? throw new InvalidOperationException("Senha não configurada no fake."));
    }

    /*
     * Retorna a senha configurada quando o GUID corresponde, ou null caso contrário.
     */
    public Task<Senha?> BuscarAsync(Guid id, CancellationToken cancellationToken)
    {
        Senha? senha = Senha is not null && Senha.Id == id ? Senha : null;

        return Task.FromResult(senha);
    }
}
