namespace GeradorSenhas.Api.Models.Responses;

/*
 * Resposta do endpoint de criação de senha.
 * Expõe apenas o GUID, evitando exibir a senha sem necessidade.
 */
public record CriarSenhaResponse(Guid Id);
