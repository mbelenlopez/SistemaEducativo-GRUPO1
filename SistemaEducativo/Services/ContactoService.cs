using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Services;

public class ContactoService
{
    // H11: un contacto de hasta estos días cuenta como "reciente" (la historia habla de "esta semana").
    public const int DiasContactoReciente = 7;
    private readonly ContactoRepositorio _contactos;

    public ContactoService(ContactoRepositorio contactos) => _contactos = contactos;

    public Task<List<Contacto>> ObtenerPorEstudianteAsync(int estudianteId) =>
        _contactos.ObtenerPorEstudianteAsync(estudianteId);

    // H10: registra un contacto con un estudiante.
    public async Task<(bool Exito, string Mensaje)> RegistrarAsync(
        int estudianteId, DateTime fecha, string tipo, string resultado)
    {
        if (!TiposContacto.Todos.Contains(tipo))
            return (false, "Elegí cómo fue el contacto: llamada, mensaje o email.");

        if (string.IsNullOrWhiteSpace(resultado))
            return (false, "Escribí el resultado del contacto.");

        await _contactos.AgregarAsync(new Contacto
        {
            EstudianteId = estudianteId,
            Fecha = fecha.Date,
            Tipo = tipo,
            Resultado = resultado.Trim()
        });

        return (true, "Contacto registrado correctamente.");
    }

    // H11: la fecha del último contacto de cada estudiante. Quien nunca fue contactado no aparece.
    public async Task<Dictionary<int, DateTime>> ObtenerUltimosContactosAsync(IReadOnlyCollection<int> estudianteIds)
    {
        if (estudianteIds.Count == 0)
            return new Dictionary<int, DateTime>();

        var contactos = await _contactos.ObtenerPorEstudiantesAsync(estudianteIds);

        return contactos
            .GroupBy(c => c.EstudianteId)
            .ToDictionary(grupo => grupo.Key, grupo => grupo.Max(c => c.Fecha));
    }

    // H11: ¿el último contacto es reciente? Es una regla del negocio, por eso vive acá y no en la pantalla.
    public static bool EsReciente(DateTime ultimoContacto, DateTime hoy) =>
        (hoy.Date - ultimoContacto.Date).TotalDays <= DiasContactoReciente;
}