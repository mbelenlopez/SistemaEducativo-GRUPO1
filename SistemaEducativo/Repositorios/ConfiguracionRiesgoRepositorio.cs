using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

// Puerto: lee y guarda la configuración del cálculo de riesgo.
public interface ConfiguracionRiesgoRepositorio
{
    // Devuelve null si el tutor nunca configuró nada.
    Task<ConfiguracionRiesgo?> ObtenerAsync();

    // Crea la configuración si no existía o la actualiza si ya existía (siempre hay una sola).
    Task GuardarAsync(ConfiguracionRiesgo configuracion);
}
