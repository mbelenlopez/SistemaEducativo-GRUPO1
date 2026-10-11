using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

[TestClass]
public class TrabajoServiceTests
{
    [TestMethod]
    public async Task Crear_GuardaElTrabajoConSuFechaLimite()
    {
        var repositorio = new TrabajoRepositorioFake();
        var service = new TrabajoService(repositorio);

        var resultado = await service.CrearAsync(1, "TP1", new DateTime(2026, 5, 1, 10, 0, 0));

        Assert.IsTrue(resultado.Exito);
        Assert.AreEqual("TP1", repositorio.Datos[0].Titulo);
        Assert.AreEqual(new DateTime(2026, 5, 1), repositorio.Datos[0].FechaLimite);
    }

    [TestMethod]
    public async Task Crear_GuardaElTituloSinEspaciosSobrantes()
    {
        var repositorio = new TrabajoRepositorioFake();
        var service = new TrabajoService(repositorio);

        await service.CrearAsync(1, "  TP1  ", new DateTime(2026, 5, 1));

        Assert.AreEqual("TP1", repositorio.Datos[0].Titulo);
    }

    [TestMethod]
    public async Task Crear_ExigeUnTitulo()
    {
        var service = new TrabajoService(new TrabajoRepositorioFake());

        var resultado = await service.CrearAsync(1, "   ", new DateTime(2026, 5, 1));

        Assert.IsFalse(resultado.Exito);
    }

    [TestMethod]
    public async Task Crear_RechazaUnaFechaLimiteSinCompletar()
    {
        var repositorio = new TrabajoRepositorioFake();
        var service = new TrabajoService(repositorio);

        var resultado = await service.CrearAsync(1, "TP1", default);

        Assert.IsFalse(resultado.Exito);
        Assert.AreEqual(0, repositorio.Datos.Count);
    }

    [TestMethod]
    public async Task ObtenerPorComision_DevuelveSoloLosDeEsaComisionOrdenadosPorFecha()
    {
        var service = new TrabajoService(new TrabajoRepositorioFake());
        await service.CrearAsync(1, "TP2", new DateTime(2026, 6, 1));
        await service.CrearAsync(1, "TP1", new DateTime(2026, 5, 1));
        await service.CrearAsync(2, "Otro", new DateTime(2026, 5, 15));

        var trabajos = await service.ObtenerPorComisionAsync(1);

        Assert.AreEqual(2, trabajos.Count);
        Assert.AreEqual("TP1", trabajos[0].Titulo);
        Assert.AreEqual("TP2", trabajos[1].Titulo);
    }
}
