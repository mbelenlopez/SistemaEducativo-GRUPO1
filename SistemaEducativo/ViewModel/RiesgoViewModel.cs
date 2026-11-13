using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;

namespace SistemaEducativo.ViewModel
{
    public partial class RiesgoViewModel : ViewModelBase
    {  
        private readonly RiesgoService _riesgoServicio;

        public RiesgoViewModel(SesionActual sesion, RiesgoService riesgoServicio) : base(sesion)
        {
            _riesgoServicio = riesgoServicio;
        }

        public ObservableCollection<PanelItem> Items { get; } = new();

        public string UmbralesTexto =>
            $"Riesgo ALTO si las inasistencias llegan a {_riesgoServicio.UmbralInasistencias:0}%" +
            $" o los trabajos no entregados al {_riesgoServicio.UmbralNoEntregados:0}%.";

        public string ResumenRiesgoTexto =>
             HayDatos ? $"{EstudiantesEnRiesgo} de {CantidadEstudiantes}" : "Datos insuficientes para generar reporte.";

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ResumenRiesgoTexto))]
        private int _cantidadEstudiantes;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(ResumenRiesgoTexto))]
        private int _estudiantesEnRiesgo;

        [ObservableProperty]
        private bool _hayDatos;

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

            foreach (var item in panel)
                Items.Add(ArmarFila(item));
        }

        [RelayCommand]
        public async Task ObtenerReporteAsync()
        {
            ReporteRiesgo estudiantesEnRiesgo = await _riesgoServicio.ObtenerReporteAsync(ComisionId);


            CantidadEstudiantes = estudiantesEnRiesgo.TotalEstudiantes;
            EstudiantesEnRiesgo = estudiantesEnRiesgo.EstudiantesEnRiesgo;
            HayDatos = estudiantesEnRiesgo.HayDatos;
        }

        // Pasa del dato de negocio al texto que se ve en pantalla.
        private static PanelItem ArmarFila(EstudianteConRiesgo item) => new()
        {
            EstudianteId = item.Estudiante.Id,
            NombreCompleto = item.Estudiante.NombreCompleto,
            InasistenciasTexto = $"Inasistencias: {item.Riesgo.PorcentajeInasistencias:0}%",
            EntregaTexto = $"No entregas: {item.Riesgo.PorcentajeNoEntregados:0}%",
            NivelTexto = TextoDelNivel(item.Riesgo.Nivel)
        };

        private static string TextoDelNivel(string nivel) => nivel switch
        {
            NivelRiesgo.Alto => "🔴 Alto",
            NivelRiesgo.Bajo => "🟢 Bajo",
            _ => "⚪ Sin datos"
        };

    }
}
