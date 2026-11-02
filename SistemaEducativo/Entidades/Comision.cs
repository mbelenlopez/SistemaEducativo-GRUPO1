namespace SistemaEducativo.Entidades;

public class Comision
{
    public int Id { get; set; }

    public string Nombre { get; set; } = string.Empty;

    public int CicloLectivo { get; set; }

    public int Cuatrimestre { get; set; }
}
