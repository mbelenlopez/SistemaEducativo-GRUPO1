using System.Collections.ObjectModel;
using System.Globalization;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// H10: registra un contacto del tutor con un estudiante y muestra su historial.
public partial class ContactoViewModel : ViewModelBase
{
    private readonly ContactoService _contactoService;
    private readonly EstudianteService _estudianteService;

    public ContactoViewModel(SesionActual sesion, ContactoService contactoService, EstudianteService estudianteService)
        : base(sesion)
    {
        _contactoService = contactoService;
        _estudianteService = estudianteService;
    }

    public ObservableCollection<Estudiante> Estudiantes { get; } = new();
    public ObservableCollection<ContactoItem> Historial { get; } = new();
    public IReadOnlyList<string> Tipos => TiposContacto.Todos;

    [ObservableProperty] private Estudiante? estudianteSeleccionado;
    [ObservableProperty] private DateTime fecha = DateTime.Today;
    [ObservableProperty] private string? tipoSeleccionado;
    [ObservableProperty] private string resultado = string.Empty;
    [ObservableProperty] private string historialTitulo = string.Empty;

    // Al elegir otro estudiante, se muestra su historial.
    partial void OnEstudianteSeleccionadoChanged(Estudiante? value) => _ = CargarHistorialAsync();

    [RelayCommand]
    public async Task CargarAsync()
    {
        Estudiantes.Clear();
        EstudianteSeleccionado = null;

        if (FaltaElegirComision())
            return;

        Reemplazar(Estudiantes, await _estudianteService.ObtenerPorComisionAsync(ComisionId));

        if (Estudiantes.Count == 0)
            Mensaje = "Todavía no hay estudiantes en la comisión.";
    }

    [RelayCommand]
    public async Task GuardarAsync()
    {
        if (EstudianteSeleccionado is null)
        {
            Mensaje = "Elegí un estudiante.";
            return;
        }

        var respuesta = await _contactoService.RegistrarAsync(
            EstudianteSeleccionado.Id, Fecha, TipoSeleccionado ?? string.Empty, Resultado);

        Mensaje = respuesta.Mensaje;

        if (!respuesta.Exito)
            return;

        Resultado = string.Empty;
        await CargarHistorialAsync();
    }

    private async Task CargarHistorialAsync()
    {
        if (EstudianteSeleccionado is null)
        {
            Historial.Clear();
            HistorialTitulo = string.Empty;
            return;
        }

        var contactos = await _contactoService.ObtenerPorEstudianteAsync(EstudianteSeleccionado.Id);

        Reemplazar(Historial, contactos.Select(ArmarFila));
        HistorialTitulo = $"Contactos de {EstudianteSeleccionado.NombreCompleto} ({contactos.Count})";
    }

    private static ContactoItem ArmarFila(Contacto contacto) =>
        new(contacto.Fecha.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture),
            contacto.Tipo ?? string.Empty,
            contacto.Resultado);
}