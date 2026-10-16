using SistemaEducativo.Entidades;
using SistemaEducativo.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Repositorios
{
    public class EntregaRepositorioSQL : EntregaRepositorio
    {
        private Database _db;
        public EntregaRepositorioSQL(Database db) {
            _db = db;
        }
        public Task AgregarAsync(Entrega entrega)
        {
            return _db.InsertAsync(entrega);
        }

        public Task<List<Entrega>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            return _db.QueryAsync<Entrega>("SELECT * FROM Entregas WHERE EstudianteId = ? ", estudianteId);
        }

        public Task<List<Entrega>> ObtenerPorTrabajoAsync(int trabajoId)
        {
            return _db.QueryAsync<Entrega>("SELECT * FROM Entregas WHERE TrabajoId = ? ", trabajoId);
        }
    }
}
