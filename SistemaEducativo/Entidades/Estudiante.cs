using SQLite;

namespace SistemaEducativo.Entidades;

[Table("Estudiantes")]
public class Estudiante
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int ComisionId { get; set; }

    [NotNull]
    public string Legajo { get; set; } = string.Empty;

    [NotNull]
    public string Apellido { get; set; } = string.Empty;

    [NotNull]
    public string Nombre { get; set; } = string.Empty;

    public DateTime FechaIncorporacion { get; set; }

    // Dato calculado para mostrar en listas. [Ignore]: no es una columna de la tabla.
    [Ignore]
    public string NombreCompleto => $"{Apellido}, {Nombre}";
}
