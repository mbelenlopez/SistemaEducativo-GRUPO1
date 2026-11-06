
namespace SistemaEducativo.Services
{
    public class CalculadoraRiesgo
    {
        public ResultadoRiesgo Calcular(int clasesTotales, int ausencias, int trabajosTotales, int noEntregados, double umbralInasistencias, double umbralNoEntregados)
        {
            // Sin clases ni trabajos registrados no hay nada que calcular.
            if (clasesTotales == 0 && trabajosTotales == 0)
                return new ResultadoRiesgo(0, 0, NivelRiesgo.SinDatos);

            // Si una de las dos señales no tiene datos (0 clases o 0 trabajos) su porcentaje es 0, nunca NaN.
            double porcentajeInasistencias = Porcentaje(ausencias, clasesTotales);
            double porcentajeNoEntregados = Porcentaje(noEntregados, trabajosTotales);

            // Riesgo alto si ALGUNA señal con datos llega a su umbral; si no, riesgo bajo.
            // Una señal sin datos no puede disparar el riesgo (ni siquiera con un umbral de 0).
            bool riesgoPorAsistencia = clasesTotales > 0 && porcentajeInasistencias >= umbralInasistencias;
            bool riesgoPorEntregas = trabajosTotales > 0 && porcentajeNoEntregados >= umbralNoEntregados;

            string nivel = riesgoPorAsistencia || riesgoPorEntregas ? NivelRiesgo.Alto : NivelRiesgo.Bajo;

            return new ResultadoRiesgo(Math.Round(porcentajeInasistencias, 2), Math.Round(porcentajeNoEntregados, 2), nivel);
        }

        private static double Porcentaje(int parte, int total) => total == 0 ? 0 : parte * 100.0 / total;
    }
}
