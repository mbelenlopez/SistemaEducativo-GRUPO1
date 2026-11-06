using SQLite;

namespace SistemaEducativo.Entidades;

public class Asistencia
{
    public int Id { get; set; }

    public int ClaseId { get; set; }

    public int EstudianteId { get; set; }

    // Condición del dominio: "Presente" o "Ausente".
    public string Condicion { get; set; } = CondicionAsistencia.Ausente;

    [Ignore]
    public bool Presente => Condicion == CondicionAsistencia.Presente;
}
