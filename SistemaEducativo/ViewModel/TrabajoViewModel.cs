using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// H2: definir los trabajos (con su fecha límite) del cuatrimestre.
// El registro de entregas (H5) se suma después sobre esta misma clase.
public partial class TrabajoViewModel : ViewModelBase
{
    private readonly TrabajoService _trabajoService;

    public TrabajoViewModel(SesionActual sesion, TrabajoService trabajoService) : base(sesion)
    {
        _trabajoService = trabajoService;
    }

    public ObservableCollection<Trabajo> Trabajos { get; } = new();

    [ObservableProperty]
    private string titulo = string.Empty;

    [ObservableProperty]
    private DateTime fechaLimite = DateTime.Today.AddDays(7);

    [RelayCommand]
    public async Task CargarAsync()
    {
        if (FaltaElegirComision())
        {
            Trabajos.Clear();
            return;
        }

        Reemplazar(Trabajos, await _trabajoService.ObtenerPorComisionAsync(ComisionId));
    }

    [RelayCommand]
    public async Task CrearAsync()
    {
        if (FaltaElegirComision())
            return;

        var resultado = await _trabajoService.CrearAsync(ComisionId, Titulo, FechaLimite);
        Mensaje = resultado.Mensaje;

        if (!resultado.Exito)
            return;

        Titulo = string.Empty;
        await CargarAsync();
    }
}
