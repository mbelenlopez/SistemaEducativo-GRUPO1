using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// H1: cargar y ver la nómina de la comisión activa.
public partial class NominaViewModel : ViewModelBase
{
    private readonly EstudianteService _service;

    public NominaViewModel(SesionActual sesion, EstudianteService service) : base(sesion)
    {
        _service = service;
    }

    public ObservableCollection<Estudiante> Estudiantes { get; } = new();

    [ObservableProperty]
    private string apellido = string.Empty;

    [ObservableProperty]
    private string nombre = string.Empty;

    [ObservableProperty]
    private string legajo = string.Empty;

    [ObservableProperty]
    private DateTime fechaIncorporacion = DateTime.Today;

    [RelayCommand]
    public async Task CargarAsync()
    {
        if (FaltaElegirComision())
        {
            Estudiantes.Clear();
            return;
        }

        Reemplazar(Estudiantes, await _service.ObtenerPorComisionAsync(ComisionId));
    }

    [RelayCommand]
    public async Task AgregarAsync()
    {
        if (FaltaElegirComision())
            return;

        var resultado = await _service.IncorporarAsync(ComisionId, Apellido, Nombre, Legajo, FechaIncorporacion);
        Mensaje = resultado.Mensaje;

        if (!resultado.Exito)
            return;

        Apellido = string.Empty;
        Nombre = string.Empty;
        Legajo = string.Empty;
        await CargarAsync();
    }
}
