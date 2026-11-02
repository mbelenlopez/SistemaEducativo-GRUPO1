using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

[Table("Trabajos")]
public class TrabajoRegistro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int ComisionId { get; set; }

    [NotNull]
    public string Titulo { get; set; } = string.Empty;

    public DateTime FechaLimite { get; set; }

    public Trabajo ToEntidad() => new() { Id = Id, ComisionId = ComisionId, Titulo = Titulo, FechaLimite = FechaLimite };

    public static TrabajoRegistro Desde(Trabajo t) =>
        new() { Id = t.Id, ComisionId = t.ComisionId, Titulo = t.Titulo, FechaLimite = t.FechaLimite };
}
