using SQLite;

namespace SistemaEducativo.Entidades;

[Table("Trabajos")]
public class Trabajo
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int ComisionId { get; set; }

    [NotNull]
    public string Titulo { get; set; } = string.Empty;

    public DateTime FechaLimite { get; set; }
}
