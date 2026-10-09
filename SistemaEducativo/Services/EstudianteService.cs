using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services;

public class EstudianteService
{
    private readonly EstudianteRepositorio _repositorio;
    public EstudianteService(EstudianteRepositorio repositorio) => _repositorio = repositorio;

    public Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId) =>
        _repositorio.ObtenerPorComisionAsync(comisionId);

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

        if (string.IsNullOrWhiteSpace(estudiante.FechaIncorporacion))
            estudiante.FechaIncorporacion = DateTime.Today.ToString("yyyy-MM-dd");

        await _repositorio.AgregarAsync(estudiante);
        return (true, "Estudiante incorporado correctamente.");
    }
}