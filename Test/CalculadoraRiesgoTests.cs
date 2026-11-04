using Microsoft.VisualStudio.TestTools.UnitTesting;
using SistemaEducativo.Services;

namespace SistemaEducativo.Tests;

// H6: las reglas del cálculo de riesgo.
[TestClass]
public class CalculadoraRiesgoTests
{
    private static ResultadoRiesgo Calcular(
        int clases, int ausencias, int trabajos, int noEntregados,
        double umbralInasistencias = 50, double umbralNoEntregados = 50) =>
        new CalculadoraRiesgo().Calcular(clases, ausencias, trabajos, noEntregados, umbralInasistencias, umbralNoEntregados);

    [TestMethod]
    public void SinClasesNiTrabajos_EsSinDatos()
    {
        var resultado = Calcular(0, 0, 0, 0);

        Assert.AreEqual(NivelRiesgo.SinDatos, resultado.Nivel);
        Assert.AreEqual(0.0, resultado.PorcentajeInasistencias, 0.001);
        Assert.AreEqual(0.0, resultado.PorcentajeNoEntregados, 0.001);
    }

    // El error que vio el profe: asistencia sin trabajos mostraba "No entregas: NaN%".
    [TestMethod]
    public void ConAsistenciaYSinTrabajos_LosPorcentajesNoSonNaN()
    {
        var resultado = Calcular(clases: 10, ausencias: 1, trabajos: 0, noEntregados: 0);

        Assert.IsFalse(double.IsNaN(resultado.PorcentajeNoEntregados));
        Assert.IsFalse(double.IsNaN(resultado.PorcentajeInasistencias));
        Assert.AreEqual(0.0, resultado.PorcentajeNoEntregados, 0.001);
        Assert.AreEqual(10.0, resultado.PorcentajeInasistencias, 0.001);
        Assert.AreEqual(NivelRiesgo.Bajo, resultado.Nivel);
    }

    [TestMethod]
    public void ConAsistenciaYSinTrabajos_ConMuchasAusencias_EsAlto()
    {
        var resultado = Calcular(clases: 10, ausencias: 8, trabajos: 0, noEntregados: 0);

        Assert.AreEqual(NivelRiesgo.Alto, resultado.Nivel);
    }

    [TestMethod]
    public void ConTrabajosYSinClases_LosPorcentajesNoSonNaN()
    {
        var bajo = Calcular(clases: 0, ausencias: 0, trabajos: 4, noEntregados: 1);
        var alto = Calcular(clases: 0, ausencias: 0, trabajos: 4, noEntregados: 3);

        Assert.IsFalse(double.IsNaN(bajo.PorcentajeInasistencias));
        Assert.AreEqual(25.0, bajo.PorcentajeNoEntregados, 0.001);
        Assert.AreEqual(NivelRiesgo.Bajo, bajo.Nivel);
        Assert.AreEqual(NivelRiesgo.Alto, alto.Nivel);
    }

    // Llegar justo al umbral ya es riesgo (se compara con "mayor o igual").
    [TestMethod]
    public void JustoEnElUmbral_EsAlto_YPorDebajoEsBajo()
    {
        Assert.AreEqual(NivelRiesgo.Alto, Calcular(10, 5, 2, 0).Nivel);   // 50% >= 50%
        Assert.AreEqual(NivelRiesgo.Bajo, Calcular(10, 4, 2, 0).Nivel);   // 40% < 50%
    }

    [TestMethod]
    public void AltoSoloPorEntregas_YAltoSoloPorAsistencia()
    {
        Assert.AreEqual(NivelRiesgo.Alto, Calcular(clases: 10, ausencias: 0, trabajos: 4, noEntregados: 2).Nivel);
        Assert.AreEqual(NivelRiesgo.Alto, Calcular(clases: 10, ausencias: 6, trabajos: 4, noEntregados: 0).Nivel);
    }

    [TestMethod]
    public void CadaSenalUsaSuPropioUmbral()
    {
        // 40% de inasistencias y 50% de no entregados.
        Assert.AreEqual(NivelRiesgo.Alto, Calcular(10, 4, 4, 2, umbralInasistencias: 40, umbralNoEntregados: 60).Nivel);
        Assert.AreEqual(NivelRiesgo.Bajo, Calcular(10, 3, 4, 2, umbralInasistencias: 40, umbralNoEntregados: 60).Nivel);
    }

    [TestMethod]
    public void UnaSenalSinDatos_NoDisparaElRiesgoNiConUmbralCero()
    {
        var resultado = Calcular(clases: 10, ausencias: 0, trabajos: 0, noEntregados: 0, umbralInasistencias: 50, umbralNoEntregados: 0);

        Assert.AreEqual(NivelRiesgo.Bajo, resultado.Nivel);
    }

    [TestMethod]
    public void LosPorcentajesSeRedondeanADosDecimales()
    {
        var resultado = Calcular(clases: 3, ausencias: 1, trabajos: 3, noEntregados: 2);

        Assert.AreEqual(33.33, resultado.PorcentajeInasistencias, 0.0001);
        Assert.AreEqual(66.67, resultado.PorcentajeNoEntregados, 0.0001);
    }
}
