using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

// H3 y H4: registrar y corregir la asistencia de una clase.
[TestClass]
public class AsistenciaServiceTests
{   
    private sealed class Escenario
    {
        public AsistenciaRepositorioFake Asistencias { get; } = new();
        public EstudianteRepositorioFake Estudiantes { get; } = new();
        public AsistenciaService Service { get; }

        public Escenario() =>
            Service = new AsistenciaService(Asistencias, Estudiantes, new ClaseRepositorioFake());

        public async Task<Estudiante> AgregarEstudianteAsync(string legajo, int comisionId = 1)
        {
            var estudiante = new Estudiante { ComisionId = comisionId, Apellido = "Apellido" + legajo, Nombre = "N", Legajo = legajo };
            await Estudiantes.AgregarAsync(estudiante);
            return estudiante;
        }

        public string CondicionDe(int claseId, int estudianteId) =>
            Asistencias.Datos.Single(a => a.ClaseId == claseId && a.EstudianteId == estudianteId).Condicion;
    }

    [TestMethod]
    public async Task Registrar_MarcaPresenteALosElegidosYAusenteAlResto()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        var b = await escenario.AgregarEstudianteAsync("2");
        var c = await escenario.AgregarEstudianteAsync("3");

        var resultado = await escenario.Service.RegistrarAsistenciaAsync(claseId: 10, comisionId: 1, presentes: new[] { a.Id, b.Id });

        Assert.IsTrue(resultado.Exito);
        Assert.AreEqual(3, escenario.Asistencias.Datos.Count);
        Assert.AreEqual(CondicionAsistencia.Presente.ToString(), escenario.CondicionDe(10, a.Id));
        Assert.AreEqual(CondicionAsistencia.Presente.ToString(), escenario.CondicionDe(10, b.Id));
        Assert.AreEqual(CondicionAsistencia.Ausente.ToString(), escenario.CondicionDe(10, c.Id));
    }

    [TestMethod]
    public async Task Registrar_RechazaSiLaClaseYaTieneAsistencia()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        await escenario.Service.RegistrarAsistenciaAsync(10, 1, new[] { a.Id });

        var segunda = await escenario.Service.RegistrarAsistenciaAsync(10, 1, new[] { a.Id });

        Assert.IsFalse(segunda.Exito);
        Assert.IsTrue(segunda.Mensaje.Contains("ya ha sido registrada"));
        Assert.AreEqual(1, escenario.Asistencias.Datos.Count);   // no se duplicó
    }

    [TestMethod]
    public async Task Registrar_RechazaSiLaComisionNoTieneEstudiantes()
    {
        var escenario = new Escenario();

        var resultado = await escenario.Service.RegistrarAsistenciaAsync(10, 1, Array.Empty<int>());

        Assert.IsFalse(resultado.Exito);
        Assert.AreEqual(0, escenario.Asistencias.Datos.Count);
    }

    [TestMethod]
    public async Task Registrar_SoloIncluyeALosEstudiantesDeEsaComision()
    {
        var escenario = new Escenario();
        var deLaComision = await escenario.AgregarEstudianteAsync("1", comisionId: 1);
        await escenario.AgregarEstudianteAsync("2", comisionId: 2);

        await escenario.Service.RegistrarAsistenciaAsync(10, 1, new[] { deLaComision.Id });

        Assert.AreEqual(1, escenario.Asistencias.Datos.Count);
        Assert.AreEqual(deLaComision.Id, escenario.Asistencias.Datos[0].EstudianteId);
    }

    [TestMethod]
    public async Task Corregir_CambiaLaCondicionDeCadaEstudiante()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        var b = await escenario.AgregarEstudianteAsync("2");
        await escenario.Service.RegistrarAsistenciaAsync(10, 1, new[] { a.Id });   // a presente, b ausente

        var resultado = await escenario.Service.CorregirAsistenciaAsync(
            10, 1, new Dictionary<int, bool> { [a.Id] = false, [b.Id] = true });   // ahora al revés

        Assert.IsTrue(resultado.Item1);
        Assert.AreEqual(CondicionAsistencia.Ausente.ToString(), escenario.CondicionDe(10, a.Id));
        Assert.AreEqual(CondicionAsistencia.Presente.ToString(), escenario.CondicionDe(10, b.Id));
        Assert.AreEqual(2, escenario.Asistencias.Datos.Count);   // corregir no duplica filas
    }

    [TestMethod]
    public async Task Corregir_ElQueNoViene_QuedaAusente()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        var b = await escenario.AgregarEstudianteAsync("2");
        await escenario.Service.RegistrarAsistenciaAsync(10, 1, new[] { a.Id, b.Id });

        await escenario.Service.CorregirAsistenciaAsync(10, 1, new Dictionary<int, bool> { [a.Id] = true });

        Assert.AreEqual(CondicionAsistencia.Presente.ToString(), escenario.CondicionDe(10, a.Id));
        Assert.AreEqual(CondicionAsistencia.Ausente.ToString(), escenario.CondicionDe(10, b.Id));
    }

    [TestMethod]
    public async Task Corregir_CompletaALosEstudiantesQueNoTeniaRegistro()
    {
        var escenario = new Escenario();
        var a = await escenario.AgregarEstudianteAsync("1");
        await escenario.Service.RegistrarAsistenciaAsync(10, 1, new[] { a.Id });
        var nuevo = await escenario.AgregarEstudianteAsync("2");   // se incorpora después

        await escenario.Service.CorregirAsistenciaAsync(
            10, 1, new Dictionary<int, bool> { [a.Id] = true, [nuevo.Id] = true });

        Assert.AreEqual(2, escenario.Asistencias.Datos.Count);
        Assert.AreEqual(CondicionAsistencia.Presente.ToString(), escenario.CondicionDe(10, nuevo.Id));
    }
}
