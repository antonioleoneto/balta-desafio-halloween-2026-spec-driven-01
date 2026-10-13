namespace GeradorSenhas.Api.Models.Domain;

/*
 * Entidade de domínio que representa uma senha gerada.
 * O identificador (GUID) é atribuído no momento da criação,
 * garantindo que toda senha possua um GUID de identificação.
 */
public class Senha
{
    public Guid Id { get; private set; }

    public string Valor { get; private set; }

    /*
     * Cria uma nova senha com um GUID de identificação.
     */
    public Senha(string valor)
    {
        Id = Guid.NewGuid();
        Valor = valor;
    }
}
