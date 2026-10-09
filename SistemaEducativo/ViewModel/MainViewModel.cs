using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.ViewModel;

// Página de Inicio: elegir la comisión activa.
public partial class MainViewModel : ViewModelBase
{
    private readonly ComisionService _comisionService;

    public MainViewModel(SesionActual sesion, ComisionService comisionService) : base(sesion)
    {
        _comisionService = comisionService;
    }

    public ObservableCollection<Comision> Comisiones { get; } = new();

    [ObservableProperty]
    private Comision? comisionSeleccionada;

    [ObservableProperty]
    private bool mostrarModelo;

    // Se ejecuta SOLA cada vez que cambia ComisionSeleccionada (la genera el toolkit).
    partial void OnComisionSeleccionadaChanged(Comision? value)
    {
        if (value is null)
            return;

        Sesion.Seleccionar(value.Id, value.Nombre);   // toda la app pasa a trabajar con esta comisión
    }

    [RelayCommand]
    public async Task CargarAsync()
    {
        if (Comisiones.Count > 0)
            return;   // ya estaban cargadas

        foreach (var comision in await _comisionService.ObtenerTodasAsync())
            Comisiones.Add(comision);

        ComisionSeleccionada = Comisiones.FirstOrDefault();
    }

    [RelayCommand]
    private void AlternarModelo() => MostrarModelo = !MostrarModelo;
}
