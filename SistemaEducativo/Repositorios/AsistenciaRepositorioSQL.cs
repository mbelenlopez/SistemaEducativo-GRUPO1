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
        {
            // --- CHIVATO 2: Confirmemos que entra acá y con qué ID ---
            System.Diagnostics.Debug.WriteLine($"[REPO] Ejecutando UpdateAsync para Asistencia ID: {asistencia.Id} con valor: {asistencia.Condicion}");
            return _db.UpdateAsync(asistencia);
        }
    }
}
