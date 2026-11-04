using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

// Pruebas de INTEGRACIÓN de H10: SQLite de verdad (un archivo temporal por prueba).
[TestClass]
[TestCategory("Integracion")]
public class PersistenciaContactoTests
{
    [ClassInitialize]
    public static void Inicializar(TestContext contexto) => SQLitePCL.Batteries_V2.Init();

    private static ContactoRepositorioSQL NuevoRepositorio() =>
        new(new Database(Path.Combine(Path.GetTempPath(), $"sistema-test-{Guid.NewGuid():N}.db3")));

    // Si ContactoRegistro no tuviera el campo Tipo, este dato se perdería al guardar.
    [TestMethod]
    public async Task Contacto_GuardaYLeeElTipo()
    {
        var repositorio = NuevoRepositorio();

        await repositorio.AgregarAsync(new Contacto
        {
            EstudianteId = 1,
            Fecha = new DateTime(2026, 5, 20),
            Tipo = TiposContacto.Email,
            Resultado = "Respondió"
        });
        var contactos = await repositorio.ObtenerPorEstudianteAsync(1);

        Assert.AreEqual(1, contactos.Count);
        Assert.AreEqual(TiposContacto.Email, contactos[0].Tipo);
        Assert.AreEqual("Respondió", contactos[0].Resultado);
        Assert.AreEqual(new DateTime(2026, 5, 20), contactos[0].Fecha);
    }

    [TestMethod]
    public async Task Contacto_AlGuardarLaEntidadRecibeSuId()
    {
        var repositorio = NuevoRepositorio();
        var contacto = new Contacto { EstudianteId = 1, Fecha = DateTime.Today, Tipo = TiposContacto.Llamada, Resultado = "Ok" };

        await repositorio.AgregarAsync(contacto);

        Assert.IsTrue(contacto.Id > 0);
    }
}
