using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

public class EstudianteRepositorioFake : EstudianteRepositorio
{
    private int _siguienteId = 1;
    public List<Estudiante> Datos { get; } = new();

    public Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId) =>
        Task.FromResult(Datos
            .Where(e => e.ComisionId == comisionId)
            .OrderBy(e => e.Apellido)
            .ThenBy(e => e.Nombre)
            .ToList());

    public Task<Estudiante?> ObtenerPorLegajoAsync(string legajo) =>
        Task.FromResult(Datos.FirstOrDefault(e => e.Legajo == legajo));

    public Task AgregarAsync(Estudiante estudiante)
    {
        estudiante.Id = _siguienteId++;   // igual que AutoIncrement en SQLite
        Datos.Add(estudiante);
        return Task.CompletedTask;
    }
}
