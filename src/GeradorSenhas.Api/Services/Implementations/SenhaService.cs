using GeradorSenhas.Api.Data;
using GeradorSenhas.Api.Models.Domain;
using GeradorSenhas.Api.Models.Interfaces;
using GeradorSenhas.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GeradorSenhas.Api.Services.Implementations;

/*
 * Implementação do serviço de aplicação de senhas.
 * Delega a geração ao Modelo (IGeradorSenha) e a persistência ao AppDbContext.
 */
public class SenhaService : ISenhaService
{
    private readonly AppDbContext _context;
    private readonly IGeradorSenha _geradorSenha;

    /*
     * Cria o serviço com o contexto de dados e o gerador de senhas.
     */
    public SenhaService(AppDbContext context, IGeradorSenha geradorSenha)
    {
        _context = context;
        _geradorSenha = geradorSenha;
    }

    /*
     * Gera uma nova senha pelo Modelo, persiste e a retorna com seu GUID.
     */
    public async Task<Senha> CriarAsync(CancellationToken cancellationToken)
    {
        Senha senha = new Senha(_geradorSenha.Gerar());

        _context.Senhas.Add(senha);
        await _context.SaveChangesAsync(cancellationToken);

        return senha;
    }

    /*
     * Busca uma senha pelo GUID sem rastreamento de alterações.
     */
    public async Task<Senha?> BuscarAsync(Guid id, CancellationToken cancellationToken)
    {
        return await _context.Senhas
            .AsNoTracking()
            .FirstOrDefaultAsync(senha => senha.Id == id, cancellationToken);
    }
}
