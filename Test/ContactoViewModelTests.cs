using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Entidades;
using SistemaEducativo.Services;
using SistemaEducativo.ViewModel;

namespace SistemaEducativo.Tests;

// H10: lo que hace la pantalla de registrar contactos.
[TestClass]
public class ContactoViewModelTests
{
    private sealed class Escenario
    {
        public ContactoRepositorioFake Contactos { get; } = new();
        public EstudianteRepositorioFake Estudiantes { get; } = new();
        public ContactoViewModel ViewModel { get; }

        public Escenario() =>
            ViewModel = new ContactoViewModel(
                new SesionActual(), new ContactoService(Contactos), new EstudianteService(Estudiantes));

        public async Task<Estudiante> AgregarEstudianteAsync()
        {
            var estudiante = new Estudiante { ComisionId = 1, Apellido = "Alvarez", Nombre = "Marina", Legajo = "12345" };
            await Estudiantes.AgregarAsync(estudiante);
            return estudiante;
        }
    }

    [TestMethod]
    public async Task Guardar_SinEstudiante_PideElegirUno()
    {
        var escenario = new Escenario();
        var vm = escenario.ViewModel;
        vm.TipoSeleccionado = TiposContacto.Llamada;
        vm.Resultado = "No atendió";

        await vm.GuardarAsync();

        Assert.AreEqual("Elegí un estudiante.", vm.Mensaje);
        Assert.AreEqual(0, escenario.Contactos.Datos.Count);
    }

    [TestMethod]
    public async Task Guardar_ConDatosValidos_GuardaLimpiaElResultadoYMuestraElHistorial()
    {
        var escenario = new Escenario();
        var estudiante = await escenario.AgregarEstudianteAsync();
        var vm = escenario.ViewModel;
        vm.EstudianteSeleccionado = estudiante;
        vm.TipoSeleccionado = TiposContacto.Llamada;
        vm.Resultado = "  No atendió  ";

        await vm.GuardarAsync();

        Assert.AreEqual("Contacto registrado correctamente.", vm.Mensaje);
        Assert.AreEqual(string.Empty, vm.Resultado);
        Assert.AreEqual(1, vm.Historial.Count);
        Assert.AreEqual(TiposContacto.Llamada, vm.Historial[0].TipoTexto);
        Assert.AreEqual("No atendió", vm.Historial[0].Resultado);
        Assert.IsTrue(vm.HistorialTitulo.Contains("(1)"), vm.HistorialTitulo);
    }

    [TestMethod]
    public async Task Guardar_SinTipo_AvisaYConservaLoEscrito()
    {
        var escenario = new Escenario();
        var estudiante = await escenario.AgregarEstudianteAsync();
        var vm = escenario.ViewModel;
        vm.EstudianteSeleccionado = estudiante;
        vm.Resultado = "Lo que escribí";

        await vm.GuardarAsync();

        Assert.IsTrue(vm.Mensaje.Contains("llamada, mensaje o email"), vm.Mensaje);
        Assert.AreEqual("Lo que escribí", vm.Resultado);
        Assert.AreEqual(0, escenario.Contactos.Datos.Count);
    }

    [TestMethod]
    public async Task Guardar_SinResultado_Avisa()
    {
        var escenario = new Escenario();
        var estudiante = await escenario.AgregarEstudianteAsync();
        var vm = escenario.ViewModel;
        vm.EstudianteSeleccionado = estudiante;
        vm.TipoSeleccionado = TiposContacto.Email;
        vm.Resultado = "   ";

        await vm.GuardarAsync();

        Assert.IsTrue(vm.Mensaje.Contains("resultado"), vm.Mensaje);
        Assert.AreEqual(0, escenario.Contactos.Datos.Count);
    }
}
