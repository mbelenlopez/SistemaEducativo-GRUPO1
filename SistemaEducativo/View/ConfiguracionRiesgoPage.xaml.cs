using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class ConfiguracionRiesgoPage : ContentPage
{
    private readonly ConfiguracionRiesgoViewModel _viewModel;

    public ConfiguracionRiesgoPage(ConfiguracionRiesgoViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
    }

    // Cada vez que se abre, se muestran los umbrales vigentes.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}
