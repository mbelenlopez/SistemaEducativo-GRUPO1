using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services;

public class CalendarioService
{
    private readonly ClaseRepositorio _repositorio;
    public CalendarioService(ClaseRepositorio repositorio) => _repositorio = repositorio;

    public async Task<(bool Exito, string Mensaje)> AgregarClaseAsync(int comisionId, DateTime fecha)
    {
        if (!Fechas.EsValida(fecha))
            return (false, "Ingresá una fecha válida, por ejemplo 05/10/2026.");

        var dia = Fechas.SoloFecha(fecha);

        if (await _repositorio.ExisteAsync(comisionId, dia))
            return (false, "La fecha ya existe en el calendario de la comisión.");

        await _repositorio.AgregarAsync(new Clase { ComisionId = comisionId, Fecha = dia });
        return (true, "Clase agregada correctamente.");
    }

    public Task<List<Clase>> ObtenerClasesAsync(int comisionId) =>
        _repositorio.ObtenerPorComisionAsync(comisionId);
}
