using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios
{
    public interface TrabajoRepositorio
    {
        Task<List<Trabajo>> ObtenerPorComisionAsync(int comisionId);
        Task AgregarAsync(Trabajo trabajo);
    }
}
