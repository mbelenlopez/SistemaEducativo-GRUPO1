using SQLite;

namespace SistemaEducativo.Entidades;

// Los umbrales del cálculo de riesgo que configura el tutor (H7).
// DECISIÓN: son GLOBALES (valen para todas las comisiones), por eso la tabla tiene una sola fila (Id = 1).
[Table("ConfiguracionRiesgo")]
public class ConfiguracionRiesgo
{
	public const int IdGlobal = 1;

	[PrimaryKey]
	public int Id { get; set; } = IdGlobal;

	// % de inasistencias a partir del cual el estudiante pasa a riesgo ALTO.
	public double UmbralInasistencias { get; set; }

	// % de trabajos no entregados a partir del cual el estudiante pasa a riesgo ALTO.
	public double UmbralNoEntregados { get; set; }
}
