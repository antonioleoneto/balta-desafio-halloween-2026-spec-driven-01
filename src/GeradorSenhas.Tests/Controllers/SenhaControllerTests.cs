using GeradorSenhas.Api.Controllers;
using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Models.Responses;
using GeradorSenhas.Tests.Fakes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GeradorSenhas.Tests.Controllers;

/*
 * Testes unitários do SenhaController.
 * Para cada endpoint há um teste do caminho de sucesso e um do caminho de erro,
 * verificando o status HTTP proposto, conforme a constituição.
 */
public class SenhaControllerTests
{
    private const string VALOR_SENHA = "Valor-De-Teste!1";

    private readonly SenhaServiceFake _senhaService;
    private readonly SenhaController _controller;

    /*
     * Instancia o controller com o serviço falso.
     */
    public SenhaControllerTests()
    {
        _senhaService = new SenhaServiceFake();
        _controller = new SenhaController(_senhaService);
    }

    /*
     * POST /criar-senha com sucesso deve retornar 201 com o GUID da senha
     * e sem expor o valor da senha.
     */
    [Fact]
    public async Task CriarSenha_Sucesso_DeveRetornar201ComGuid()
    {
        Senha senha = new Senha(VALOR_SENHA);
        _senhaService.Senha = senha;

        IActionResult resultado = await _controller.CriarSenha(CancellationToken.None);

        CreatedAtActionResult created = Assert.IsType<CreatedAtActionResult>(resultado);
        Assert.Equal(StatusCodes.Status201Created, created.StatusCode);
        Assert.Equal(nameof(SenhaController.BuscarSenha), created.ActionName);
        Assert.Equal(senha.Id, created.RouteValues?["guid"]);
        CriarSenhaResponse response = Assert.IsType<CriarSenhaResponse>(created.Value);
        Assert.Equal(senha.Id, response.Id);
    }

    /*
     * POST /criar-senha com falha de validação deve retornar 400 com ProblemDetails
     * explicando o motivo da falha.
     */
    [Fact]
    public async Task CriarSenha_Erro_DeveRetornar400ComProblemDetails()
    {
        const string MENSAGEM_ERRO = "A senha gerada não atende aos requisitos.";
        _senhaService.MensagemErroCriacao = MENSAGEM_ERRO;

        IActionResult resultado = await _controller.CriarSenha(CancellationToken.None);

        ObjectResult objectResult = Assert.IsType<ObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status400BadRequest, objectResult.StatusCode);
        ProblemDetails problemDetails = Assert.IsType<ProblemDetails>(objectResult.Value);
        Assert.Equal(StatusCodes.Status400BadRequest, problemDetails.Status);
        Assert.Equal(MENSAGEM_ERRO, problemDetails.Detail);
    }

    /*
     * GET /buscar-senha/{guid} com GUID existente deve retornar 200 com a senha.
     */
    [Fact]
    public async Task BuscarSenha_Sucesso_DeveRetornar200ComSenha()
    {
        Senha senha = new Senha(VALOR_SENHA);
        _senhaService.Senha = senha;

        IActionResult resultado = await _controller.BuscarSenha(senha.Id, CancellationToken.None);

        OkObjectResult ok = Assert.IsType<OkObjectResult>(resultado);
        Assert.Equal(StatusCodes.Status200OK, ok.StatusCode);
        BuscarSenhaResponse response = Assert.IsType<BuscarSenhaResponse>(ok.Value);
        Assert.Equal(senha.Id, response.Id);
        Assert.Equal(VALOR_SENHA, response.Valor);
    }

    /*
     * GET /buscar-senha/{guid} com GUID inexistente deve retornar 404.
     */
    [Fact]
    public async Task BuscarSenha_Erro_DeveRetornar404()
    {
        IActionResult resultado = await _controller.BuscarSenha(Guid.NewGuid(), CancellationToken.None);

        NotFoundResult notFound = Assert.IsType<NotFoundResult>(resultado);
        Assert.Equal(StatusCodes.Status404NotFound, notFound.StatusCode);
    }
}
