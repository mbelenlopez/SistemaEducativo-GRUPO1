using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services;

// Una línea del historial de asistencia: la fecha de la clase y si el estudiante estuvo presente.
public record DetalleAsistencia(DateTime Fecha, bool Presente);

// Una línea del historial de entregas: el trabajo y su condición ("Entregado" / "No entregado").
public record DetalleEntrega(string Trabajo, DateTime FechaLimite, string Condicion);

// Todo lo que muestra la ficha de un estudiante (H9). Es de solo lectura.
public record FichaEstudiante(
    Estudiante Estudiante,
    ResultadoRiesgo Riesgo,
    int ClasesRegistradas,
    int Ausencias,
    int TrabajosRegistrados,
    int NoEntregados,
    double UmbralInasistencias,
    double UmbralNoEntregados,
    IReadOnlyList<DetalleAsistencia> Asistencias,
    IReadOnlyList<DetalleEntrega> Entregas,
    IReadOnlyList<Contacto> Contactos)
{
    // Porcentajes "seguros": sin clases (o sin trabajos) dan 0 y no NaN (0 / 0).
    public double PorcentajeInasistencias =>
        ClasesRegistradas == 0 ? 0 : Math.Round(Ausencias * 100.0 / ClasesRegistradas, 2);

    public double PorcentajeNoEntregados =>
        TrabajosRegistrados == 0 ? 0 : Math.Round(NoEntregados * 100.0 / TrabajosRegistrados, 2);

    // Qué indicador(es) justifican el riesgo ALTO. Misma regla que CalculadoraRiesgo (>= umbral, sin redondear).
    public bool SuperaUmbralInasistencias =>
        ClasesRegistradas > 0 && Ausencias * 100.0 / ClasesRegistradas >= UmbralInasistencias;

    public bool SuperaUmbralNoEntregados =>
        TrabajosRegistrados > 0 && NoEntregados * 100.0 / TrabajosRegistrados >= UmbralNoEntregados;
}
