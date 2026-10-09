using SQLite;

namespace SistemaEducativo.Entidades;

[Table("Comisiones")]
public class Comision
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique, NotNull]
    public string Nombre { get; set; } = string.Empty;

    public int CicloLectivo { get; set; }

    public int Cuatrimestre { get; set; }
}
