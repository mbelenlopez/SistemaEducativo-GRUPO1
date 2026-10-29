using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Services;

public class ContactoService
{
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
}