using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services
{
    public interface TrabajoRepositorio
    {
        Task<List<Trabajo>> ObtenerPorComisionAsync(int ComisionId);
        Task AgrearAsync(Trabajo Trabajo);
    }
}
