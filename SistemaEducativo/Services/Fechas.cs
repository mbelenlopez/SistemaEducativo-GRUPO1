namespace SistemaEducativo.Services;

// Reglas comunes de las fechas del sistema. Las entidades guardan DateTime;
// Database las escribe en SQLite como texto ISO (ver storeDateTimeAsTicks).
public static class Fechas
{
    // Una fecha sin completar llega como DateTime.MinValue (default).
    public static bool EsValida(DateTime fecha) => fecha != default;

    // Las fechas del sistema son DÍAS: se descarta la hora para que una misma fecha
    // sea siempre igual a sí misma (por ejemplo, al buscar si una clase ya existe).
    public static DateTime SoloFecha(DateTime fecha) => fecha.Date;
}
