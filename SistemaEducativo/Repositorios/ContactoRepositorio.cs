using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public interface ContactoRepositorio
{
    Task<List<Contacto>> ObtenerPorEstudianteAsync(int estudianteId);
    
    // Los contactos de varios estudiantes con una sola consulta (para el panel de riesgo, H11).
    Task<List<Contacto>> ObtenerPorEstudiantesAsync(IReadOnlyCollection<int> estudianteIds);
    Task AgregarAsync(Contacto contacto);
}
