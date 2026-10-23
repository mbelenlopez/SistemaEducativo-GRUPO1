using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;

namespace SistemaEducativo.ViewModel
{
    public partial class AsistenciaViewModel : ViewModelBase
    {
        private readonly AsistenciaService _asistenciaService;
        private readonly CalendarioService _calendarioService;
        private readonly EstudianteService _estudianteService;

        public AsistenciaViewModel(SesionActual sesion, AsistenciaService asistenciaService, CalendarioService calendarioService, EstudianteService estudianteService)
            : base(sesion)
        {
            _asistenciaService = asistenciaService;
            _calendarioService = calendarioService;
            _estudianteService = estudianteService;
        }


        public ObservableCollection<Clase> Clases{ get; } = new();
        public ObservableCollection<AsistenciaItem> Items { get; } = new();

        [ObservableProperty]
        public Clase? claseSeleccionada;

        partial void OnClaseSeleccionadaChanged(Clase? clase) => _ = CargarItemsAsync();

        [RelayCommand]
        public async Task CargarAsync()
        {
            var claseAnteriorId = ClaseSeleccionada?.Id;
            Clases.Clear();
            Items.Clear();

            if (FaltaElegirComision())
            {
                return;
            }

            foreach(var clase in await _calendarioService.ObtenerClasesAsync(ComisionId))
            {
                Clases.Add(clase);
            }

            if(Clases.Count() == 0)
            {
                ClaseSeleccionada = null;
                Mensaje = "Primero debes crear una clase en el calendario.";
                return;
            }

            ClaseSeleccionada = Clases.FirstOrDefault(c => c.Id == claseAnteriorId) ?? Clases.First();
        }

        private async Task CargarItemsAsync()
        {
            Items.Clear();
            if(ClaseSeleccionada is null)
            {
                return;
            }

            var registros = await _asistenciaService.ObtenerAsistenciasPorClaseAsync(ClaseSeleccionada.Id);
            var estudiantes = await _estudianteService.ObtenerPorComisionAsync(ComisionId);

            foreach(var estudiante in estudiantes)
            {
                var registro = registros.FirstOrDefault(r => r.EstudianteId == estudiante.Id);
                Items.Add(new AsistenciaItem(estudiante, registro?.Condicion == "Presente"));
            }

            Mensaje = registros.Count() > 0
                ? "Esta clase ya tiene la asistencia registrada"
                : "Marcá a quién estuvo presente. Los que no marques quedarán como ausentes.";
        }


        [RelayCommand]
        public async Task GuardarAsync()
        {
            if(ClaseSeleccionada is null)
            {
                Mensaje = "Seleccioná una clase.";
                return;
            }

            var claseId = ClaseSeleccionada.Id;
            
            var presentes = Items.Where(x => x.Presente).Select(x => x.Estudiante.Id).ToList();
            var registrosActuales = await _asistenciaService.ObtenerAsistenciasPorClaseAsync(claseId);

            (bool Exito, string Mensaje) resultado;

            if(registrosActuales.Count > 0)
            {
                resultado = await _asistenciaService.CorregirAsistenciaAsync(claseId, ComisionId, presentes);
            }
            else
            {
                resultado = await _asistenciaService.RegistrarAsistenciaAsync(claseId, ComisionId, presentes);
            }

            if (resultado.Exito)
                await CargarItemsAsync();
                
            Mensaje = resultado.Mensaje;
         }

    }
}
