using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Models.Exceptions;
using GeradorSenhas.Api.Models.Responses;
using GeradorSenhas.Api.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GeradorSenhas.Api.Controllers;

/*
 * Controller que expõe os endpoints de criação e busca de senhas.
 * Não contém regra de negócio: apenas traduz o resultado do serviço em respostas HTTP.
 */
[ApiController]
public class SenhaController : ControllerBase
{
    private readonly ISenhaService _senhaService;

    /*
     * Cria o controller com o serviço de aplicação de senhas.
     */
    public SenhaController(ISenhaService senhaService)
    {
        _senhaService = senhaService;
    }

    /*
     * Cria uma nova senha e retorna 201 com o GUID de identificação.
     * Em caso de falha de validação, retorna 400 com ProblemDetails.
     */
    [HttpPost("criar-senha")]
    [ProducesResponseType<CriarSenhaResponse>(StatusCodes.Status201Created)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CriarSenha(CancellationToken cancellationToken)
    {
        try
        {
            Senha senha = await _senhaService.CriarAsync(cancellationToken);

            return CreatedAtAction(
                nameof(BuscarSenha),
                new { guid = senha.Id },
                new CriarSenhaResponse(senha.Id));
        }
        catch (SenhaInvalidaException exception)
        {
            return Problem(
                detail: exception.Message,
                statusCode: StatusCodes.Status400BadRequest,
                title: "Não foi possível criar a senha.");
        }
    }

    /*
     * Busca uma senha pelo GUID. Retorna 200 quando encontrada e 404 caso contrário.
     */
    [HttpGet("buscar-senha/{guid:guid}")]
    [ProducesResponseType<BuscarSenhaResponse>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> BuscarSenha(Guid guid, CancellationToken cancellationToken)
    {
        Senha? senha = await _senhaService.BuscarAsync(guid, cancellationToken);

        if (senha is null)
        {
            return NotFound();
        }

        return Ok(new BuscarSenhaResponse(senha.Id, senha.Valor));
    }
}
