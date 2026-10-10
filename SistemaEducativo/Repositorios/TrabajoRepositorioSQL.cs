using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Repositorios
{
    public class TrabajoRepositorioSQL : TrabajoRepositorio
    {
        private readonly Database _db;

        public TrabajoRepositorioSQL(Database db) => _db = db;

        public Task AgrearAsync(Trabajo Trabajo) => _db.InsertAsync(Trabajo);

        public Task<List<Trabajo>> ObtenerPorComisionAsync(int ComisionId) =>
            _db.QueryAsync<Trabajo>(
                "SELECT * FROM Trabajos WHERE ComisionId = ? ORDER BY FechaLimite, Titulo"
                );
    }
}