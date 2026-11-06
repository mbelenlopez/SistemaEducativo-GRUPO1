using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios
{
    public class TrabajoRepositorioSQL : TrabajoRepositorio
    {
        private readonly Database _db;

        public TrabajoRepositorioSQL(Database db) => _db = db;

        public async Task AgregarAsync(Trabajo trabajo)
        {
            var trabajoRegistro = TrabajoRegistro.Desde(trabajo);

            await _db.InsertAsync(trabajoRegistro);

            trabajo.Id = trabajoRegistro.Id;
        }

        public async Task<List<Trabajo>> ObtenerPorComisionAsync(int comisionId)
        {
            var trabajos = await _db.QueryAsync<TrabajoRegistro>(
                "SELECT * FROM Trabajos WHERE ComisionId = ? ORDER BY FechaLimite, Titulo",
                comisionId);

            return trabajos.Select(t => t.ToEntidad()).ToList();
        }
    }
}