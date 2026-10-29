using SQLite;

namespace SistemaEducativo.Entidades;

// Un contacto del tutor con un estudiante (llamada, mensaje, etc.).
// H9 solo los LEE para mostrarlos en la ficha; H10 es la que los registra.
[Table("Contactos")]
public class Contacto
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int EstudianteId { get; set; }

    [NotNull]
    public DateTime Fecha { get; set; }

    public string Tipo { get; set; } = string.Empty;

    [NotNull]
    public string Resultado { get; set; } = string.Empty;
}
