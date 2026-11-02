using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios
{
    public class ClaseRepositorioSQL : ClaseRepositorio
    {
        private readonly Database _db;
        public ClaseRepositorioSQL(Database db) => _db = db;

        public async Task AgregarAsync(Clase clase)
        {
            var claseRegistro = ClaseRegistro.Desde(clase);

            await _db.InsertAsync(claseRegistro);

            clase.Id = claseRegistro.Id;
        }

        public async Task<bool> ExisteAsync(int comisionId, DateTime fecha)
        {
            var lista = await _db.QueryAsync<ClaseRegistro>(
                "SELECT * FROM Clases WHERE ComisionId = ? AND Fecha = ?",
                comisionId, fecha);
            return lista.Count > 0;
        }

        public async Task<List<Clase>> ObtenerPorComisionAsync(int comisionId)
        {
           var clases = await _db.QueryAsync<ClaseRegistro>(
                "SELECT * FROM Clases WHERE ComisionId = ? ORDER BY Fecha", comisionId);

            return clases.Select(c => c.ToEntidad()).ToList();
        }
            
    }
}
