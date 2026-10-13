namespace GeradorSenhas.Api.Models;

/*
 * Resposta do endpoint de busca de senha pelo GUID.
 */
public record BuscarSenhaResponse(Guid Id, string Valor);
