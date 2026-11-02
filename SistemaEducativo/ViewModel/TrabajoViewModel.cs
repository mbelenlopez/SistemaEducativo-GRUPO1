using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;
using System.Collections.ObjectModel;

namespace SistemaEducativo.ViewModel
{
    public partial class TrabajoViewModel : ViewModelBase
    {
        private readonly TrabajoService _trabajoService;
        public TrabajoViewModel(TrabajoService trabajoService, SesionActual sesion) : base(sesion)
        {
            _trabajoService = trabajoService;
        }


        public ObservableCollection<Trabajo> Trabajos { get; } = new();

        [ObservableProperty]
        private DateTime fechaLimite = DateTime.Today.AddDays(7);


        [ObservableProperty]
        private Trabajo? trabajoSeleccionado;


        [ObservableProperty]
        private string titulo = string.Empty;

        [RelayCommand]
        public async Task CrearAsync()
        {
            if (!Sesion.HayComision)
            {
                Mensaje = SinComision;
                return;
            }

            var resultado = await _trabajoService.CrearAsync(
                ComisionId,
                Titulo,
                FechaLimite
                );
            Mensaje = resultado.Mensaje;

            if (!resultado.Exito)
                return;
            Titulo = string.Empty;
            await CargarAsync();
        }

        [RelayCommand]
        public async Task CargarAsync()
        {
            if (!Sesion.HayComision)
            {
                Mensaje = SinComision;
                return;
            }
            var trabajos = await _trabajoService.ObtenerPorComisionAsync(ComisionId);

            Trabajos.Clear();
            foreach (var trabajo in trabajos) 
                Trabajos.Add(trabajo);
        }

<<<<<<< Updated upstream
=======
        Reemplazar(Trabajos, await _trabajoService.ObtenerPorComisionAsync(ComisionId));
        TrabajoSeleccionado = Trabajos.FirstOrDefault(t => t.Id == trabajoAnteriorId);
    }

    [RelayCommand]
    public async Task CrearAsync()
    {
        if (FaltaElegirComision())
            return;

        var resultado = await _trabajoService.CrearAsync(ComisionId, Titulo, FechaLimite);
        Mensaje = resultado.Mensaje;

        if (!resultado.Exito)
            return;

        Titulo = string.Empty;
        await CargarAsync();
    }

    private async Task CargarItemsAsync()
    {
        Items.Clear();

        if (TrabajoSeleccionado is null)
            return;

        var registros = await _entregaService.ObtenerPorTrabajoAsync(TrabajoSeleccionado.Id);
        var estudiantes = await _estudianteService.ObtenerPorComisionAsync(ComisionId);

        foreach (var estudiante in estudiantes)
        {
            var registro = registros.FirstOrDefault(x => x.EstudianteId == estudiante.Id);
            // Si el estudiante se incorporó después de registrar el trabajo, no tiene dato: "Sin datos".
            Items.Add(new EntregaItem(estudiante, registro?.Condicion == CondicionEntrega.Entregado, registro?.Condicion ?? "Sin datos"));
        }

        Mensaje = registros.Count > 0
            ? "Las entregas ya están registradas; podés visualizarlas."
            : "Marcá quién entregó el trabajo. Los demás quedarán como no entregado.";
    }

    [RelayCommand]
    public async Task GuardarAsync()
    {
        if (TrabajoSeleccionado is null)
        {
            Mensaje = "Seleccioná un trabajo.";
            return;
        }

        var trabajoId = TrabajoSeleccionado.Id;
        var existentes = await _entregaService.ObtenerPorTrabajoAsync(trabajoId);
        if (existentes.Count > 0)
        {
            Mensaje = "Las entregas ya estaban registradas. Esta vista es de consulta.";
            return;
        }

        var resultado = await _entregaService.RegistrarEntregasAsync(
            trabajoId, ComisionId, Items.Where(x => x.Entregado).Select(x => x.Estudiante.Id).ToList());

        if (resultado.Exito)
            await CargarItemsAsync();

        Mensaje = resultado.Mensaje;
>>>>>>> Stashed changes
    }
}
