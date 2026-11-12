using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

public class ContactoRepositorioFake : ContactoRepositorio
{
    private int _siguienteId = 1;
    public List<Contacto> Datos { get; } = new();

    public Task<List<Contacto>> ObtenerPorEstudianteAsync(int estudianteId) =>
        Task.FromResult(Datos.Where(c => c.EstudianteId == estudianteId).ToList());

    public Task<List<Contacto>> ObtenerPorEstudiantesAsync(IReadOnlyCollection<int> estudianteIds) =>
        Task.FromResult(Datos.Where(c => estudianteIds.Contains(c.EstudianteId)).ToList());


    public Task AgregarAsync(Contacto contacto)
    {
        contacto.Id = _siguienteId++;
        Datos.Add(contacto);
        return Task.CompletedTask;
    }
}
