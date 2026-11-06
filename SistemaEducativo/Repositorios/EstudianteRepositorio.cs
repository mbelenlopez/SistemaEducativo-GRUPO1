using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public interface EstudianteRepositorio
{
    Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId);
    Task<Estudiante?> ObtenerPorLegajoAsync(string legajo);
    Task<Estudiante?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(Estudiante estudiante);
}