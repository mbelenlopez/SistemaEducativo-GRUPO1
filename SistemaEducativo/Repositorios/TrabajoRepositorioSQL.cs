using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios
{
    public class TrabajoRepositorioSQL : TrabajoRepositorio
    {
        private readonly Database _db;

        public TrabajoRepositorioSQL(Database db) => _db = db;

        public async Task AgregarAsync(Trabajo trabajo)
        {
            var registro = TrabajoRegistro.Desde(trabajo);
            await _db.InsertAsync(registro);
            trabajo.Id = registro.Id;
        }

        public async Task<List<Trabajo>> ObtenerPorComisionAsync(int comisionId)
        {
            var filas = await _db.QueryAsync<TrabajoRegistro>(
                "SELECT * FROM Trabajos WHERE ComisionId = ? ORDER BY FechaLimite, Titulo",
                comisionId);
            return filas.Select(f => f.ToEntidad()).ToList();
        }
    }
}