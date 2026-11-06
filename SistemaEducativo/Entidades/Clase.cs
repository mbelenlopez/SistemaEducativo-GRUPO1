using SQLite;

namespace SistemaEducativo.Entidades;

public class Clase
{
    public int Id { get; set; }

    public int ComisionId { get; set; }

    public DateTime Fecha { get; set; }

    // Para mostrar en pantalla. [Ignore]: no es una columna de la tabla.
    [Ignore]
    public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");
}
