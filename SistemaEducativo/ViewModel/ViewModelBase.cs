using CommunityToolkit.Mvvm.ComponentModel;

namespace SistemaEducativo.ViewModel;

// Lo que comparten todos los ViewModel: la sesión (comisión activa) y un mensaje para el usuario.
public abstract partial class ViewModelBase : ObservableObject
{
    protected ViewModelBase(SesionActual sesion)
    {
        Sesion = sesion;
    }

    public SesionActual Sesion { get; }

    protected int ComisionId => Sesion.ComisionActualId;

    [ObservableProperty]
    private string mensaje = string.Empty;

    protected const string SinComision = "Primero elegí una comisión en la página de Inicio.";
}
