
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services
{
    public class CalendarioService
    {
        public ClaseRepositorio _repositorio;
        public CalendarioService(ClaseRepositorio repositorio) => _repositorio = repositorio;

        public async Task<(bool Exito, string Mensaje)> AgregarClaseAsync(int comisionId, DateTime fecha)
        {
            if (await _repositorio.ExisteAsync(comisionId, fecha))
                return (false, "La fecha ya existe en el calendario de la comisión.");

            await _repositorio.AgregarAsync(
                new Clase
                {
                    ComisionId = comisionId,
                    Fecha = fecha
                });

            return (true, "Clase agregada correctamente.");
        }

        public Task<List<Clase>> ObtenerClasesAsync(int comisionId)
            => _repositorio.ObtenerPorComisionAsync(comisionId);
    }
}
