namespace SistemaEducativo.Entidades;

// Un contacto del tutor con un estudiante (llamada, mensaje, email).
// H9 los muestra en la ficha; H10 los registra.
public class Contacto
{
    public int Id { get; set; }

    public int EstudianteId { get; set; }

    public DateTime Fecha { get; set; }

    // Una de las constantes de TiposContacto.
    public string Tipo { get; set; } = string.Empty;

    public string Resultado { get; set; } = string.Empty;
}
