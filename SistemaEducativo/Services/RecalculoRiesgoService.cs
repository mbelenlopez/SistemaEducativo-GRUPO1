namespace SistemaEducativo.Services;

// Cuántos estudiantes quedaron en cada nivel después de recalcular.
public record ResumenRecalculo(int Estudiantes, int EnRiesgoAlto, int EnRiesgoBajo, int SinDatos);

// H7 (CA2): recalcula el riesgo de TODOS los estudiantes de TODAS las comisiones con los umbrales vigentes.
// El riesgo no se guarda en la base: se calcula siempre a partir de la asistencia y las entregas,
// así que ningún estudiante puede quedar con un valor "viejo". Esto además muestra el resultado.
public class RecalculoRiesgoService
{
    private readonly ComisionService _comisiones;
    private readonly RiesgoService _riesgo;

    public RecalculoRiesgoService(ComisionService comisiones, RiesgoService riesgo)
    {
        _comisiones = comisiones;
        _riesgo = riesgo;
    }

    public async Task<ResumenRecalculo> RecalcularTodosAsync()
    {
        int alto = 0, bajo = 0, sinDatos = 0;

        foreach (var comision in await _comisiones.ObtenerTodasAsync())
        {
            foreach (var item in await _riesgo.CalcularComisionAsync(comision.Id))
            {
                switch (item.Riesgo.Nivel)
                {
                    case NivelRiesgo.Alto: alto++; break;
                    case NivelRiesgo.Bajo: bajo++; break;
                    default: sinDatos++; break;
                }
            }
        }

        return new ResumenRecalculo(alto + bajo + sinDatos, alto, bajo, sinDatos);
    }
}
