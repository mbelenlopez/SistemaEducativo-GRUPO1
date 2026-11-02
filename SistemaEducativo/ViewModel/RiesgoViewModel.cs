using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;

namespace SistemaEducativo.ViewModel
{
    public partial class RiesgoViewModel : ViewModelBase
    {
        private readonly EstudianteService _estudianteServicio;
        private readonly RiesgoService _riesgoServicio;
        public RiesgoViewModel(SesionActual sesion, EstudianteService estudianteServicio, RiesgoService riesgoServicio) : base(sesion)
        {
            _estudianteServicio = estudianteServicio;
            _riesgoServicio = riesgoServicio;
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
            Items.Clear();

            if (FaltaElegirComision())
            {
                return;
            }

            var estudiantes = await _estudianteServicio.ObtenerPorComisionAsync(ComisionId);
            Mensaje = estudiantes.Count == 0 ? "Todavía no hay estudiantes en esta comisión." : string.Empty;

            var filas = new List<PanelItem>();

            foreach(var estudiante in estudiantes)
            {
                var riesgo = await _riesgoServicio.CalcularAsync(estudiante.Id);

                filas.Add(new PanelItem
                {
                    EstudianteId = estudiante.Id,
                    NombreCompleto = estudiante.NombreCompleto,
                    InasistenciasTexto = $"Inasistencias: {riesgo.PorcentajeInasistencias:0}%",
                    EntregaTexto = $"No entregas: {riesgo.PorcentajeNoEntregados:0}%",
                    Nivel = riesgo.Nivel,
                    NivelTexto = riesgo.Nivel switch
                    {
                        "Alto" => "🔴 Alto",
                        "Bajo" => "🟢 Bajo",
                        _ => "⚪ Sin datos"
                    },
                    Puntaje = Math.Max(riesgo.PorcentajeInasistencias, riesgo.PorcentajeNoEntregados)
                });
            }
            var filasOrdenadas = filas.OrderBy(f => f.Prioridad).ThenByDescending(f => f.Puntaje).ThenBy(f => f.NombreCompleto);

            foreach(var fila in filasOrdenadas)
            {
                Items.Add(fila);
            }
        }

    }
}
