
namespace SistemaEducativo.Services
{
    public class CalculadoraRiesgo
    {
        public ResultadoRiesgo Calcular(int clasesTotales, int ausencias, int trabajosTotales, int noEntregados, double umbralInasistencias, double umbralNoEntregados)
        {
            // Sin clases registradas no hay porcentaje que calcular.
            if (clasesTotales == 0 && trabajosTotales == 0)
                return new ResultadoRiesgo(0, 0, NivelRiesgo.SinDatos);

            double porcentajeInasistencias = ausencias * 100.0 / clasesTotales;
            double porcentajeNoEntregados = noEntregados * 100.0 / trabajosTotales;

            /*Si el porcentaje de inasistencias es mayor o igual al umbral(50),
            se considera riesgo alto, si no riesgo bajo.Los resultados se guardan en nivel
            */
            var umbral = porcentajeInasistencias >= umbralInasistencias ||
                         porcentajeNoEntregados >= umbralNoEntregados;

            string nivel = umbral ? NivelRiesgo.Alto : NivelRiesgo.Bajo;

            return new ResultadoRiesgo(Math.Round(porcentajeInasistencias, 2), Math.Round(porcentajeNoEntregados, 2), nivel);
        }
    }
}
