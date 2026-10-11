using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// H2: definir las clases (fechas) del cuatrimestre.
public partial class CalendarioViewModel : ViewModelBase
{
    private readonly CalendarioService _calendarioService;

    public CalendarioViewModel(SesionActual sesion, CalendarioService calendarioService) : base(sesion)
    {
        _calendarioService = calendarioService;
    }

    public ObservableCollection<Clase> Clases { get; } = new();

    [ObservableProperty]
    private DateTime fecha = DateTime.Today;

    [RelayCommand]
    public async Task CargarAsync()
    {
        if (FaltaElegirComision())
        {
            Clases.Clear();
            return;
        }

        Reemplazar(Clases, await _calendarioService.ObtenerClasesAsync(ComisionId));
    }

    [RelayCommand]
    public async Task AgregarAsync()
    {
        if (FaltaElegirComision())
            return;

        var resultado = await _calendarioService.AgregarClaseAsync(ComisionId, Fecha);
        Mensaje = resultado.Mensaje;

        if (resultado.Exito)
            await CargarAsync();
    }
}
