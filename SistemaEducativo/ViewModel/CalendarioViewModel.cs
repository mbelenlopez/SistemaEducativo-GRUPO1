
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;

namespace SistemaEducativo.ViewModel
{
    public partial class CalendarioViewModel : ViewModelBase
    {
        private readonly CalendarioService _calendarioService;
        public CalendarioViewModel(SesionActual sesion, CalendarioService service) : base(sesion)
        {
            _calendarioService = service;
        }

        public ObservableCollection<Clase> Clases { get; } = new();

        [ObservableProperty]
        private DateTime fecha = DateTime.Today;

        [RelayCommand]
        public async Task CargarAsync()
        {

            if (!Sesion.HayComision)
            {
                Mensaje = SinComision;
                return;
            }

            Clases.Clear();

            foreach (var clase in await _calendarioService.ObtenerClasesAsync(ComisionId))
                Clases.Add(clase);

        }

        [RelayCommand]
        public async Task AgregarAsync()
        {
            if (!Sesion.HayComision)
            {
                Mensaje = SinComision;
                return;
            }

            var resultado = await _calendarioService.AgregarClaseAsync(
                ComisionId,
                Fecha);
            Mensaje = resultado.Mensaje;

            if (resultado.Exito)
                await CargarAsync();
        }
    }
}