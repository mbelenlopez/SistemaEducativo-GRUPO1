using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

[TestClass]
public class FechasTests
{
    [TestMethod]
    public void EsValida_RechazaUnaFechaSinCompletar()
    {
        Assert.IsFalse(Fechas.EsValida(default));
        Assert.IsTrue(Fechas.EsValida(new DateTime(2026, 10, 5)));
    }

    [TestMethod]
    public void SoloFecha_DescartaLaHora()
    {
        Assert.AreEqual(new DateTime(2026, 10, 5), Fechas.SoloFecha(new DateTime(2026, 10, 5, 18, 30, 0)));
    }
}
