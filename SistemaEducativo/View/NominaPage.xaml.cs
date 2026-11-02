using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class NominaPage : ContentPage
{
    private readonly NominaViewModel _viewModel;

    public NominaPage(NominaViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = viewModel;
    }

    // Cada vez que la página se muestra, se recargan los datos de la comisión activa.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}