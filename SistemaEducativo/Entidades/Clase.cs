using SQLite;

namespace SistemaEducativo.Entidades;

[Table("Clases")]
public class Clase
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int ComisionId { get; set; }

    [NotNull]
    public DateTime Fecha { get; set; }

    // Para mostrar en pantalla. [Ignore]: no es una columna de la tabla.
    [Ignore]
    public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");
}
