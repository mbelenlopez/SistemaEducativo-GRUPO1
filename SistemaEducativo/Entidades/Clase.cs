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
    public string Fecha { get; set; } = string.Empty;
}
