using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

// H10: registrar un contacto con un estudiante.
[TestClass]
public class ContactoServiceTests
{
    private static (ContactoService Service, ContactoRepositorioFake Repositorio) Nuevo()
    {
        var repositorio = new ContactoRepositorioFake();
        return (new ContactoService(repositorio), repositorio);
    }

    [TestMethod]
    public async Task Registrar_ConDatosValidos_GuardaElContacto()
    {
        var (service, repositorio) = Nuevo();

        var resultado = await service.RegistrarAsync(7, new DateTime(2026, 5, 20, 15, 30, 0), TiposContacto.Llamada, "No atendió");

        Assert.IsTrue(resultado.Exito);
        Assert.AreEqual("Contacto registrado correctamente.", resultado.Mensaje);
        Assert.AreEqual(1, repositorio.Datos.Count);
        Assert.AreEqual(7, repositorio.Datos[0].EstudianteId);
        Assert.AreEqual(TiposContacto.Llamada, repositorio.Datos[0].Tipo);
        Assert.AreEqual("No atendió", repositorio.Datos[0].Resultado);
    }

    [TestMethod]
    public async Task Registrar_GuardaSoloLaFechaSinLaHora()
    {
        var (service, repositorio) = Nuevo();

        await service.RegistrarAsync(7, new DateTime(2026, 5, 20, 15, 30, 0), TiposContacto.Mensaje, "Respondió");

        Assert.AreEqual(new DateTime(2026, 5, 20), repositorio.Datos[0].Fecha);
    }

    [TestMethod]
    public async Task Registrar_QuitaLosEspaciosDelResultado()
    {
        var (service, repositorio) = Nuevo();

        await service.RegistrarAsync(7, DateTime.Today, TiposContacto.Email, "   Quedó en venir   ");

        Assert.AreEqual("Quedó en venir", repositorio.Datos[0].Resultado);
    }

    [TestMethod]
    public async Task Registrar_AceptaLosTresTiposDeContacto()
    {
        foreach (var tipo in TiposContacto.Todos)
        {
            var (service, _) = Nuevo();

            var resultado = await service.RegistrarAsync(7, DateTime.Today, tipo, "Ok");

            Assert.IsTrue(resultado.Exito, tipo);
        }
    }

    [TestMethod]
    public async Task Registrar_RechazaUnTipoInvalidoOVacio_YNoGuardaNada()
    {
        foreach (var tipo in new[] { "", "Carta", "llamada" })
        {
            var (service, repositorio) = Nuevo();

            var resultado = await service.RegistrarAsync(7, DateTime.Today, tipo, "Ok");

            Assert.IsFalse(resultado.Exito, $"Debería rechazar el tipo \"{tipo}\".");
            Assert.IsTrue(resultado.Mensaje.Contains("llamada, mensaje o email"), resultado.Mensaje);
            Assert.AreEqual(0, repositorio.Datos.Count);
        }
    }

    [TestMethod]
    public async Task Registrar_RechazaUnResultadoVacio_YNoGuardaNada()
    {
        foreach (var resultadoTexto in new[] { "", "   " })
        {
            var (service, repositorio) = Nuevo();

            var resultado = await service.RegistrarAsync(7, DateTime.Today, TiposContacto.Llamada, resultadoTexto);

            Assert.IsFalse(resultado.Exito);
            Assert.IsTrue(resultado.Mensaje.Contains("resultado"), resultado.Mensaje);
            Assert.AreEqual(0, repositorio.Datos.Count);
        }
    }

    [TestMethod]
    public async Task ObtenerPorEstudiante_SoloDevuelveLosContactosDeEseEstudiante()
    {
        var (service, _) = Nuevo();
        await service.RegistrarAsync(1, DateTime.Today, TiposContacto.Llamada, "A");
        await service.RegistrarAsync(2, DateTime.Today, TiposContacto.Llamada, "B");
        await service.RegistrarAsync(1, DateTime.Today, TiposContacto.Email, "C");

        var contactos = await service.ObtenerPorEstudianteAsync(1);

        Assert.AreEqual(2, contactos.Count);
        Assert.IsTrue(contactos.All(c => c.EstudianteId == 1));
    }

    // ---- H11: el último contacto de cada estudiante en el panel ----

    [TestMethod]
    public async Task UltimosContactos_DevuelveLaFechaMasReciente()
    {
        var (service, _) = Nuevo();
        await service.RegistrarAsync(1, new DateTime(2026, 5, 10), TiposContacto.Llamada, "A");
        await service.RegistrarAsync(1, new DateTime(2026, 5, 20), TiposContacto.Mensaje, "B");
        await service.RegistrarAsync(1, new DateTime(2026, 5, 15), TiposContacto.Email, "C");

        var ultimos = await service.ObtenerUltimosContactosAsync(new List<int> { 1 });

        Assert.AreEqual(new DateTime(2026, 5, 20), ultimos[1]);
    }

    [TestMethod]
    public async Task UltimosContactos_QuienNuncaFueContactadoNoAparece()
    {
        var (service, _) = Nuevo();
        await service.RegistrarAsync(1, new DateTime(2026, 5, 10), TiposContacto.Llamada, "A");

        var ultimos = await service.ObtenerUltimosContactosAsync(new List<int> { 1, 2 });

        Assert.IsTrue(ultimos.ContainsKey(1));
        Assert.IsFalse(ultimos.ContainsKey(2));
    }

    [TestMethod]
    public async Task UltimosContactos_CadaEstudianteTieneLaFechaDeSusPropiosContactos()
    {
        var (service, _) = Nuevo();
        await service.RegistrarAsync(1, new DateTime(2026, 5, 10), TiposContacto.Llamada, "A");
        await service.RegistrarAsync(2, new DateTime(2026, 5, 20), TiposContacto.Llamada, "B");

        var ultimos = await service.ObtenerUltimosContactosAsync(new List<int> { 1, 2 });

        Assert.AreEqual(new DateTime(2026, 5, 10), ultimos[1]);
        Assert.AreEqual(new DateTime(2026, 5, 20), ultimos[2]);
    }

    [TestMethod]
    public async Task UltimosContactos_SinEstudiantes_DevuelveVacio()
    {
        var (service, _) = Nuevo();

        var ultimos = await service.ObtenerUltimosContactosAsync(new List<int>());

        Assert.AreEqual(0, ultimos.Count);
    }

    [TestMethod]
    public void EsReciente_HastaSieteDias_SiEsReciente_DesdeOchoNo()
    {
        var hoy = new DateTime(2026, 5, 27);

        Assert.IsTrue(ContactoService.EsReciente(new DateTime(2026, 5, 27), hoy), "hoy");
        Assert.IsTrue(ContactoService.EsReciente(new DateTime(2026, 5, 20), hoy), "hace 7 días");
        Assert.IsFalse(ContactoService.EsReciente(new DateTime(2026, 5, 19), hoy), "hace 8 días");
    }
}
