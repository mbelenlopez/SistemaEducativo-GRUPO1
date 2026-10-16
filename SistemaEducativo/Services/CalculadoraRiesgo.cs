using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Services
{
    public class CalculadoraRiesgo
    {
        public ResultadoRiesgo Calcular(int clasesTotales, int ausencias, double umbralInasistencias)
        {
            // Sin clases registradas no hay porcentaje que calcular.
            if (clasesTotales == 0)
                return new ResultadoRiesgo(0, NivelRiesgo.SinDatos);

            double porcentaje = ausencias * 100.0 / clasesTotales;

            /*Si el porcentaje de inasistencias es mayor o igual al umbral(50),
            se considera riesgo alto, si no riesgo bajo.Los resultados se guardan en nivel
            */
            string nivel = porcentaje >= umbralInasistencias ? NivelRiesgo.Alto : NivelRiesgo.Bajo;

            return new ResultadoRiesgo(Math.Round(porcentaje, 2), nivel);
        }
    }
}
