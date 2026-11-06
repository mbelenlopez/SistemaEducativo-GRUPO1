namespace SistemaEducativo.Entidades;

public class Entrega
{
    public int Id { get; set; }

    public int TrabajoId { get; set; }

    public int EstudianteId { get; set; }

    // Condición del dominio: CondicionEntrega.Entregado o CondicionEntrega.NoEntregado.
    public string Condicion { get; set; } = CondicionEntrega.NoEntregado;

    public bool Entregado => Condicion == CondicionEntrega.Entregado;
}