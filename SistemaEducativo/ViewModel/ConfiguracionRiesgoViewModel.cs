using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// H7: pantalla para configurar los umbrales del cálculo de riesgo sin tocar el código.
public partial class ConfiguracionRiesgoViewModel : ViewModelBase
{
    private readonly ConfiguracionRiesgoService _configuracion;
    private readonly RecalculoRiesgoService _recalculo;

    public ConfiguracionRiesgoViewModel(
        SesionActual sesion,
        ConfiguracionRiesgoService configuracion,
        RecalculoRiesgoService recalculo) : base(sesion)
    {
        _configuracion = configuracion;
        _recalculo = recalculo;
    }

    // Los campos son texto (y no número) para poder avisar si el tutor escribe algo que no es un número.
    [ObservableProperty]
    private string umbralInasistenciasTexto = string.Empty;

    [ObservableProperty]
    private string umbralNoEntregadosTexto = string.Empty;

    // Resultado del recálculo que se hace al guardar (CA2).
    [ObservableProperty]
    private string resumenTexto = string.Empty;

    public string DefectoTexto =>
        $"Valor por defecto: {ConfiguracionRiesgoService.UmbralPorDefecto:0}% para cada indicador (se usa mientras no configures otro).";

    [RelayCommand]
    public async Task CargarAsync()
    {
        Mostrar(await _configuracion.ObtenerAsync());
        Mensaje = string.Empty;
        ResumenTexto = string.Empty;
    }

    [RelayCommand]
    public async Task GuardarAsync()
    {
        ResumenTexto = string.Empty;

        if (!TryLeerPorcentaje(UmbralInasistenciasTexto, out var inasistencias))
        {
            Mensaje = MensajeNoNumerico("inasistencias");
            return;
        }

        if (!TryLeerPorcentaje(UmbralNoEntregadosTexto, out var noEntregados))
        {
            Mensaje = MensajeNoNumerico("trabajos no entregados");
            return;
        }

        // Acá se rechazan los valores negativos o mayores a 100 (CA3); si alguno es inválido no se guarda ninguno.
        var resultado = await _configuracion.GuardarAsync(inasistencias, noEntregados);
        Mensaje = resultado.Mensaje;

        if (!resultado.Exito)
            return;

        Mostrar(await _configuracion.ObtenerAsync());

        // CA2: el cambio se aplica a todos los estudiantes y se muestra cómo quedaron.
        var resumen = await _recalculo.RecalcularTodosAsync();
        ResumenTexto = ArmarResumen(resumen);
    }

    private void Mostrar(UmbralesRiesgo umbrales)
    {
        UmbralInasistenciasTexto = Formatear(umbrales.Inasistencias);
        UmbralNoEntregadosTexto = Formatear(umbrales.NoEntregados);
    }

    // Acepta "40", "40.5", "40,5" (según la configuración regional) y "40%".
    public static bool TryLeerPorcentaje(string? texto, out double valor)
    {
        valor = 0;

        if (string.IsNullOrWhiteSpace(texto))
            return false;

        var limpio = texto.Replace("%", string.Empty).Trim();

        return double.TryParse(limpio, NumberStyles.Float, CultureInfo.CurrentCulture, out valor)
            || double.TryParse(limpio, NumberStyles.Float, CultureInfo.InvariantCulture, out valor);
    }

    private static string MensajeNoNumerico(string senal) =>
        $"El umbral de {senal} debe ser un número entre {ConfiguracionRiesgoService.Minimo:0} y {ConfiguracionRiesgoService.Maximo:0}%.";

    private static string ArmarResumen(ResumenRecalculo resumen)
    {
        var palabra = resumen.Estudiantes == 1 ? "estudiante" : "estudiantes";

        return $"Riesgo recalculado para {resumen.Estudiantes} {palabra}: " +
               $"{resumen.EnRiesgoAlto} en riesgo alto, {resumen.EnRiesgoBajo} en riesgo bajo, {resumen.SinDatos} sin datos.";
    }

    private static string Formatear(double valor) => valor.ToString("0.##", CultureInfo.CurrentCulture);
}
