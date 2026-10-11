using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

public class TrabajoRepositorioFake : TrabajoRepositorio
{
    private int _siguienteId = 1;
    public List<Trabajo> Datos { get; } = new();

    public Task<List<Trabajo>> ObtenerPorComisionAsync(int comisionId) =>
        Task.FromResult(Datos
            .Where(t => t.ComisionId == comisionId)
            .OrderBy(t => t.FechaLimite)
            .ThenBy(t => t.Titulo)
            .ToList());

    public Task AgregarAsync(Trabajo trabajo)
    {
        trabajo.Id = _siguienteId++;
        Datos.Add(trabajo);
        return Task.CompletedTask;
    }
}
