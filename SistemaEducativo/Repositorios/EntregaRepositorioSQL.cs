using SistemaEducativo.Entidades;


namespace SistemaEducativo.Repositorios
{
    public class EntregaRepositorioSQL : EntregaRepositorio
    {
        private readonly Database _db;

        public EntregaRepositorioSQL(Database db) => _db = db;

        public async Task AgregarAsync(Entrega entrega)
        {
            var registro = EntregaRegistro.Desde(entrega);
            await _db.InsertAsync(registro);
            entrega.Id = registro.Id;
        }

        public async Task<List<Entrega>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            var filas = await _db.QueryAsync<EntregaRegistro>("SELECT * FROM Entregas WHERE EstudianteId = ? ", estudianteId);
            return filas.Select(f => f.ToEntidad()).ToList();
        }

        public async Task<List<Entrega>> ObtenerPorTrabajoAsync(int trabajoId)
        {
            var filas = await _db.QueryAsync<EntregaRegistro>("SELECT * FROM Entregas WHERE TrabajoId = ? ", trabajoId);
            return filas.Select(f => f.ToEntidad()).ToList();
        }
    }
}
