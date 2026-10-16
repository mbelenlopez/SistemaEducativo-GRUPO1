using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class RiesgoPage : ContentPage
{
    private readonly RiesgoViewModel _viewModel;

    public RiesgoPage(RiesgoViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = viewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}