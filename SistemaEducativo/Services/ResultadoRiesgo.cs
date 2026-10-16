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
        string Nivel
    );

    // Estudiante con su calculo de riesgo
    public record EstudianteConRiesgo(
        Estudiante Estudiante,
        ResultadoRiesgo Riesgo
    );
}
