namespace SistemaEducativo;

public partial class App : Application
{
    public App()
    {
        InitializeComponent();
    }

    // La ventana arranca con el AppShell, que arma el menú lateral y la navegación entre páginas.
    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(new AppShell());
    }
}
