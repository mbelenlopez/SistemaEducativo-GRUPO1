using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

[Table("Entregas")]
public class EntregaRegistro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int TrabajoId { get; set; }

    [Indexed, NotNull]
    public int EstudianteId { get; set; }

    [NotNull]
    public string Condicion { get; set; } = CondicionEntrega.NoEntregado;

    public Entrega ToEntidad() => new()
    {
        Id = Id,
        TrabajoId = TrabajoId,
        EstudianteId = EstudianteId,
        Condicion = Condicion
    };

    public static EntregaRegistro Desde(Entrega e) => new()
    {
        Id = e.Id,
        TrabajoId = e.TrabajoId,
        EstudianteId = e.EstudianteId,
        Condicion = e.Condicion
    };
}
