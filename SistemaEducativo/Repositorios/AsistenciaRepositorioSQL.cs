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

        public async Task<List<Asistencia>> ObtenerPorClaseAsync(int claseId)
        {
            var asistencias = await _db.QueryAsync<AsistenciaRegistro>("SELECT * FROM Asistencias WHERE ClaseId = ?", claseId);
                
            return asistencias.Select(a => a.ToEntidad()).ToList();
        }
            

        public async Task<List<Asistencia>> ObtenerPorEstudianteAsync(int estudianteId)
        {
            var asistencias = await _db.QueryAsync<AsistenciaRegistro>("SELECT * FROM Asistencias WHERE EstudianteId = ?", estudianteId);
            return asistencias.Select(a => a.ToEntidad()).ToList();
        }        
        public async Task AgregarAsync(Asistencia asistencia)
        {
            var asistenciaRegistro = AsistenciaRegistro.Desde(asistencia);
            await _db.InsertAsync(asistenciaRegistro);

            asistencia.Id = asistenciaRegistro.Id;
        }            

        public Task CorregirAsistenciaAsync(Asistencia asistencia)
        {
            return _db.UpdateAsync(AsistenciaRegistro.Desde(asistencia));
        }
    }
}
