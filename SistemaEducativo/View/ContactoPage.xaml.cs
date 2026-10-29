using SistemaEducativo.ViewModel;

namespace SistemaEducativo.View;

public partial class ContactoPage : ContentPage
{
    private readonly ContactoViewModel _viewModel;

    public ContactoPage(ContactoViewModel viewModel)
    {
        _viewModel = viewModel;
        InitializeComponent();
        BindingContext = _viewModel;
    }

    // Cada vez que se muestra la página, se recargan los estudiantes de la comisión activa.
    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _viewModel.CargarAsync();
    }
}