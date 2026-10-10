using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class CalendarioPage : ContentPage
{
	private readonly CalendarioViewModel _calendarioViewModel;


	public CalendarioPage(CalendarioViewModel viewModel)
	{
		InitializeComponent();
		_calendarioViewModel = viewModel;
		BindingContext = _calendarioViewModel;
	}

	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _calendarioViewModel.CargarAsync();
	}
}