using SistemaEducativo.View;

namespace SistemaEducativo;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Página que no está en el menú lateral: se abre desde el panel de riesgo (H9).
        Routing.RegisterRoute(nameof(FichaEstudiantePage), typeof(FichaEstudiantePage));
    }
}
