using GeradorSenhas.Api.Models.Interfaces;

namespace GeradorSenhas.Api.Models.Domain;

/*
 * Implementação do gerador de senhas.
 * As regras de negócio RN02, RN03 e RN04 serão implementadas na tarefa T-05.
 */
public class GeradorSenha : IGeradorSenha
{
    /*
     * Gera o valor de uma nova senha.
     */
    public string Gerar()
    {
        throw new NotImplementedException("Geração de senha pendente da tarefa T-05.");
    }
}
