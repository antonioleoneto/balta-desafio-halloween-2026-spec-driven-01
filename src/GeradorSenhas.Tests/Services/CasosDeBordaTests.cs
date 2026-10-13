using GeradorSenhas.Api.Data;
using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Services.Implementations;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace GeradorSenhas.Tests.Services;

/*
 * Testes unitários dos casos de borda da especificação.
 * Exercitam o fluxo real de criação de senha (SenhaService + GeradorSenha)
 * com SQLite em memória. Como a geração é aleatória, cada caso é verificado
 * sobre um grande número de senhas criadas: uma implementação correta passa
 * em todas as execuções, mantendo os testes repetíveis e reprodutíveis.
 */
public class CasosDeBordaTests : IDisposable
{
    private const int QUANTIDADE_CRIACOES = 1000;
    private const int TAMANHO_SENHA = 16;

    private readonly SqliteConnection _connection;
    private readonly AppDbContext _context;
    private readonly SenhaService _senhaService;

    /*
     * Abre uma conexão SQLite em memória, cria o schema e instancia o serviço
     * com o gerador de senhas real.
     */
    public CasosDeBordaTests()
    {
        _connection = new SqliteConnection("DataSource=:memory:");
        _connection.Open();

        DbContextOptions<AppDbContext> options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite(_connection)
            .Options;

        _context = new AppDbContext(options);
        _context.Database.EnsureCreated();

        _senhaService = new SenhaService(_context, new GeradorSenha());
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
     * Caso de borda: não deve ser possível criar uma senha com menos de 16 caracteres.
     */
    [Fact]
    public async Task CriarAsync_SenhaCriada_NaoDeveTerMenosDe16Caracteres()
    {
        for (int i = 0; i < QUANTIDADE_CRIACOES; i++)
        {
            Senha senha = await _senhaService.CriarAsync(CancellationToken.None);

            Assert.False(
                senha.Valor.Length < TAMANHO_SENHA,
                $"A senha '{senha.Valor}' possui {senha.Valor.Length} caracteres, menos de {TAMANHO_SENHA}.");
        }
    }

    /*
     * Caso de borda: não deve ser possível criar uma senha com mais de 16 caracteres.
     */
    [Fact]
    public async Task CriarAsync_SenhaCriada_NaoDeveTerMaisDe16Caracteres()
    {
        for (int i = 0; i < QUANTIDADE_CRIACOES; i++)
        {
            Senha senha = await _senhaService.CriarAsync(CancellationToken.None);

            Assert.False(
                senha.Valor.Length > TAMANHO_SENHA,
                $"A senha '{senha.Valor}' possui {senha.Valor.Length} caracteres, mais de {TAMANHO_SENHA}.");
        }
    }

    /*
     * Caso de borda: não deve ter espaços em branco em uma senha.
     */
    [Fact]
    public async Task CriarAsync_SenhaCriada_NaoDeveTerEspacosEmBranco()
    {
        for (int i = 0; i < QUANTIDADE_CRIACOES; i++)
        {
            Senha senha = await _senhaService.CriarAsync(CancellationToken.None);

            Assert.False(
                senha.Valor.Any(char.IsWhiteSpace),
                $"A senha '{senha.Valor}' possui espaços em branco.");
        }
    }

    /*
     * Caso de borda: ao criar uma senha, não existe a possibilidade de não ser
     * retornado um GUID de identificação. O GUID retornado deve estar preenchido
     * e identificar a senha persistida.
     */
    [Fact]
    public async Task CriarAsync_SenhaCriada_DeveSempreRetornarGuidDeIdentificacao()
    {
        for (int i = 0; i < QUANTIDADE_CRIACOES; i++)
        {
            Senha senha = await _senhaService.CriarAsync(CancellationToken.None);

            Assert.NotEqual(Guid.Empty, senha.Id);

            Senha? persistida = await _senhaService.BuscarAsync(senha.Id, CancellationToken.None);

            Assert.NotNull(persistida);
            Assert.Equal(senha.Valor, persistida.Valor);
        }
    }
}
