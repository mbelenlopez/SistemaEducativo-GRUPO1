using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;

namespace SistemaEducativo.Tests;

// Pruebas de INTEGRACIÓN: usan SQLite de verdad (un archivo temporal por prueba) para comprobar
// cómo se guardan y se leen las fechas y que las consultas por comisión funcionen.
// Si fallan por un problema de la PC (por ejemplo, la librería nativa de SQLite), este archivo
// se puede borrar sin afectar a la aplicación.
[TestClass]
[TestCategory("Integracion")]
public class PersistenciaFechasTests
{
    // Una fila "cruda" para leer qué tipo de dato guardó SQLite.
    public class FilaTipo
    {
        public string Tipo { get; set; } = string.Empty;
    }

    [ClassInitialize]
    public static void Inicializar(TestContext contexto) => SQLitePCL.Batteries_V2.Init();

    private static Database NuevaBase() =>
        new(Path.Combine(Path.GetTempPath(), $"sistema-test-{Guid.NewGuid():N}.db3"));

    [TestMethod]
    public async Task Clase_SeGuardaYSeLeeConLaMismaFecha()
    {
        var repositorio = new ClaseRepositorioSQL(NuevaBase());

        await repositorio.AgregarAsync(new Clase { ComisionId = 1, Fecha = new DateTime(2026, 10, 5) });
        var clases = await repositorio.ObtenerPorComisionAsync(1);

        Assert.AreEqual(1, clases.Count);
        Assert.AreEqual(new DateTime(2026, 10, 5), clases[0].Fecha);
    }

    // storeDateTimeAsTicks: false -> la fecha queda como texto, no como un número.
    [TestMethod]
    public async Task Fecha_SeGuardaComoTextoYNoComoNumero()
    {
        var db = NuevaBase();
        await new ClaseRepositorioSQL(db).AgregarAsync(new Clase { ComisionId = 1, Fecha = new DateTime(2026, 10, 5) });

        var filas = await db.QueryAsync<FilaTipo>("SELECT typeof(Fecha) AS Tipo FROM Clases");

        Assert.AreEqual("text", filas.Single().Tipo);
    }

    [TestMethod]
    public async Task ExisteAsync_EncuentraLaFechaGuardadaYNoOtras()
    {
        var repositorio = new ClaseRepositorioSQL(NuevaBase());
        await repositorio.AgregarAsync(new Clase { ComisionId = 1, Fecha = new DateTime(2026, 10, 5) });

        Assert.IsTrue(await repositorio.ExisteAsync(1, new DateTime(2026, 10, 5)));
        Assert.IsFalse(await repositorio.ExisteAsync(1, new DateTime(2026, 10, 6)));
        Assert.IsFalse(await repositorio.ExisteAsync(2, new DateTime(2026, 10, 5)));
    }

    [TestMethod]
    public async Task ObtenerPorComision_OrdenaLasClasesCronologicamente()
    {
        var repositorio = new ClaseRepositorioSQL(NuevaBase());
        await repositorio.AgregarAsync(new Clase { ComisionId = 1, Fecha = new DateTime(2026, 10, 12) });
        await repositorio.AgregarAsync(new Clase { ComisionId = 1, Fecha = new DateTime(2026, 9, 20) });
        await repositorio.AgregarAsync(new Clase { ComisionId = 1, Fecha = new DateTime(2026, 10, 5) });

        var clases = await repositorio.ObtenerPorComisionAsync(1);

        CollectionAssert.AreEqual(
            new[] { new DateTime(2026, 9, 20), new DateTime(2026, 10, 5), new DateTime(2026, 10, 12) },
            clases.Select(c => c.Fecha).ToArray());
    }

    // Esta prueba habría detectado el error de H2: la consulta de trabajos no recibía la comisión.
    [TestMethod]
    public async Task Trabajo_ObtenerPorComision_FiltraPorComisionYOrdenaPorFecha()
    {
        var repositorio = new TrabajoRepositorioSQL(NuevaBase());
        await repositorio.AgregarAsync(new Trabajo { ComisionId = 1, Titulo = "TP2", FechaLimite = new DateTime(2026, 6, 1) });
        await repositorio.AgregarAsync(new Trabajo { ComisionId = 1, Titulo = "TP1", FechaLimite = new DateTime(2026, 5, 1) });
        await repositorio.AgregarAsync(new Trabajo { ComisionId = 2, Titulo = "Otro", FechaLimite = new DateTime(2026, 5, 15) });

        var trabajos = await repositorio.ObtenerPorComisionAsync(1);

        CollectionAssert.AreEqual(new[] { "TP1", "TP2" }, trabajos.Select(t => t.Titulo).ToArray());
        Assert.AreEqual(new DateTime(2026, 5, 1), trabajos[0].FechaLimite);
    }

    [TestMethod]
    public async Task Estudiante_FechaDeIncorporacion_SeGuardaYSeLee()
    {
        var repositorio = new EstudianteRepositorioSQL(NuevaBase());
        await repositorio.AgregarAsync(new Estudiante
        {
            ComisionId = 1, Apellido = "Gomez", Nombre = "Martin", Legajo = "12345",
            FechaIncorporacion = new DateTime(2026, 3, 9)
        });

        var estudiante = await repositorio.ObtenerPorLegajoAsync("12345");

        Assert.IsNotNull(estudiante);
        Assert.AreEqual(new DateTime(2026, 3, 9), estudiante.FechaIncorporacion);
    }

    [TestMethod]
    public async Task QueryPorIds_DevuelveSoloLosIdsPedidosYNadaConUnaListaVacia()
    {
        var db = NuevaBase();
        var repositorio = new EstudianteRepositorioSQL(db);
        foreach (var legajo in new[] { "1", "2", "3" })
            await repositorio.AgregarAsync(new Estudiante { ComisionId = 1, Apellido = "A" + legajo, Nombre = "N", Legajo = legajo });

        var algunos = await db.QueryPorIdsAsync<Estudiante>("Estudiantes", "Id", new[] { 1, 3 });
        var ninguno = await db.QueryPorIdsAsync<Estudiante>("Estudiantes", "Id", Array.Empty<int>());

        CollectionAssert.AreEquivalent(new[] { 1, 3 }, algunos.Select(e => e.Id).ToArray());
        Assert.AreEqual(0, ninguno.Count);
    }
}
