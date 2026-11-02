using CommunityToolkit.Mvvm.ComponentModel;

namespace SistemaEducativo.ViewModel;

// Estado compartido por todas las páginas: cuál es la comisión con la que se está trabajando.
// Se registra como Singleton, así todas las páginas ven la misma comisión activa.
public partial class SesionActual : ObservableObject
{
    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(HayComision))]
    private int comisionActualId;

    [ObservableProperty]
    private string comisionActualNombre = "Sin comisión seleccionada";

    public bool HayComision => ComisionActualId != 0;

    public void Seleccionar(int id, string nombre)
    {
        ComisionActualId = id;
        ComisionActualNombre = nombre;
    }
}
