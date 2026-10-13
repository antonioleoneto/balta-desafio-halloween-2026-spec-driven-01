namespace GeradorSenhas.Api.Models.Exceptions;

/*
 * Exceção de domínio lançada quando uma senha não pode ser criada.
 * A mensagem deve explicar de maneira clara o motivo da falha.
 */
public class SenhaInvalidaException : Exception
{
    /*
     * Cria a exceção com a mensagem que descreve o motivo da falha.
     */
    public SenhaInvalidaException(string message) : base(message)
    {
    }
}
