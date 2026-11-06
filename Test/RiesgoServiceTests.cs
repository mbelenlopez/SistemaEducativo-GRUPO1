using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests
{

    // H6 y H8: el cálculo por estudiante y el panel ordenado.
    [TestClass]
    public class RiesgoServiceTests
    {
        private sealed class Escenario
        {
            public AsistenciaRepositorioFake Asistencias { get; } = new();
            public EntregaRepositorioFake Entregas { get; } = new();
            public EstudianteRepositorioFake Estudiantes { get; } = new();
            public RiesgoService Service { get; }

            public ConfiguracionRiesgoService Configuracion { get; } = new(new ConfiguracionRiesgoRepositorioFake());

            public Escenario() =>
                Service = new RiesgoService(Asistencias, Estudiantes, Entregas, new CalculadoraRiesgo(), Configuracion);

            public async Task<Estudiante> AgregarAsync(string apellido, int clases, int ausencias, int trabajos = 0, int noEntregados = 0, int comisionId = 1)
            {
                var estudiante = new Estudiante { ComisionId = comisionId, Apellido = apellido, Nombre = "N", Legajo = apellido };
                await Estudiantes.AgregarAsync(estudiante);

                for (int i = 0; i < clases; i++)
                    await Asistencias.AgregarAsync(new Asistencia
                    {
                        ClaseId = i + 1,
                        EstudianteId = estudiante.Id,
                        Condicion = i < ausencias ? CondicionAsistencia.Ausente : CondicionAsistencia.Presente
                    });

                for (int i = 0; i < trabajos; i++)
                    await Entregas.AgregarAsync(new Entrega
                    {
                        TrabajoId = i + 1,
                        EstudianteId = estudiante.Id,
                        Condicion = i < noEntregados ? CondicionEntrega.NoEntregado : CondicionEntrega.Entregado
                    });

                return estudiante;
            }
        }

        [TestMethod]
        public async Task Calcular_ContabilizaAusenciasYTrabajosNoEntregados()
        {
            var escenario = new Escenario();
            var marina = await escenario.AgregarAsync("Alvarez", clases: 6, ausencias: 4, trabajos: 2, noEntregados: 1);

            var riesgo = await escenario.Service.CalcularAsync(marina.Id);

            Assert.AreEqual(66.67, riesgo.PorcentajeInasistencias, 0.001);
            Assert.AreEqual(50.0, riesgo.PorcentajeNoEntregados, 0.001);
            Assert.AreEqual(NivelRiesgo.Alto, riesgo.Nivel);
        }

        // El caso del profe, de punta a punta: asistencia y ningún trabajo registrado.
        [TestMethod]
        public async Task Calcular_ConAsistenciaYSinTrabajos_NoMuestraNaN()
        {
            var escenario = new Escenario();
            var estudiante = await escenario.AgregarAsync("Gomez", clases: 10, ausencias: 1);

            var riesgo = await escenario.Service.CalcularAsync(estudiante.Id);

            Assert.IsFalse(double.IsNaN(riesgo.PorcentajeNoEntregados));
            Assert.AreEqual(NivelRiesgo.Bajo, riesgo.Nivel);
        }

        [TestMethod]
        public async Task CalcularComision_SoloIncluyeAlosEstudiantesDeEsaComision()
        {
            var escenario = new Escenario();
            await escenario.AgregarAsync("Uno", clases: 4, ausencias: 0, comisionId: 1);
            await escenario.AgregarAsync("Dos", clases: 4, ausencias: 0, comisionId: 2);

            var resultado = await escenario.Service.CalcularComisionAsync(1);

            Assert.AreEqual(1, resultado.Count);
            Assert.AreEqual("Uno, N", resultado[0].Estudiante.NombreCompleto);
        }

        // El orden del panel es una regla de negocio: primero riesgo alto, luego el más grave, luego por nombre.
        [TestMethod]
        public async Task ObtenerPanel_OrdenaPorNivelLuegoPorGravedadYLuegoPorNombre()
        {
            var escenario = new Escenario();
            await escenario.AgregarAsync("Perez", clases: 10, ausencias: 6);    // alto, 60
            await escenario.AgregarAsync("Gomez", clases: 10, ausencias: 9);    // alto, 90
            await escenario.AgregarAsync("Lopez", clases: 10, ausencias: 1);    // bajo
            await escenario.AgregarAsync("Diaz", clases: 0, ausencias: 0);      // sin datos
            await escenario.AgregarAsync("Alvarez", clases: 10, ausencias: 6);  // alto, 60 (empata con Perez)

            var panel = await escenario.Service.ObtenerPanelAsync(1);

            CollectionAssert.AreEqual(
                new[] { "Gomez, N", "Alvarez, N", "Perez, N", "Lopez, N", "Diaz, N" },
                panel.Select(p => p.Estudiante.NombreCompleto).ToArray());
        }

        [TestMethod]
        public async Task ObtenerPanel_SinEstudiantes_DevuelveListaVacia()
        {
            var escenario = new Escenario();

            var panel = await escenario.Service.ObtenerPanelAsync(1);

            Assert.AreEqual(0, panel.Count);
        }
    }

}
