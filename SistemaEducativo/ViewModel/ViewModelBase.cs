using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;

namespace SistemaEducativo.ViewModel;

// Lo que comparten todos los ViewModel: la sesión (comisión activa), un mensaje para el usuario
// y tres ayudas que evitan repetir el mismo código en cada pantalla.
public abstract partial class ViewModelBase : ObservableObject
{
    private const string MensajeSinComision = "Primero elegí una comisión en la página de Inicio.";

    // Se usa cuando el usuario intenta guardar antes de que termine de cargar la lista.
    protected const string MensajeCargando = "Esperá un momento: todavía se están cargando los datos.";

    protected ViewModelBase(SesionActual sesion)
    {
        Sesion = sesion;
    }

    public SesionActual Sesion { get; }

    protected int ComisionId => Sesion.ComisionActualId;

    [ObservableProperty]
    private string mensaje = string.Empty;

    // Si todavía no se eligió una comisión, avisa al usuario y devuelve true (la pantalla no puede trabajar).
    protected bool FaltaElegirComision()
    {
        if (Sesion.HayComision)
            return false;

        Mensaje = MensajeSinComision;
        return true;
    }

    // Reemplaza todo el contenido de una lista. Los elementos nuevos se leen ANTES de vaciarla,
    // así dos cargas que se superponen nunca dejan elementos duplicados.
    protected static void Reemplazar<T>(ObservableCollection<T> destino, IEnumerable<T> nuevos)
    {
        var elementos = nuevos.ToList();

        destino.Clear();
        foreach (var elemento in elementos)
            destino.Add(elemento);
    }

    // Lanza una operación async desde un lugar que no puede esperarla (por ejemplo, el cambio de
    // una propiedad). Si falla, el error se muestra en Mensaje en vez de perderse sin avisar.
    protected async void EjecutarSinEsperar(Func<Task> operacion)
    {
        try
        {
            await operacion();
        }
        catch (Exception ex)
        {
            Mensaje = $"Ocurrió un error inesperado: {ex.Message}";
        }
    }
}
