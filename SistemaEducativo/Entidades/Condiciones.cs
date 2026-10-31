namespace SistemaEducativo.Entidades;

// Los valores posibles de "Condicion", escritos UNA sola vez.
// Se guardan en la base como texto, con exactamente estos valores.
public static class CondicionAsistencia
{
    public const string Presente = "Presente";
    public const string Ausente = "Ausente";
}

public static class CondicionEntrega
{
    public const string Entregado = "Entregado";
    public const string NoEntregado = "No entregado";
}
