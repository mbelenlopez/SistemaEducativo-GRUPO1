using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

// Cómo se guarda la configuración de riesgo en SQLite: una sola fila.
[Table("ConfiguracionRiesgo")]
public class ConfiguracionRiesgoRegistro
{
    [PrimaryKey]
    public int Id { get; set; }

    public double UmbralInasistencias { get; set; }

    public double UmbralNoEntregados { get; set; }

    public ConfiguracionRiesgo ToEntidad() => new()
    {
        Id = Id,
        UmbralInasistencias = UmbralInasistencias,
        UmbralNoEntregados = UmbralNoEntregados
    };

    public static ConfiguracionRiesgoRegistro Desde(ConfiguracionRiesgo c) => new()
    {
        Id = ConfiguracionRiesgo.IdGlobal,
        UmbralInasistencias = c.UmbralInasistencias,
        UmbralNoEntregados = c.UmbralNoEntregados
    };
}
