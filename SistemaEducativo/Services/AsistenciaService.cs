using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;
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
        private readonly ClaseRepositorio _claseRepositorio;

        public AsistenciaService(AsistenciaRepositorio asistenciaRepositorio, EstudianteRepositorio estudianteRepositorio, ClaseRepositorio claseRepositorio)
        {
            _asistenciaRepositorio = asistenciaRepositorio;
            _estudianteRepositorio = estudianteRepositorio;
            _claseRepositorio = claseRepositorio;
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

        public async Task<(bool, string Mensaje)> CorregirAsistenciaAsync(int claseId, int comisionId, Dictionary<int, bool> estados)
        {
            var asistenciasExistentes = await ObtenerAsistenciasPorClaseAsync(claseId);

            var estudiantesComision = await _estudianteRepositorio.ObtenerPorComisionAsync(comisionId);

            foreach (var estudiante in estudiantesComision)
            {
                var asistenciaReg = asistenciasExistentes.FirstOrDefault(a => a.EstudianteId == estudiante.Id);

                bool presente = estados.TryGetValue(estudiante.Id, out var p) && p;
                string nuevaCondicion = presente ? "Presente" : "Ausente";

                if (asistenciaReg != null)
                {
                    asistenciaReg.Condicion = nuevaCondicion;
                    await _asistenciaRepositorio.CorregirAsistenciaAsync(asistenciaReg);
                }
                else
                {
                    var nuevaAsistencia = new Asistencia
                    {
                        ClaseId = claseId,
                        EstudianteId = estudiante.Id,
                        Condicion = nuevaCondicion
                    };
                    await _asistenciaRepositorio.AgregarAsync(nuevaAsistencia);
                }

            }

            return (true, "Asistencia corregida y completada correctamente.");
        }
    }
}
