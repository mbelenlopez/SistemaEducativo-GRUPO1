using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services;

public interface EstudianteRepositorio
{
    Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId);
    Task<Estudiante?> ObtenerPorLegajoAsync(string legajo);
    Task AgregarAsync(Estudiante estudiante);
}