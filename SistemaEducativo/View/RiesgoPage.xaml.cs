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

    // CA6 de H9: un clic sobre un estudiante del panel abre su ficha.
    private async void OnEstudianteSeleccionado(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not PanelItem fila)
            return;

        // Se limpia la selección para poder volver a abrir al mismo estudiante al regresar al panel.
        if (sender is CollectionView lista)
            lista.SelectedItem = null;

        await Shell.Current.GoToAsync(
            nameof(FichaEstudiantePage),
            new Dictionary<string, object> { [FichaEstudianteViewModel.ParametroEstudianteId] = fila.EstudianteId });
    }
}