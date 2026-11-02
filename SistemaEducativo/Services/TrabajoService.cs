
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services
{
    public class TrabajoService
    {
        private readonly TrabajoRepositorio _repositorio;

        public TrabajoService(TrabajoRepositorio repositorio) => _repositorio = repositorio;

        public Task<List<Trabajo>> ObtenerPorComisionAsync(int comisionId)
            => _repositorio.ObtenerPorComisionAsync(comisionId);

        public async Task<(bool Exito, string Mensaje)> CrearAsync(int comisionId, string titulo, DateTime fechaLimite)
        {
            if (string.IsNullOrWhiteSpace(titulo))
                return (false, "El trabajo necesita un título.");
           
            await _repositorio.AgrearAsync(
                    new Trabajo{
                        ComisionId = comisionId,
                        Titulo = titulo,
                        FechaLimite = fechaLimite
                    });

            return (true, "Trabajo creado correctamente.");
        }
    }
}