using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class AsistenciaPage : ContentPage
{
	private AsistenciaViewModel _viewModel;
    public AsistenciaPage(AsistenciaViewModel viewModel)
	{
		_viewModel = viewModel;
		InitializeComponent();
		BindingContext = _viewModel;
    }

	protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}