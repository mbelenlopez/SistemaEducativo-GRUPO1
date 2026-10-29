using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Services
{
    public class RiesgoService
    {
        // Umbrales vigentes (H7): los que configuró el tutor o, si nunca configuró nada, los de por defecto (50%).
        // Se refrescan en cada cálculo y se exponen para que las pantallas puedan mostrarlos.
        public double UmbralInasistencias { get; private set; } = ConfiguracionRiesgoService.UmbralPorDefecto;
        public double UmbralNoEntregados { get; private set; } = ConfiguracionRiesgoService.UmbralPorDefecto;

        private readonly AsistenciaRepositorio _asistenciaRepositorio;
        private readonly EstudianteRepositorio _estudianteRepositorio;
        private readonly EntregaRepositorio _entregaRepositorio;
        private readonly CalculadoraRiesgo _calculadoraRiesgo;
        private readonly ConfiguracionRiesgoService _configuracion;

        public RiesgoService(
            AsistenciaRepositorio asistenciaRepositorio,
            EstudianteRepositorio estudianteRepositorio,
            EntregaRepositorio entregaRepositorio,
            CalculadoraRiesgo calculadoraRiesgo,
            ConfiguracionRiesgoService configuracion)
        {
            _asistenciaRepositorio = asistenciaRepositorio;
            _estudianteRepositorio = estudianteRepositorio;
            _entregaRepositorio = entregaRepositorio;
            _calculadoraRiesgo = calculadoraRiesgo;
            _configuracion = configuracion;
        }

        // Lee los umbrales vigentes y los deja en UmbralInasistencias / UmbralNoEntregados.
        public async Task<UmbralesRiesgo> CargarUmbralesAsync()
        {
            var umbrales = await _configuracion.ObtenerAsync();
            UmbralInasistencias = umbrales.Inasistencias;
            UmbralNoEntregados = umbrales.NoEntregados;
            return umbrales;
        }

        // Calcula el riesgo de un estudiante basado en su id
        public async Task<ResultadoRiesgo> CalcularAsync(int estudianteId)
        {
            var umbrales = await CargarUmbralesAsync();
            return await CalcularConUmbralesAsync(estudianteId, umbrales);
        }

        // Calcula el riesgo de todos los estudiantes de una comisión
        public async Task<List<EstudianteConRiesgo>> CalcularComisionAsync(int comisionId)
        {
            var umbrales = await CargarUmbralesAsync();
            var estudiantes = await _estudianteRepositorio.ObtenerPorComisionAsync(comisionId);
            var resultado = new List<EstudianteConRiesgo>();

            foreach (var estudiante in estudiantes)
            {
                var riesgo = await CalcularAsync(estudiante.Id, umbrales);
                resultado.Add(new EstudianteConRiesgo(estudiante, riesgo));
            }

            return resultado;
        }

        private async Task<ResultadoRiesgo> CalcularConUmbralesAsync(int estudianteId, UmbralesRiesgo umbrales)
        {
            var asistencia = await _asistenciaRepositorio.ObtenerPorEstudianteAsync(estudianteId);
            var entregas = await _entregaRepositorio.ObtenerPorEstudianteAsync(estudianteId);

            return _calculadoraRiesgo.Calcular(
                clasesTotales: asistencia.Count,
                ausencias: asistencia.Count(a => a.Condicion == "Ausente"),
                trabajosTotales: entregas.Count,
                noEntregados: entregas.Count(e => e.Condicion == "No entregado"),
                umbralInasistencias: umbrales.Inasistencias,
                umbralNoEntregados: umbrales.NoEntregados
            );
        }
    }
}
