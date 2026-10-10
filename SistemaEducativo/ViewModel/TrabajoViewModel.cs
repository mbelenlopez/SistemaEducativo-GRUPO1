using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;

namespace SistemaEducativo.ViewModel
{
    public partial class TrabajoViewModel : ViewModelBase
    {
        private readonly TrabajoService _trabajoService;
        public TrabajoViewModel(TrabajoService trabajoService, SesionActual sesion) : base(sesion)
        {
            _trabajoService = trabajoService;
        }


        public ObservableCollection<Trabajo> Trabajos { get; } = new();

        [ObservableProperty]
        private DateTime fechaLimite = DateTime.Today.AddDays(7);


        [ObservableProperty]
        private Trabajo? trabajoSeleccionado;


        [ObservableProperty]
        private string titulo = string.Empty;

        [RelayCommand]
        public async Task CrearAsync()
        {
            if (!Sesion.HayComision)
            {
                Mensaje = SinComision;
                return;
            }

            var resultado = await _trabajoService.CrearAsync(
                ComisionId,
                Titulo,
                FechaLimite
                );
            Mensaje = resultado.Mensaje;

            if (!resultado.Exito)
                return;
            Titulo = string.Empty;
            await CargarAsync();
        }

        [RelayCommand]
        public async Task CargarAsync()
        {
            if (!Sesion.HayComision)
            {
                Mensaje = SinComision;
                return;
            }
            var trabajos = await _trabajoService.ObtenerPorComisionAsync(ComisionId);

            Trabajos.Clear();
            foreach (var trabajo in trabajos) 
                Trabajos.Add(trabajo);
        }

    }
}
