using SQLite;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SistemaEducativo.Entidades;

public class Clase
{
    public int Id { get; set; }

    public int ComisionId { get; set; }

    public DateTime Fecha { get; set; }

    public string FechaFormateada => Fecha.ToString("dd/MM/yyyy");
}
