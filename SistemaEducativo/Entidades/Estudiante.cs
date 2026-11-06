namespace SistemaEducativo.Entidades;

public class Estudiante
{
    public int Id { get; set; }

    public int ComisionId { get; set; }

    public string Legajo { get; set; } = string.Empty;

    public string Apellido { get; set; } = string.Empty;

    public string Nombre { get; set; } = string.Empty;

    public DateTime FechaIncorporacion { get; set; }

    // Dato calculado para mostrar en listas: no se guarda en la base.
    public string NombreCompleto => $"{Apellido}, {Nombre}";
}
