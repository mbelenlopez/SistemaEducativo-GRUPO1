using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Services
{
    public class RiesgoService
    {
        
        public double UmbralInasistencias = 50;
        
        private readonly AsistenciaRepositorio _asistenciaRepositorio;
        private readonly EstudianteRepositorio _estudianteRepositorio;
        private readonly CalculadoraRiesgo _calculadoraRiesgo;
    
        public RiesgoService(

            AsistenciaRepositorio asistenciaRepositorio,
            EstudianteRepositorio estudianteRepositorio,
            CalculadoraRiesgo calculadoraRiesgo

        )
        {
            _asistenciaRepositorio = asistenciaRepositorio;
            _estudianteRepositorio = estudianteRepositorio;
            _calculadoraRiesgo = calculadoraRiesgo;
        }
        // Calcula el riesgo de un estudiante basado en su id
        public async Task<ResultadoRiesgo> CalcularAsync(int estudianteId)
        {
            var asistencia = await _asistenciaRepositorio.ObtenerPorEstudianteAsync(estudianteId);
            return _calculadoraRiesgo.Calcular(
                clasesTotales: asistencia.Count,
                ausencias: asistencia.Count(a => a.Condicion == "Ausente"),
                umbralInasistencias: UmbralInasistencias
            );
        }
        // Calcula el riesgo de todos los estudiantes de una comisión
        public async Task<List<EstudianteConRiesgo>> CalcularComisionAsync(int comisionId)
        {
            var estudiantes = await _estudianteRepositorio.ObtenerPorComisionAsync(comisionId);
            var resultado = new List<EstudianteConRiesgo>();

            foreach (var estudiante in estudiantes)
            {
                var riesgo = await CalcularAsync(estudiante.Id);
                resultado.Add(new EstudianteConRiesgo(estudiante, riesgo));
            }

            return resultado;
        }

    }
}
