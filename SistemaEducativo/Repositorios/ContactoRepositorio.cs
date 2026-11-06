using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public interface ContactoRepositorio
{
    Task<List<Contacto>> ObtenerPorEstudianteAsync(int estudianteId);
    Task AgregarAsync(Contacto contacto);
}
