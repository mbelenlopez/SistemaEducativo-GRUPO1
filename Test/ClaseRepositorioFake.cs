using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

public class ClaseRepositorioFake : ClaseRepositorio
{
    private int _siguienteId = 1;
    public List<Clase> Datos { get; } = new();

    public Task<List<Clase>> ObtenerPorComisionAsync(int comisionId) =>
        Task.FromResult(Datos.Where(c => c.ComisionId == comisionId).OrderBy(c => c.Fecha).ToList());

    public Task<bool> ExisteAsync(int comisionId, DateTime fecha) =>
        Task.FromResult(Datos.Any(c => c.ComisionId == comisionId && c.Fecha == fecha));

    public Task AgregarAsync(Clase clase)
    {
        clase.Id = _siguienteId++;
        Datos.Add(clase);
        return Task.CompletedTask;
    }
}
