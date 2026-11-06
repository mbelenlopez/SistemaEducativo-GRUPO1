
namespace SistemaEducativo.ViewModel
{
    public class PanelItem
    {
        public int EstudianteId { get; set; }   // H9: para abrir la ficha del estudiante
        public string NombreCompleto { get; set; } = string.Empty;
        public string InasistenciasTexto { get; set; } = string.Empty;
        public string EntregaTexto { get; set; } = string.Empty;
        public string NivelTexto { get; set; } = string.Empty;
    }
}
