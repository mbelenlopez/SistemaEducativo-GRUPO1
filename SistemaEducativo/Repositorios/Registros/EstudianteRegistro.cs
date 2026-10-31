using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

[Table("Estudiantes")]
public class EstudianteRegistro
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

    public Estudiante ToEntidad() => new()
    {
        Id = Id,
        ComisionId = ComisionId,
        Legajo = Legajo,
        Apellido = Apellido,
        Nombre = Nombre,
        FechaIncorporacion = FechaIncorporacion
    };

    public static EstudianteRegistro Desde(Estudiante e) => new()
    {
        Id = e.Id,
        ComisionId = e.ComisionId,
        Legajo = e.Legajo,
        Apellido = e.Apellido,
        Nombre = e.Nombre,
        FechaIncorporacion = e.FechaIncorporacion
    };
}
