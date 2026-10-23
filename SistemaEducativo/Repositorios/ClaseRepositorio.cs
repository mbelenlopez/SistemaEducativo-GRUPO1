using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios
{
    public interface ClaseRepositorio
    {
        Task<List<Clase>> ObtenerPorComisionAsync(int comisionId);
        Task<bool> ExisteAsync(int comisionId, DateTime fecha);
        Task AgregarAsync(Clase clase);
    }
}