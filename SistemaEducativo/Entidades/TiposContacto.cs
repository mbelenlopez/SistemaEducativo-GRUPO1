namespace SistemaEducativo.Entidades;

// Las formas de contacto que ofrece la pantalla.
public static class TiposContacto
{
    public const string Llamada = "Llamada";
    public const string Mensaje = "Mensaje";
    public const string Email = "Email";

    public static readonly IReadOnlyList<string> Todos = new[] { Llamada, Mensaje, Email };
}