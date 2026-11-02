using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

[TestClass]
public class CalendarioServiceTests
{
    private static readonly DateTime QuinceDeAbril = new(2026, 4, 15);

    // CA3 de H2: no se puede duplicar una fecha de clase en la misma comisión.
    [TestMethod]
    public async Task AgregarClase_NoPermiteFechaDuplicadaEnLaComision()
    {
        var service = new CalendarioService(new ClaseRepositorioFake());
        await service.AgregarClaseAsync(1, QuinceDeAbril);

        var segunda = await service.AgregarClaseAsync(1, QuinceDeAbril);

        Assert.IsFalse(segunda.Exito);
    }

    [TestMethod]
    public async Task AgregarClase_LaMismaFechaEnOtraComisionSiSePermite()
    {
        var service = new CalendarioService(new ClaseRepositorioFake());
        await service.AgregarClaseAsync(1, QuinceDeAbril);

        var otraComision = await service.AgregarClaseAsync(2, QuinceDeAbril);

        Assert.IsTrue(otraComision.Exito);
    }

    [TestMethod]
    public async Task AgregarClase_RechazaUnaFechaSinCompletar()
    {
        var service = new CalendarioService(new ClaseRepositorioFake());

        var resultado = await service.AgregarClaseAsync(1, default);

        Assert.IsFalse(resultado.Exito);
    }

    // La clase se guarda como día completo: la hora no cuenta.
    [TestMethod]
    public async Task AgregarClase_GuardaSoloElDia()
    {
        var repositorio = new ClaseRepositorioFake();
        var service = new CalendarioService(repositorio);

        await service.AgregarClaseAsync(1, QuinceDeAbril.AddHours(18).AddMinutes(30));

        Assert.AreEqual(QuinceDeAbril, repositorio.Datos[0].Fecha);
    }

    // La misma fecha con otra hora sigue siendo la misma clase.
    [TestMethod]
    public async Task AgregarClase_LaHoraNoCambiaLaIdentidadDeLaFecha()
    {
        var service = new CalendarioService(new ClaseRepositorioFake());
        await service.AgregarClaseAsync(1, QuinceDeAbril);

        var conOtraHora = await service.AgregarClaseAsync(1, QuinceDeAbril.AddHours(18));

        Assert.IsFalse(conOtraHora.Exito);
    }

    [TestMethod]
    public async Task ObtenerClases_DevuelveLasDeLaComisionEnOrdenCronologico()
    {
        var service = new CalendarioService(new ClaseRepositorioFake());
        await service.AgregarClaseAsync(1, new DateTime(2026, 4, 22));
        await service.AgregarClaseAsync(1, new DateTime(2026, 4, 8));
        await service.AgregarClaseAsync(2, new DateTime(2026, 4, 1));

        var clases = await service.ObtenerClasesAsync(1);

        Assert.AreEqual(2, clases.Count);
        Assert.AreEqual(new DateTime(2026, 4, 8), clases[0].Fecha);
        Assert.AreEqual(new DateTime(2026, 4, 22), clases[1].Fecha);
    }
}
