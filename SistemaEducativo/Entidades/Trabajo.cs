using SQLite;

namespace SistemaEducativo.Entidades;

public class Trabajo
{
    public int Id { get; set; }

    public int ComisionId { get; set; }

    public string Titulo { get; set; } = string.Empty;

    public DateTime FechaLimite { get; set; }
}
