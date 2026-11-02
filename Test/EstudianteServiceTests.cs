using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

[TestClass]
public class EstudianteServiceTests
{
    // CA1 de H1: se carga un estudiante y, si no trae fecha, queda con la de hoy.
    [TestMethod]
    public async Task AgregarEstudiante_LoGuardaYCompletaLaFechaDeIncorporacion()
    {
        var repositorio = new EstudianteRepositorioFake();
        var service = new EstudianteService(repositorio);

        var resultado = await service.AgregarAsync(new Estudiante
        {
            ComisionId = 1, Apellido = "Gomez", Nombre = "Martin", Legajo = "12345"
        });

        Assert.IsTrue(resultado.Exito);
        Assert.AreEqual(1, repositorio.Datos.Count);
        Assert.AreEqual(DateTime.Today, repositorio.Datos[0].FechaIncorporacion);
    }

    // CA3 de H1: un legajo ya usado se rechaza y la nómina no cambia.
    [TestMethod]
    public async Task AgregarEstudiante_NoPermiteLegajoDuplicado()
    {
        var repositorio = new EstudianteRepositorioFake();
        var service = new EstudianteService(repositorio);
        await service.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "Alvarez", Nombre = "Marina", Legajo = "12345" });

        var segundo = await service.AgregarAsync(new Estudiante { ComisionId = 2, Apellido = "Perez", Nombre = "Juan", Legajo = "12345" });

        Assert.IsFalse(segundo.Exito);
        Assert.AreEqual(1, repositorio.Datos.Count);
    }

    [TestMethod]
    public async Task AgregarEstudiante_ExigeApellidoNombreYLegajo()
    {
        var service = new EstudianteService(new EstudianteRepositorioFake());

        var resultado = await service.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "Gomez", Nombre = "", Legajo = "1" });

        Assert.IsFalse(resultado.Exito);
    }

    // CA1 de H1: la nómina se muestra ordenada por apellido.
    [TestMethod]
    public async Task ObtenerPorComision_DevuelveOrdenadoPorApellido()
    {
        var service = new EstudianteService(new EstudianteRepositorioFake());
        await service.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "Perez", Nombre = "Juan", Legajo = "100" });
        await service.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "Alvarez", Nombre = "Marina", Legajo = "101" });
        await service.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "Gomez", Nombre = "Martin", Legajo = "102" });

        var nomina = await service.ObtenerPorComisionAsync(1);

        Assert.AreEqual("Alvarez", nomina[0].Apellido);
        Assert.AreEqual("Gomez", nomina[1].Apellido);
        Assert.AreEqual("Perez", nomina[2].Apellido);
    }

    // CA4 de H1: abrir una comisión no muestra estudiantes de otra.
    [TestMethod]
    public async Task ObtenerPorComision_NoMezclaComisiones()
    {
        var service = new EstudianteService(new EstudianteRepositorioFake());
        await service.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "Alvarez", Nombre = "Marina", Legajo = "200" });
        await service.AgregarAsync(new Estudiante { ComisionId = 2, Apellido = "Diaz", Nombre = "Carla", Legajo = "201" });

        var nominaComision1 = await service.ObtenerPorComisionAsync(1);

        Assert.AreEqual(1, nominaComision1.Count);
        Assert.AreEqual("Alvarez", nominaComision1[0].Apellido);
    }

    // La pantalla entrega la fecha como DateTime y se guarda tal cual (sin la hora).
    [TestMethod]
    public async Task Incorporar_GuardaLaFechaDeIncorporacion()
    {
        var repositorio = new EstudianteRepositorioFake();
        var service = new EstudianteService(repositorio);

        var resultado = await service.IncorporarAsync(1, "Gomez", "Martin", "12345", new DateTime(2026, 3, 9, 15, 45, 0));

        Assert.IsTrue(resultado.Exito);
        Assert.AreEqual(new DateTime(2026, 3, 9), repositorio.Datos[0].FechaIncorporacion);
        Assert.AreEqual(1, repositorio.Datos[0].ComisionId);
    }

    [TestMethod]
    public async Task Incorporar_AplicaLasMismasReglasQueAgregar()
    {
        var service = new EstudianteService(new EstudianteRepositorioFake());
        await service.IncorporarAsync(1, "Alvarez", "Marina", "12345", new DateTime(2026, 3, 9));

        var repetido = await service.IncorporarAsync(1, "Perez", "Juan", "12345", new DateTime(2026, 3, 9));

        Assert.IsFalse(repetido.Exito);
    }
}
