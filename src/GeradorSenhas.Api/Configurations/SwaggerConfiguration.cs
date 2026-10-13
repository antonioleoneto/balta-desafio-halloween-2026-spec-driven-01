using Microsoft.OpenApi;

namespace GeradorSenhas.Api.Configurations;

/*
 * Configuração do Swagger, conforme a decisão do plano técnico:
 * "O Swagger deve ser implementado para testar os endpoints da API".
 * Gera o documento OpenAPI dos controllers e expõe a interface Swagger UI
 * em /swagger, apenas no ambiente de desenvolvimento, para não publicar
 * a documentação da API em produção.
 */
public static class SwaggerConfiguration
{
    public const string DOCUMENTO_NOME = "v1";
    public const string DOCUMENTO_TITULO = "Gerador de Senhas Fortes";

    /*
     * Registra o explorador de endpoints e o gerador do documento OpenAPI.
     */
    public static IServiceCollection AddSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
            options.SwaggerDoc(DOCUMENTO_NOME, new OpenApiInfo
            {
                Title = DOCUMENTO_TITULO,
                Version = DOCUMENTO_NOME
            }));

        return services;
    }

    /*
     * Expõe o documento OpenAPI e a interface Swagger UI no pipeline HTTP,
     * somente quando a aplicação está em ambiente de desenvolvimento.
     * Retorna true quando o Swagger foi habilitado.
     */
    public static bool UseSwaggerEmDesenvolvimento(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
        {
            return false;
        }

        app.UseSwagger();
        app.UseSwaggerUI(options =>
            options.SwaggerEndpoint($"/swagger/{DOCUMENTO_NOME}/swagger.json", DOCUMENTO_TITULO));

        return true;
    }
}
