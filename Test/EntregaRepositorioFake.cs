using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

public class EntregaRepositorioFake : EntregaRepositorio
{
    private int _siguienteId = 1;
    public List<Entrega> Datos { get; } = new();

    public Task<List<Entrega>> ObtenerPorTrabajoAsync(int trabajoId) =>
        Task.FromResult(Datos.Where(e => e.TrabajoId == trabajoId).ToList());

    public Task<List<Entrega>> ObtenerPorEstudianteAsync(int estudianteId) =>
        Task.FromResult(Datos.Where(e => e.EstudianteId == estudianteId).ToList());

    public Task AgregarAsync(Entrega entrega)
    {
        entrega.Id = _siguienteId++;
        Datos.Add(entrega);
        return Task.CompletedTask;
    }
}
