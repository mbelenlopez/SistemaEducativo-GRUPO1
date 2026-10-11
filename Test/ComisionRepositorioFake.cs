using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

// Repositorio falso: cumple la misma interfaz que el real, pero guarda todo en una lista en memoria.
public class ComisionRepositorioFake : ComisionRepositorio
{
    public List<Comision> Datos { get; } = new();

    public Task<List<Comision>> ObtenerTodasAsync() =>
        Task.FromResult(Datos.OrderBy(c => c.Nombre).ToList());
}
