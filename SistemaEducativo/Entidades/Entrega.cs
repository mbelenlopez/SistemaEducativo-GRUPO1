using SQLite;

namespace SistemaEducativo.Entidades;

[Table("Entregas")]
public class Entrega
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int TrabajoId { get; set; }

    [Indexed, NotNull]
    public int EstudianteId { get; set; }

    // Condición del dominio: "Entregado" o "No entregado".
    [NotNull]
    public string Condicion { get; set; } = "No entregado";

    [Ignore]
    public bool Entregado => Condicion == "Entregado";
}
