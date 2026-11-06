using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

// Cómo se guarda una Comision en SQLite. La entidad del dominio no sabe nada de esto.
[Table("Comisiones")]
public class ComisionRegistro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Unique, NotNull]
    public string Nombre { get; set; } = string.Empty;

    public int CicloLectivo { get; set; }

    public int Cuatrimestre { get; set; }

    public Comision ToEntidad() => new()
    {
        Id = Id,
        Nombre = Nombre,
        CicloLectivo = CicloLectivo,
        Cuatrimestre = Cuatrimestre
    };

    public static ComisionRegistro Desde(Comision c) => new()
    {
        Id = c.Id,
        Nombre = c.Nombre,
        CicloLectivo = c.CicloLectivo,
        Cuatrimestre = c.Cuatrimestre
    };
}
