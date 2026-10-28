using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class FichaEstudiantePage : ContentPage
{
	private readonly FichaEstudianteViewModel _viewModel;

	public FichaEstudiantePage(FichaEstudianteViewModel viewModel)
	{
		_viewModel = viewModel;
		InitializeComponent();
		BindingContext = viewModel;
	}

	// Si el id todavía no llegó, CargarAsync no hace nada; apenas llega, ApplyQueryAttributes vuelve a cargar.
	protected override async void OnAppearing()
	{
		base.OnAppearing();
		await _viewModel.CargarAsync();
	}

	// Vuelve a la página anterior (el panel de riesgo).
	private async void OnVolver(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("..");
	}
}
