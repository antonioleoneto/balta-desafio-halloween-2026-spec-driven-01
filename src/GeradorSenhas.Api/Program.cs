using GeradorSenhas.Api.Configurations;
using GeradorSenhas.Api.Data;
using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Models.Interfaces;
using GeradorSenhas.Api.Services.Implementations;
using GeradorSenhas.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

/*
 * Ponto de entrada da API do Gerador de Senhas Fortes.
 * Configura o container de injeção de dependência e o pipeline HTTP
 * seguindo a arquitetura MVC sem camada de Visão.
 */
WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddSwagger();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("GeradorSenhas")));

builder.Services.AddSingleton<IGeradorSenha, GeradorSenha>();
builder.Services.AddScoped<ISenhaService, SenhaService>();

WebApplication app = builder.Build();

using (IServiceScope scope = app.Services.CreateScope())
{
    AppDbContext context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    context.Database.EnsureCreated();
}

app.UseExceptionHandler();
app.UseStatusCodePages();

app.UseSwaggerEmDesenvolvimento();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
