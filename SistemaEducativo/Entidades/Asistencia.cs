using SQLite;

namespace SistemaEducativo.Entidades;

[Table("Asistencias")]
public class Asistencia
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int ClaseId { get; set; }

    [Indexed, NotNull]
    public int EstudianteId { get; set; }

    // Condición del dominio: "Presente" o "Ausente".
    [NotNull]
    public string Condicion { get; set; } = "Ausente";

    [Ignore]
    public bool Presente => Condicion == "Presente";
}
