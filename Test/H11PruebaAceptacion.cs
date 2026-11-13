using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using SistemaEducativo.Repositorios;
using SistemaEducativo.Services;
using SistemaEducativo.ViewModel;
using SistemaEducativo.Entidades;
using Assert = Xunit.Assert;

namespace SistemaEducativo.Tests
{
    public class H11PruebaAceptacion : IDisposable
    {
        private readonly string _rutaDbTemporal;

        public H11PruebaAceptacion()
        {
            // Creamos una ruta para una base de datos temporal única en la PC
            _rutaDbTemporal = Path.Combine(Path.GetTempPath(), $"test_h11_{Guid.NewGuid()}.db3");
        }

        [Fact]
        public async Task H11_ElPanelMuestraContactosYRespetaOrdenDeRiesgo()
        {
            // ==========================================
            // 1. ARRANGE (Preparar el escenario real)
            // ==========================================

            var db = new Database(_rutaDbTemporal);

            // Insertamos datos usando tus propios métodos
            await db.InsertAsync(new ComisionRegistro { Id = 1, Nombre = "1°A" });

            await db.InsertAsync(new EstudianteRegistro { Id = 1, ComisionId = 1, Apellido = "Álvarez", Nombre = "Marina", Legajo = "111" });
            await db.InsertAsync(new EstudianteRegistro { Id = 2, ComisionId = 1, Apellido = "Ibarra", Nombre = "Lucía", Legajo = "222" });
            await db.InsertAsync(new EstudianteRegistro { Id = 3, ComisionId = 1, Apellido = "Domínguez", Nombre = "Tomás", Legajo = "333" });

            var fechaAyer = DateTime.Today.AddDays(-1);
            await db.InsertAsync(new ContactoRegistro { EstudianteId = 1, Fecha = fechaAyer, Tipo = "Llamada", Resultado = "Compromiso" });

            var fechaHace2Dias = DateTime.Today.AddDays(-2);
            await db.InsertAsync(new ContactoRegistro { EstudianteId = 3, Fecha = fechaHace2Dias, Tipo = "Llamada", Resultado = "Sin respuesta" });

            // Instanciamos repositorios REALES
            var asistenciaRepo = new AsistenciaRepositorioSQL(db);
            var estudianteRepo = new EstudianteRepositorioSQL(db);
            var entregaRepo = new EntregaRepositorioSQL(db);
            var configRepo = new ConfiguracionRiesgoRepositorioSQL(db);

            // Instanciamos servicios
            var calculadora = new CalculadoraRiesgo();
            var configuracionRiesgo = new ConfiguracionRiesgoService(configRepo);
            var riesgoService = new RiesgoService(asistenciaRepo, estudianteRepo, entregaRepo, calculadora, configuracionRiesgo);

            // Instanciamos la sesión con la propiedad correcta (ComisionActualId)
            var sesion = new SesionActual { ComisionActualId = 1 };

            // Instanciamos tu ViewModel REAL
            var viewModel = new RiesgoViewModel(sesion, riesgoService, db);

            // ==========================================
            // 2. ACT (Ejecutar la acción)
            // ==========================================
            await viewModel.RecalcularAsync();

            // ==========================================
            // 3. ASSERT (Verificar criterios)
            // ==========================================
            var itemMarina = viewModel.Items.FirstOrDefault(i => i.EstudianteId == 1);
            var itemIbarra = viewModel.Items.FirstOrDefault(i => i.EstudianteId == 2);
            var itemDominguez = viewModel.Items.FirstOrDefault(i => i.EstudianteId == 3);

            // CA1: Marina debe mostrar la fecha de ayer
            Assert.NotNull(itemMarina);
            Assert.Contains(fechaAyer.ToString("dd/MM/yyyy"), itemMarina.ContactoTexto);

            // CA2: Ibarra debe mostrar "Sin contactar"
            Assert.NotNull(itemIbarra);
            Assert.Equal("Sin contactar", itemIbarra.ContactoTexto);

            // CA3: Distinguir entre reciente y viejo
            Assert.NotNull(itemDominguez);
            Assert.Contains(fechaHace2Dias.ToString("dd/MM/yyyy"), itemDominguez.ContactoTexto);

            // CA4: El orden debe respetar el riesgo
            var prioridades = viewModel.Items.Select(i => i.Prioridad).ToList();
            var prioridadesOrdenadas = prioridades.OrderBy(p => p).ToList();
            Assert.Equal(prioridadesOrdenadas, prioridades);
        }

        public void Dispose()
        {
            if (File.Exists(_rutaDbTemporal))
            {
                try { File.Delete(_rutaDbTemporal); } catch { }
            }
        }
    }
}