using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;

    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }

    // Botón "Iniciar Paso 1": navega a la página de Nómina usando la ruta definida en AppShell.xaml.
    private async void OnIrANomina(object? sender, EventArgs e)
    {
        await Shell.Current.GoToAsync("//NominaPage");
    }
}
