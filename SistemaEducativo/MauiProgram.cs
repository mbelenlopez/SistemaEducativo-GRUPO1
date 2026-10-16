using SistemaEducativo.Repositorios;
using SistemaEducativo.Services;
using SistemaEducativo.View;
using SistemaEducativo.ViewModel;

namespace SistemaEducativo;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();

        builder.UseMauiApp<App>();

        // ---- Datos: una sola conexión a SQLite compartida por todos los repositorios ----
        builder.Services.AddSingleton<Database>();

        // ---- Repositorios: cada interfaz (puerto) se conecta con su implementación SQL ----
        builder.Services.AddSingleton<ComisionRepositorio, ComisionRepositorioSQL>();
        builder.Services.AddSingleton<EstudianteRepositorio, EstudianteRepositorioSQL>();
        builder.Services.AddSingleton<ClaseRepositorio, ClaseRepositorioSQL>();
        builder.Services.AddSingleton<TrabajoRepositorio, TrabajoRepositorioSQL>(); 
        builder.Services.AddSingleton<AsistenciaRepositorio, AsistenciaRepositorioSQL>();
        builder.Services.AddSingleton<EntregaRepositorio, EntregaRepositorioSQL>();


        // ---- Services: la lógica de negocio ----
        builder.Services.AddSingleton<ComisionService>();
        builder.Services.AddSingleton<EstudianteService>();
        builder.Services.AddSingleton<CalendarioService>();
        builder.Services.AddSingleton<TrabajoService>();
        builder.Services.AddSingleton<AsistenciaService>();
        builder.Services.AddSingleton<CalculadoraRiesgo>();
        builder.Services.AddSingleton<RiesgoService>();
        builder.Services.AddSingleton<EntregaService>();

        // ---- ViewModel: el estado y las acciones de cada página ----
        builder.Services.AddSingleton<SesionActual>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<NominaViewModel>();
        builder.Services.AddSingleton<CalendarioViewModel>();
        builder.Services.AddSingleton<TrabajoViewModel>();
        builder.Services.AddSingleton<AsistenciaViewModel>();
        builder.Services.AddSingleton<RiesgoViewModel>();

        // ---- View: las páginas (el AppShell las abre desde el menú lateral) ----
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NominaPage>();
        builder.Services.AddTransient<CalendarioPage>();
        builder.Services.AddTransient<TrabajoPage>();
        builder.Services.AddTransient<AsistenciaPage>();
        builder.Services.AddTransient<RiesgoPage>();

        return builder.Build();
    }
}
