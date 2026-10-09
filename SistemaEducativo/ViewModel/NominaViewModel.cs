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
        Estudiantes.Clear();

        if (!Sesion.HayComision)
        {
            Mensaje = SinComision;
            return;
        }

        foreach (var estudiante in await _service.ObtenerPorComisionAsync(ComisionId))
            Estudiantes.Add(estudiante);
    }

    [RelayCommand]
    public async Task AgregarAsync()
    {
        if (!Sesion.HayComision)
        {
            Mensaje = SinComision;
            return;
        }

        var resultado = await _service.AgregarAsync(new Estudiante
        {
            ComisionId = ComisionId,
            Apellido = Apellido,
            Nombre = Nombre,
            Legajo = Legajo,
            FechaIncorporacion = FechaIncorporacion.ToString("yyyy-MM-dd")
        });

        Mensaje = resultado.Mensaje;
        if (!resultado.Exito)
            return;

        Apellido = string.Empty;
        Nombre = string.Empty;
        Legajo = string.Empty;
        await CargarAsync();
    }
}