using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

[Table("Contactos")]
public class ContactoRegistro
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed, NotNull]
    public int EstudianteId { get; set; }

    [NotNull]
    public DateTime Fecha { get; set; }

    // Se agregó con H10. Sin [NotNull]: los contactos viejos no tienen tipo.
    public string Tipo { get; set; } = string.Empty;

    [NotNull]
    public string Resultado { get; set; } = string.Empty;

    public Contacto ToEntidad() => new()
    {
        Id = Id,
        EstudianteId = EstudianteId,
        Fecha = Fecha,
        Tipo = Tipo ?? string.Empty,
        Resultado = Resultado
    };

    public static ContactoRegistro Desde(Contacto c) => new()
    {
        Id = c.Id,
        EstudianteId = c.EstudianteId,
        Fecha = c.Fecha,
        Tipo = c.Tipo,
        Resultado = c.Resultado
    };
}
