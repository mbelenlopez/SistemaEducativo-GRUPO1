using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class TrabajoPage : ContentPage
{
	private readonly TrabajoViewModel _viewModel;
	public TrabajoPage(TrabajoViewModel viewModel)
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