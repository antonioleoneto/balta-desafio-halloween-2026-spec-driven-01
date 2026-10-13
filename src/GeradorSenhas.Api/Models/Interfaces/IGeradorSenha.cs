namespace GeradorSenhas.Api.Models.Interfaces;

/*
 * Contrato do gerador de senhas. A geração é regra de negócio,
 * por isso vive na camada de Modelo e não no controller.
 */
public interface IGeradorSenha
{
    /*
     * Gera o valor de uma nova senha.
     */
    string Gerar();
}
