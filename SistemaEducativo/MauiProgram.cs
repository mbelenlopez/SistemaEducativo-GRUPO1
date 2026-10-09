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

        // ---- Services: la lógica de negocio ----
        builder.Services.AddSingleton<ComisionService>();
        builder.Services.AddSingleton<EstudianteService>();

        // ---- ViewModel: el estado y las acciones de cada página ----
        builder.Services.AddSingleton<SesionActual>();
        builder.Services.AddSingleton<MainViewModel>();
        builder.Services.AddSingleton<NominaViewModel>();

        // ---- View: las páginas (el AppShell las abre desde el menú lateral) ----
        builder.Services.AddTransient<MainPage>();
        builder.Services.AddTransient<NominaPage>();

        return builder.Build();
    }
}
