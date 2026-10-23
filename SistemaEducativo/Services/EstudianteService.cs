using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Services;

public class EstudianteService
{
    private readonly EstudianteRepositorio _repositorio;
    public EstudianteService(EstudianteRepositorio repositorio) => _repositorio = repositorio;

    public Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId) =>
        _repositorio.ObtenerPorComisionAsync(comisionId);

    // Lo que usa la pantalla: recibe los datos sueltos y arma el estudiante.
    public Task<(bool Exito, string Mensaje)> IncorporarAsync(
        int comisionId, string apellido, string nombre, string legajo, DateTime fechaIncorporacion) =>
        AgregarAsync(new Estudiante
        {
            ComisionId = comisionId,
            Apellido = apellido,
            Nombre = nombre,
            Legajo = legajo,
            FechaIncorporacion = fechaIncorporacion
        });

    public async Task<(bool Exito, string Mensaje)> AgregarAsync(Estudiante estudiante)
    {
        if (string.IsNullOrWhiteSpace(estudiante.Legajo) ||
            string.IsNullOrWhiteSpace(estudiante.Apellido) ||
            string.IsNullOrWhiteSpace(estudiante.Nombre))
            return (false, "Apellido, nombre y legajo son obligatorios.");

        estudiante.Legajo = estudiante.Legajo.Trim();
        estudiante.Apellido = estudiante.Apellido.Trim();
        estudiante.Nombre = estudiante.Nombre.Trim();

        // El legajo identifica al estudiante en todo el instituto: no puede repetirse.
        var existente = await _repositorio.ObtenerPorLegajoAsync(estudiante.Legajo);
        if (existente != null)
            return (false, $"El legajo {estudiante.Legajo} ya pertenece a {existente.Apellido}, {existente.Nombre}.");

        // Sin fecha de incorporación se toma hoy.
        estudiante.FechaIncorporacion = Fechas.EsValida(estudiante.FechaIncorporacion)
            ? Fechas.SoloFecha(estudiante.FechaIncorporacion)
            : DateTime.Today;

        await _repositorio.AgregarAsync(estudiante);
        return (true, "Estudiante incorporado correctamente.");
    }
}
