using GeradorSenhas.Api.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace GeradorSenhas.Api.Data;

/*
 * Contexto do EF Core para persistência relacional em SQLite.
 */
public class AppDbContext : DbContext
{
    public DbSet<Senha> Senhas => Set<Senha>();

    /*
     * Cria o contexto com as opções configuradas na injeção de dependência.
     */
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    /*
     * Mapeia a entidade Senha: chave primária pelo GUID e valor obrigatório e único.
     */
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Senha>(entity =>
        {
            entity.HasKey(senha => senha.Id);
            entity.Property(senha => senha.Valor).IsRequired();
            entity.HasIndex(senha => senha.Valor).IsUnique();
        });
    }
}
