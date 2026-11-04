using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Services
{
    //El calculo del riesgo de un estudiante
    public record ResultadoRiesgo(
        double PorcentajeInasistencias,
        double PorcentajeNoEntregados,
        string Nivel
    )
    {
        // Orden del panel: primero los de riesgo ALTO, después los BAJO y al final los SIN DATOS.
        public int Prioridad => Nivel == NivelRiesgo.Alto ? 0 : Nivel == NivelRiesgo.Bajo ? 1 : 2;

        // Qué tan grave es: el peor de los dos indicadores. Desempata dentro del mismo nivel.
        public double Puntaje => Math.Max(PorcentajeInasistencias, PorcentajeNoEntregados);
    }

    // Estudiante con su calculo de riesgo
    public record EstudianteConRiesgo(
        Estudiante Estudiante,
        ResultadoRiesgo Riesgo
    );
}
