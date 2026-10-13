using GeradorSenhas.Api.Models;

namespace GeradorSenhas.Api.Services;

/*
 * Serviço de aplicação responsável por orquestrar a criação
 * e a busca de senhas. Único ponto de acesso ao banco para os endpoints.
 */
public interface ISenhaService
{
    /*
     * Gera, persiste e retorna uma nova senha.
     */
    Task<Senha> CriarAsync(CancellationToken cancellationToken);

    /*
     * Busca uma senha pelo GUID. Retorna null quando não encontrada.
     */
    Task<Senha?> BuscarAsync(Guid id, CancellationToken cancellationToken);
}
