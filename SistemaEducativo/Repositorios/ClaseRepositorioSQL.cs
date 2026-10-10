using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Repositorios
{
    public class ClaseRepositorioSQL : ClaseRepositorio
    {
        private readonly Database _db;
        public ClaseRepositorioSQL(Database db) => _db = db;

        public Task AgregarAsync(Clase clase) => _db.InsertAsync(clase);

        public async Task<bool> ExisteAsync(int comisionId, DateTime fecha)
        {
            var lista = await _db.QueryAsync<Clase>(
                "SELECT * FROM Clases WHERE ComisionId = ? AND Fecha = ?",
                comisionId, fecha);
            return lista.Count > 0;
        }

        public Task<List<Clase>> ObtenerPorComisionAsync(int comisionId)
            => _db.QueryAsync<Clase>(
                "SELECT * FROM Clases WHERE ComisionId = ? ORDER BY Fecha", comisionId);
    }
}
