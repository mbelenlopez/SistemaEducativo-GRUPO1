
namespace SistemaEducativo.ViewModel
{
    public class PanelItem
    {
        public string NombreCompleto { get; set; } = string.Empty;
        public string InasistenciasTexto { get; set; } = string.Empty;
        public string EntregaTexto { get; set; } = string.Empty;
        public string Nivel { get; set; } = string.Empty;
        public string NivelTexto { get; set; } = string.Empty;
        public double Puntaje { get; set; }

        public int Prioridad => Nivel == "Alto" ? 0 : Nivel == "Bajo" ? 1 : 2;
    }
}
