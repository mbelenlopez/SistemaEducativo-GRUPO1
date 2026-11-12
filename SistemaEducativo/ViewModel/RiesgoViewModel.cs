using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;
using System.Globalization;

namespace SistemaEducativo.ViewModel
{
    public partial class RiesgoViewModel : ViewModelBase
    {  
        private readonly RiesgoService _riesgoServicio;
        private readonly ContactoService _contactoServicio;

        public RiesgoViewModel(SesionActual sesion, RiesgoService riesgoServicio, ContactoService contactoServicio) : base(sesion)
        {
            _riesgoServicio = riesgoServicio;
            _contactoServicio = contactoServicio;
        }

        public ObservableCollection<PanelItem> Items { get; } = new();

        public string UmbralesTexto =>
            $"Riesgo ALTO si las inasistencias llegan a {_riesgoServicio.UmbralInasistencias:0}%" +
            $" o los trabajos no entregados al {_riesgoServicio.UmbralNoEntregados:0}%.";

        [RelayCommand]
        public async Task CargarAsync() => RecalcularAsync();

        [RelayCommand]
        public async Task RecalcularAsync()
        {
            // H7: antes de calcular se leen los umbrales vigentes y se actualiza el texto que los muestra.
            await _riesgoServicio.CargarUmbralesAsync();
            OnPropertyChanged(nameof(UmbralesTexto));

            Items.Clear();

            if (FaltaElegirComision())
                return;

            var panel = await _riesgoServicio.ObtenerPanelAsync(ComisionId);   // ya viene calculado y ordenado
            Mensaje = panel.Count == 0 ? "Todavía no hay estudiantes en esta comisión." : string.Empty;

            // H11: la fecha del último contacto de cada estudiante, con una sola consulta.
            var ultimosContactos = await _contactoServicio.ObtenerUltimosContactosAsync(
                panel.Select(item => item.Estudiante.Id).ToList());

            foreach (var item in panel)
                Items.Add(ArmarFila(item, ultimosContactos));
        }

        // Pasa del dato de negocio al texto que se ve en pantalla.
        private static PanelItem ArmarFila(EstudianteConRiesgo item, Dictionary<int, DateTime> ultimosContactos) => new()
        {
            EstudianteId = item.Estudiante.Id,
            NombreCompleto = item.Estudiante.NombreCompleto,
            InasistenciasTexto = $"Inasistencias: {item.Riesgo.PorcentajeInasistencias:0}%",
            EntregaTexto = $"No entregas: {item.Riesgo.PorcentajeNoEntregados:0}%",
            NivelTexto = TextoDelNivel(item.Riesgo.Nivel),
            ContactoTexto = TextoContacto(item.Estudiante.Id, ultimosContactos)
        };

        private static string TextoDelNivel(string nivel) => nivel switch
        {
            NivelRiesgo.Alto => "🔴 Alto",
            NivelRiesgo.Bajo => "🟢 Bajo",
            _ => "⚪ Sin datos"
        };

        // H11: lo que se ve en cada fila sobre el último contacto con el estudiante.
        private static string TextoContacto(int estudianteId, Dictionary<int, DateTime> ultimosContactos)
        {
            if (!ultimosContactos.TryGetValue(estudianteId, out var fecha))
                return "❗ Sin contactar";

            var dias = (DateTime.Today - fecha.Date).Days;
            var cuando = dias switch
            {
                <= 0 => "hoy",
                1 => "hace 1 día",
                _ => $"hace {dias} días"
            };
            var detalle = $"el {fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture)} ({cuando})";

            return ContactoService.EsReciente(fecha, DateTime.Today)
                ? $"✅ Contactado {detalle}"
                : $"🕓 Último contacto {detalle}";
        }
    }
}
