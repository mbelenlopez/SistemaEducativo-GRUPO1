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

        public async Task<(bool, string Mensaje)> CorregirAsistenciaAsync(int claseId, int comisionId, IEnumerable<int> presentes)
        {
            var idsPresentes = presentes.ToHashSet();

            var asistenciasExistentes = await _asistenciaRepositorio.ObtenerPorClaseAsync(claseId);

            foreach(var asistencia in asistenciasExistentes)
            {
                bool estaPresente = idsPresentes.Contains(asistencia.EstudianteId);
                asistencia.Condicion = estaPresente ? "Presente" : "Ausente";

                await _asistenciaRepositorio.CorregirAsistenciaAsync(asistencia);
            }

            return (true, "Asistencia corregida correctamente.");
        }
    }
}
