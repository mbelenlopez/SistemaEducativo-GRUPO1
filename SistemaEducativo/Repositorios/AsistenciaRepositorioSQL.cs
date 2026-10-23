using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Repositorios
{
    public class AsistenciaRepositorioSQL : AsistenciaRepositorio
    {
        private readonly Database _db;

        public AsistenciaRepositorioSQL(Database db) => _db = db;

        public Task<List<Asistencia>> ObtenerPorClaseAsync(int claseId)
            => _db.QueryAsync<Asistencia>("SELECT * FROM Asistencias WHERE ClaseId = ?", claseId);

        public Task<List<Asistencia>> ObtenerPorEstudianteAsync(int estudianteId)
            => _db.QueryAsync<Asistencia>("SELECT * FROM Asistencias WHERE EstudianteId = ?", estudianteId);
        
        public Task AgregarAsync(Asistencia asistencia)
            => _db.InsertAsync(asistencia);
        
        public Task CorregirAsistenciaAsync(Asistencia asistencia)
            => _db.UpdateAsync(asistencia);
    }
}
