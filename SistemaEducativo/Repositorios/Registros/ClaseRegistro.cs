using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

[Table("Clases")]
public class ClaseRegistro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int ComisionId { get; set; }

    [NotNull]
    public DateTime Fecha { get; set; }

    public Clase ToEntidad() => new() { Id = Id, ComisionId = ComisionId, Fecha = Fecha };

    public static ClaseRegistro Desde(Clase c) => new() { Id = c.Id, ComisionId = c.ComisionId, Fecha = c.Fecha };
}
