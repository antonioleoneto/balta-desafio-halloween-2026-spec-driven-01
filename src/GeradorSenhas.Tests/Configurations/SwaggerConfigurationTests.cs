using GeradorSenhas.Api.Configurations;
using GeradorSenhas.Api.Controllers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.Swagger;

namespace GeradorSenhas.Tests.Configurations;

/*
 * Testes unitários da configuração do Swagger, decisão do plano técnico
 * para testar os endpoints da API no navegador.
 */
public class SwaggerConfigurationTests
{
    /*
     * O documento OpenAPI gerado deve conter os dois endpoints do contrato:
     * /criar-senha e /buscar-senha/{guid}.
     */
    [Fact]
    public void AddSwagger_DeveGerarDocumentoComOsEndpointsDoContrato()
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder();
        builder.Services.AddControllers().AddApplicationPart(typeof(SenhaController).Assembly);
        builder.Services.AddSwagger();
        using WebApplication app = builder.Build();

        ISwaggerProvider swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
        OpenApiDocument documento = swaggerProvider.GetSwagger(SwaggerConfiguration.DOCUMENTO_NOME);

        Assert.Equal(SwaggerConfiguration.DOCUMENTO_TITULO, documento.Info.Title);
        Assert.Contains("/criar-senha", documento.Paths.Keys);
        Assert.Contains("/buscar-senha/{guid}", documento.Paths.Keys);
    }

    /*
     * Em ambiente de desenvolvimento o Swagger deve ser habilitado no pipeline.
     */
    [Fact]
    public void UseSwaggerEmDesenvolvimento_AmbienteDesenvolvimento_DeveHabilitar()
    {
        using WebApplication app = CriarAplicacao(Environments.Development);

        bool habilitado = app.UseSwaggerEmDesenvolvimento();

        Assert.True(habilitado);
    }

    /*
     * Fora do ambiente de desenvolvimento o Swagger não deve ser exposto.
     */
    [Fact]
    public void UseSwaggerEmDesenvolvimento_AmbienteProducao_NaoDeveHabilitar()
    {
        using WebApplication app = CriarAplicacao(Environments.Production);

        bool habilitado = app.UseSwaggerEmDesenvolvimento();

        Assert.False(habilitado);
    }

    /*
     * Cria uma aplicação com o Swagger registrado no ambiente informado.
     */
    private static WebApplication CriarAplicacao(string ambiente)
    {
        WebApplicationBuilder builder = WebApplication.CreateBuilder(
            new WebApplicationOptions { EnvironmentName = ambiente });
        builder.Services.AddSwagger();

        return builder.Build();
    }
}
