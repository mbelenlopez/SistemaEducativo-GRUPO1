using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

public class AsistenciaRepositorioFake : AsistenciaRepositorio
{
	private int _siguienteId = 1;
	public List<Asistencia> Datos { get; } = new();

	public Task<List<Asistencia>> ObtenerPorClaseAsync(int claseId) =>
		Task.FromResult(Datos.Where(a => a.ClaseId == claseId).ToList());

	public Task<List<Asistencia>> ObtenerPorEstudianteAsync(int estudianteId) =>
		Task.FromResult(Datos.Where(a => a.EstudianteId == estudianteId).ToList());

	public Task AgregarAsync(Asistencia asistencia)
	{
		asistencia.Id = _siguienteId++;
		Datos.Add(asistencia);
		return Task.CompletedTask;
	}

	// Los objetos de la lista ya son los mismos que se modificaron: no hay nada más que guardar.
	public Task CorregirAsistenciaAsync(Asistencia asistencia) => Task.CompletedTask;
}
