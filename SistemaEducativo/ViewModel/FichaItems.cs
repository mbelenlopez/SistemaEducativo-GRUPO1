namespace SistemaEducativo.ViewModel;

// Las filas que muestra la ficha. Ya vienen con el texto listo para pantalla.
public sealed record AsistenciaFila(string FechaTexto, string EstadoTexto);

public sealed record EntregaFila(string Trabajo, string FechaLimiteTexto, string EstadoTexto);

public sealed record ContactoFila(string FechaTexto, string Resultado);
