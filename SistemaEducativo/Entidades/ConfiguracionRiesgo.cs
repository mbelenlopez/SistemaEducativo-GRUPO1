namespace SistemaEducativo.Entidades;

// Los umbrales del cálculo de riesgo que configura el tutor (H7).
// DECISIÓN: son GLOBALES (valen para todas las comisiones): hay una sola configuración (Id = 1).
// Ver docs/H7-configuracion-umbrales.md.
public class ConfiguracionRiesgo
{
    public const int IdGlobal = 1;

    public int Id { get; set; } = IdGlobal;

    // % de inasistencias a partir del cual el estudiante pasa a riesgo ALTO.
    public double UmbralInasistencias { get; set; }

    // % de trabajos no entregados a partir del cual el estudiante pasa a riesgo ALTO.
    public double UmbralNoEntregados { get; set; }
}
