using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

// H5: registrar quién entregó un trabajo.
[TestClass]
public class EntregaServiceTests
{
    private sealed class Escenario
    {
        public EntregaRepositorioFake Entregas { get; } = new();
        public EstudianteRepositorioFake Estudiantes { get; } = new();
        public EntregaService Service { get; }

        public Escenario() => Service = new EntregaService(Entregas, Estudiantes);

        public async Task<Estudiante> AgregarEstudianteAsync(string legajo, int comisionId = 1)
        {
            var estudiante = new Estudiante { ComisionId = comisionId, Apellido = "Apellido" + legajo, Nombre = "N", Legajo = legajo };
            await Estudiantes.AgregarAsync(estudiante);
            return estudiante;
        }
    }

    [TestMethod]
    public async Task Registrar_MarcaEntregadoALosElegidosYNoEntregadoAlResto()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        var b = await escenario.AgregarEstudianteAsync("2");

        var resultado = await escenario.Service.RegistrarEntregasAsync(trabajoId: 5, comisionId: 1, entregaron: new[] { a.Id });
        var guardadas = await escenario.Service.ObtenerPorTrabajoAsync(5);

        Assert.IsTrue(resultado.Exito);
        Assert.AreEqual(2, guardadas.Count);
        Assert.AreEqual(CondicionEntrega.Entregado, guardadas.Single(e => e.EstudianteId == a.Id).Condicion);
        Assert.AreEqual(CondicionEntrega.NoEntregado, guardadas.Single(e => e.EstudianteId == b.Id).Condicion);
    }

    [TestMethod]
    public async Task Registrar_RechazaSiElTrabajoYaTieneEntregas()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        await escenario.Service.RegistrarEntregasAsync(5, 1, new[] { a.Id });

        var segunda = await escenario.Service.RegistrarEntregasAsync(5, 1, new[] { a.Id });

        Assert.IsFalse(segunda.Exito);
        Assert.IsTrue(segunda.Mensaje.Contains("ya fueron registradas"));
        Assert.AreEqual(1, escenario.Entregas.Datos.Count);   // no se duplicó
    }

    [TestMethod]
    public async Task Registrar_RechazaSiLaComisionNoTieneEstudiantes()
    {
        var escenario = new Escenario();

        var resultado = await escenario.Service.RegistrarEntregasAsync(5, 1, Array.Empty<int>());

        Assert.IsFalse(resultado.Exito);
        Assert.AreEqual(0, escenario.Entregas.Datos.Count);
    }

    [TestMethod]
    public async Task Registrar_SoloIncluyeALosEstudiantesDeEsaComision()
    {
        var escenario = new Escenario();
        var deLaComision = await escenario.AgregarEstudianteAsync("1", comisionId: 1);
        await escenario.AgregarEstudianteAsync("2", comisionId: 2);

        await escenario.Service.RegistrarEntregasAsync(5, 1, new[] { deLaComision.Id });

        Assert.AreEqual(1, escenario.Entregas.Datos.Count);
        Assert.AreEqual(deLaComision.Id, escenario.Entregas.Datos[0].EstudianteId);
    }
}
