using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// H9: ficha de un estudiante con su historial. Es de SOLO LECTURA (registrar un contacto es H10).
// Se abre desde el panel de riesgo, que le pasa el id del estudiante (ver ApplyQueryAttributes).
public partial class FichaEstudianteViewModel : ViewModelBase, IQueryAttributable
{
    // Nombre del parámetro con el que la navegación le pasa el id del estudiante a esta ficha.
    public const string ParametroEstudianteId = "estudianteId";

    private readonly FichaEstudianteService _fichaService;

    public FichaEstudianteViewModel(SesionActual sesion, FichaEstudianteService fichaService) : base(sesion)
    {
        _fichaService = fichaService;
    }

    public ObservableCollection<AsistenciaFila> Asistencias { get; } = new();
    public ObservableCollection<EntregaFila> Entregas { get; } = new();
    public ObservableCollection<ContactoFila> Contactos { get; } = new();

    [ObservableProperty]
    private int estudianteId;

    // false mientras no hay ficha cargada (o si el estudiante no existe): la pantalla oculta las tarjetas.
    [ObservableProperty]
    private bool hayFicha;

    [ObservableProperty]
    private string nombreCompleto = string.Empty;

    [ObservableProperty]
    private string legajo = string.Empty;

    [ObservableProperty]
    private string incorporacionTexto = string.Empty;

    // ---- Riesgo y los datos que lo justifican (CA3) ----
    [ObservableProperty]
    private string nivelTexto = string.Empty;

    [ObservableProperty]
    private string motivoRiesgoTexto = string.Empty;

    [ObservableProperty]
    private string inasistenciasTexto = string.Empty;

    [ObservableProperty]
    private string noEntregasTexto = string.Empty;

    [ObservableProperty]
    private string umbralesTexto = string.Empty;

    // ---- Avisos de "no hay nada" por sección: una sección vacía no rompe ni oculta las demás (CA5) ----
    [ObservableProperty]
    private bool sinAsistencias;

    [ObservableProperty]
    private bool sinEntregas;

    [ObservableProperty]
    private bool sinContactos;

    // Shell llama a este método al navegar y entrega los parámetros (el id del estudiante).
    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        if (!query.TryGetValue(ParametroEstudianteId, out var valor) || valor is null)
            return;

        if (!int.TryParse(Convert.ToString(valor, CultureInfo.InvariantCulture), NumberStyles.Integer, CultureInfo.InvariantCulture, out var id))
            return;

        EstudianteId = id;
        EjecutarSinEsperar(CargarAsync);
    }

    [RelayCommand]
    public async Task CargarAsync()
    {
        if (EstudianteId == 0)
            return;   // todavía no llegó el id (la navegación lo entrega un instante después)

        try
        {
            var ficha = await _fichaService.ObtenerAsync(EstudianteId);

            if (ficha is null)
            {
                HayFicha = false;
                Mensaje = "No se encontró al estudiante.";
                return;
            }

            Mostrar(ficha);
            Mensaje = string.Empty;
        }
        catch (Exception ex)
        {
            HayFicha = false;
            Mensaje = $"Ocurrió un error inesperado: {ex.Message}";
        }
    }

    private void Mostrar(FichaEstudiante ficha)
    {
        var estudiante = ficha.Estudiante;

        NombreCompleto = estudiante.NombreCompleto;
        Legajo = estudiante.Legajo;
        IncorporacionTexto = Fechas.EsValida(estudiante.FechaIncorporacion)
            ? $"Incorporado el {Formatear(estudiante.FechaIncorporacion)}"
            : string.Empty;

        NivelTexto = ficha.Riesgo.Nivel switch
        {
            NivelRiesgo.Alto => "🔴 Alto",
            NivelRiesgo.Bajo => "🟢 Bajo",
            _ => "⚪ Sin datos"
        };

        InasistenciasTexto = ficha.ClasesRegistradas == 0
            ? "Inasistencias: todavía no hay clases con asistencia registrada."
            : $"Inasistencias: {ficha.Ausencias} de {ficha.ClasesRegistradas} clases ({ficha.PorcentajeInasistencias:0}%)";

        NoEntregasTexto = ficha.TrabajosRegistrados == 0
            ? "Trabajos no entregados: todavía no hay entregas registradas."
            : $"Trabajos no entregados: {ficha.NoEntregados} de {ficha.TrabajosRegistrados} ({ficha.PorcentajeNoEntregados:0}%)";

        UmbralesTexto =
            $"Riesgo ALTO si las inasistencias llegan a {ficha.UmbralInasistencias:0}%" +
            $" o los trabajos no entregados al {ficha.UmbralNoEntregados:0}%.";

        MotivoRiesgoTexto = ArmarMotivo(ficha);

        Reemplazar(Asistencias, ficha.Asistencias.Select(a =>
            new AsistenciaFila(Formatear(a.Fecha), a.Presente ? "🟢 Presente" : "🔴 Ausente")));

        Reemplazar(Entregas, ficha.Entregas.Select(e =>
            new EntregaFila(
                e.Trabajo,
                $"Fecha límite: {Formatear(e.FechaLimite)}",
                e.Condicion == "Entregado" ? "🟢 Entregado" : "🔴 No entregado")));

        Reemplazar(Contactos, ficha.Contactos.Select(c =>
            new ContactoFila(Formatear(c.Fecha), c.Resultado)));

        SinAsistencias = Asistencias.Count == 0;
        SinEntregas = Entregas.Count == 0;
        SinContactos = Contactos.Count == 0;

        HayFicha = true;
    }

    private static string ArmarMotivo(FichaEstudiante ficha)
    {
        switch (ficha.Riesgo.Nivel)
        {
            case NivelRiesgo.Alto:
                var motivos = new List<string>();
                if (ficha.SuperaUmbralInasistencias)
                    motivos.Add($"las inasistencias ({ficha.PorcentajeInasistencias:0}%) alcanzan el umbral de {ficha.UmbralInasistencias:0}%");
                if (ficha.SuperaUmbralNoEntregados)
                    motivos.Add($"los trabajos no entregados ({ficha.PorcentajeNoEntregados:0}%) alcanzan el umbral de {ficha.UmbralNoEntregados:0}%");
                return motivos.Count == 0 ? string.Empty : "Motivo: " + string.Join(" y ", motivos) + ".";

            case NivelRiesgo.Bajo:
                return "Las inasistencias y los trabajos no entregados están por debajo de los umbrales.";

            default:
                return "Todavía no hay asistencias ni entregas registradas para calcular el riesgo.";
        }
    }

    // dd/MM/yyyy fijo, sin depender de la configuración regional de la PC.
    private static string Formatear(DateTime fecha) => fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture);
}
