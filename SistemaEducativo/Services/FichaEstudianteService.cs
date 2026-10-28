using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Services;

// H9: arma la ficha de un estudiante juntando su asistencia, sus entregas, su riesgo y sus contactos.
// Solo LEE: no modifica ningún dato.
public class FichaEstudianteService
{
    private readonly EstudianteRepositorio _estudiantes;
    private readonly AsistenciaRepositorio _asistencias;
    private readonly EntregaRepositorio _entregas;
    private readonly ClaseRepositorio _clases;
    private readonly TrabajoRepositorio _trabajos;
    private readonly ContactoRepositorio _contactos;
    private readonly RiesgoService _riesgo;

    public FichaEstudianteService(
        EstudianteRepositorio estudiantes,
        AsistenciaRepositorio asistencias,
        EntregaRepositorio entregas,
        ClaseRepositorio clases,
        TrabajoRepositorio trabajos,
        ContactoRepositorio contactos,
        RiesgoService riesgo)
    {
        _estudiantes = estudiantes;
        _asistencias = asistencias;
        _entregas = entregas;
        _clases = clases;
        _trabajos = trabajos;
        _contactos = contactos;
        _riesgo = riesgo;
    }

    // Devuelve null si el estudiante no existe.
    public async Task<FichaEstudiante?> ObtenerAsync(int estudianteId)
    {
        var estudiante = await _estudiantes.ObtenerPorIdAsync(estudianteId);
        if (estudiante is null)
            return null;

        var asistencias = await _asistencias.ObtenerPorEstudianteAsync(estudianteId);
        var entregas = await _entregas.ObtenerPorEstudianteAsync(estudianteId);
        var contactos = await _contactos.ObtenerPorEstudianteAsync(estudianteId);

        // Las asistencias guardan solo el id de la clase y las entregas el id del trabajo:
        // se buscan en las de la comisión del estudiante para mostrar la fecha y el título.
        var clasePorId = (await _clases.ObtenerPorComisionAsync(estudiante.ComisionId)).ToDictionary(c => c.Id);
        var trabajoPorId = (await _trabajos.ObtenerPorComisionAsync(estudiante.ComisionId)).ToDictionary(t => t.Id);

        // Mismo cálculo que el panel de riesgo (H6/H8): así la ficha y el panel nunca se contradicen.
        var riesgo = await _riesgo.CalcularAsync(estudianteId);

        // Historial: lo más reciente primero.
        var detalleAsistencias = asistencias
            .Where(a => clasePorId.ContainsKey(a.ClaseId))
            .Select(a => new DetalleAsistencia(clasePorId[a.ClaseId].Fecha, a.Presente))
            .OrderByDescending(d => d.Fecha)
            .ToList();

        var detalleEntregas = entregas
            .Where(e => trabajoPorId.ContainsKey(e.TrabajoId))
            .Select(e => new
            {
                Trabajo = trabajoPorId[e.TrabajoId],
                e.Condicion
            })
            .OrderByDescending(x => x.Trabajo.FechaLimite)
            .ThenBy(x => x.Trabajo.Titulo)
            .Select(x => new DetalleEntrega(x.Trabajo.Titulo, x.Trabajo.FechaLimite, x.Condicion))
            .ToList();

        var contactosOrdenados = contactos
            .OrderByDescending(c => c.Fecha)
            .ThenByDescending(c => c.Id)
            .ToList();

        return new FichaEstudiante(
            Estudiante: estudiante,
            Riesgo: riesgo,
            ClasesRegistradas: asistencias.Count,
            Ausencias: asistencias.Count(a => a.Condicion == "Ausente"),
            TrabajosRegistrados: entregas.Count,
            NoEntregados: entregas.Count(e => e.Condicion == "No entregado"),
            UmbralInasistencias: _riesgo.UmbralInasistencias,
            UmbralNoEntregados: _riesgo.UmbralNoEntregados,
            Asistencias: detalleAsistencias,
            Entregas: detalleEntregas,
            Contactos: contactosOrdenados);
    }
}
