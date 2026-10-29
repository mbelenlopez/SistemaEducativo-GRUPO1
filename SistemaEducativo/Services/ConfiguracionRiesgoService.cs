using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Services;

// H7: los umbrales del cálculo de riesgo se configuran desde la pantalla, sin tocar el código.

//  - Hay un umbral por cada señal (inasistencias y trabajos no entregados), cada uno por separado.
//  - Son globales: valen para todas las comisiones.
//  - Un umbral válido está entre 0 y 100 (ambos incluidos).
//  - Si el tutor nunca configuró nada se usa el valor por defecto: 50%.
public class ConfiguracionRiesgoService
{
    // Valor por defecto documentado (CA5): 50% para las dos señales.
    public const double UmbralPorDefecto = 50;

    public const double Minimo = 0;
    public const double Maximo = 100;

    private readonly ConfiguracionRiesgoRepositorio _repositorio;

    // Copia en memoria de los umbrales vigentes: evita leer la base una vez por cada estudiante.
    private UmbralesRiesgo? _vigentes;

    public ConfiguracionRiesgoService(ConfiguracionRiesgoRepositorio repositorio) => _repositorio = repositorio;

    public static UmbralesRiesgo PorDefecto => new(UmbralPorDefecto, UmbralPorDefecto);

    public static bool EsValido(double umbral) =>
        !double.IsNaN(umbral) && umbral >= Minimo && umbral <= Maximo;

    // Los umbrales que se usan hoy: los configurados o, si no hay (o están dañados), los de por defecto.
    public async Task<UmbralesRiesgo> ObtenerAsync()
    {
        if (_vigentes is not null)
            return _vigentes;

        var guardada = await _repositorio.ObtenerAsync();

        _vigentes = guardada is null
            ? PorDefecto
            : new UmbralesRiesgo(
                EsValido(guardada.UmbralInasistencias) ? guardada.UmbralInasistencias : UmbralPorDefecto,
                EsValido(guardada.UmbralNoEntregados) ? guardada.UmbralNoEntregados : UmbralPorDefecto);

        return _vigentes;
    }

    // Valida los dos valores y, solo si ambos son correctos, los guarda. Si alguno es inválido no se guarda ninguno.
    public async Task<(bool Exito, string Mensaje)> GuardarAsync(double inasistencias, double noEntregados)
    {
        if (!EsValido(inasistencias))
            return (false, MensajeRango("inasistencias"));

        if (!EsValido(noEntregados))
            return (false, MensajeRango("trabajos no entregados"));

        await _repositorio.GuardarAsync(new ConfiguracionRiesgo
        {
            UmbralInasistencias = inasistencias,
            UmbralNoEntregados = noEntregados
        });

        // Desde acá, todos los cálculos usan los valores nuevos.
        _vigentes = new UmbralesRiesgo(inasistencias, noEntregados);
        return (true, "Umbrales guardados.");
    }

    public static string MensajeRango(string senal) =>
        $"El umbral de {senal} debe estar entre {Minimo:0} y {Maximo:0}%.";
}
