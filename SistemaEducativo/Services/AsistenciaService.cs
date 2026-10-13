using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Services
{
    public class AsistenciaService
    {
        private readonly AsistenciaRepositorio _asistenciaRepositorio;
        private readonly EstudianteRepositorio _estudianteRepositorio;

        public AsistenciaService(AsistenciaRepositorio asistenciaRepositorio, EstudianteRepositorio estudianteRepositorio)
        {
            _asistenciaRepositorio = asistenciaRepositorio;
            _estudianteRepositorio = estudianteRepositorio;
        }

        public Task<List<Asistencia>> ObtenerAsistenciasPorClaseAsync(int claseId) =>
            _asistenciaRepositorio.ObtenerPorClaseAsync(claseId);

        public async Task<(bool Exito, string Mensaje)> RegistrarAsistenciaAsync(int claseId, int comisionId, IEnumerable<int> presentes)
        {
            if ((await _asistenciaRepositorio.ObtenerPorClaseAsync(claseId)).Count > 0)
            {
                return (false, "La asistencia para esta clase ya ha sido registrada.");
            }

            var ids = presentes.ToHashSet();

            var estudiantes = await _estudianteRepositorio.ObtenerPorComisionAsync(comisionId);

            if(estudiantes.Count == 0)
            {
                return (false, "No hay estudiantes registrados en esta comisión.");
            };

            foreach(var estudiante in estudiantes)
            {
                var asistencia = new Asistencia
                {
                    ClaseId = claseId,
                    EstudianteId = estudiante.Id,
                    Condicion = ids.Contains(estudiante.Id) ? "Presente" : "Ausente"
                };
                await _asistenciaRepositorio.AgregarAsync(asistencia);
            }

            return (true, "Asistencia registrada correctamente.");
        }
    }
}
