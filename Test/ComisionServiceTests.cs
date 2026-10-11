using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

[TestClass]
public class ComisionServiceTests
{
    [TestMethod]
    public async Task ObtenerTodas_DevuelveLasComisionesOrdenadasPorNombre()
    {
        var repositorio = new ComisionRepositorioFake();
        repositorio.Datos.Add(new Comision { Id = 2, Nombre = "Informática 2026" });
        repositorio.Datos.Add(new Comision { Id = 1, Nombre = "Ingeniería 2026" });
        var service = new ComisionService(repositorio);

        var comisiones = await service.ObtenerTodasAsync();

        Assert.AreEqual(2, comisiones.Count);
        Assert.AreEqual("Informática 2026", comisiones[0].Nombre);
    }
}
