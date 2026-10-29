namespace SistemaEducativo.Services;

// Los dos umbrales vigentes, en porcentaje (0 a 100): uno por cada señal del cálculo de riesgo.
public record UmbralesRiesgo(double Inasistencias, double NoEntregados);
