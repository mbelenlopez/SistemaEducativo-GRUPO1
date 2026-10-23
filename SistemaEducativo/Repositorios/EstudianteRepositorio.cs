using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public interface EstudianteRepositorio
{
    Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId);
    Task<Estudiante?> ObtenerPorLegajoAsync(string legajo);
    Task AgregarAsync(Estudiante estudiante);
}