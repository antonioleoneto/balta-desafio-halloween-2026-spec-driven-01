using GeradorSenhas.Api.Data;
using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Services.Implementations;
using GeradorSenhas.Tests.Fakes;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GeradorSenhas.Tests.Services;

/*
 * Testes unitários do SenhaService.
 * Usa SQLite em memória (mesmo provedor do plano técnico) e um gerador falso,
 * garantindo isolamento e resultados repetíveis entre execuções.
 */
public class SenhaServiceTests : IDisposable
{
    private const string VALOR_SENHA = "Valor-De-Teste!1";

    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly SenhaService _senhaService;

    /*
     * Abre uma conexão SQLite em memória, cria o schema e instancia o serviço.
     */
    public SenhaServiceTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _senhaService = new SenhaService(_context, new GeradorSenhaFake(VALOR_SENHA));
    }

    /*
     * Libera o contexto e a conexão em memória ao final de cada teste.
     */
    public void Dispose()
    {
        _context.Dispose();
        _connection.Dispose();
        GC.SuppressFinalize(this);
    }

    /*
     * CriarAsync deve retornar a senha gerada pelo Modelo com um GUID preenchido.
     */
    [Fact]
    public async Task CriarAsync_DeveRetornarSenhaComGuidEValorGerado()
    {
        Senha senha = await _senhaService.CriarAsync(CancellationToken.None);

        Assert.NotEqual(Guid.Empty, senha.Id);
        Assert.Equal(VALOR_SENHA, senha.Valor);
    }

    /*
     * CriarAsync deve persistir a senha no banco.
     */
    [Fact]
    public async Task CriarAsync_DevePersistirSenhaNoBanco()
    {
        Senha senha = await _senhaService.CriarAsync(CancellationToken.None);

        Senha? persistida = await _context.Senhas
            .AsNoTracking()
            .FirstOrDefaultAsync(item => item.Id == senha.Id);

        Assert.NotNull(persistida);
        Assert.Equal(VALOR_SENHA, persistida.Valor);
    }

    /*
     * BuscarAsync deve retornar a senha quando o GUID existe.
     */
    [Fact]
    public async Task BuscarAsync_GuidExistente_DeveRetornarSenha()
    {
        Senha criada = await _senhaService.CriarAsync(CancellationToken.None);

        Senha? encontrada = await _senhaService.BuscarAsync(criada.Id, CancellationToken.None);

        Assert.NotNull(encontrada);
        Assert.Equal(criada.Id, encontrada.Id);
        Assert.Equal(criada.Valor, encontrada.Valor);
    }

    /*
     * BuscarAsync deve retornar null quando o GUID não existe.
     */
    [Fact]
    public async Task BuscarAsync_GuidInexistente_DeveRetornarNull()
    {
        Senha? encontrada = await _senhaService.BuscarAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(encontrada);
    }
}
