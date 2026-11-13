using System;

namespace SistemaEducativo.ViewModel
{
    public class PanelItem
    {
        public int EstudianteId { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
        public string InasistenciasTexto { get; set; } = string.Empty;
        public string EntregaTexto { get; set; } = string.Empty;

        public string Nivel { get; set; } = string.Empty;
        public string NivelTexto { get; set; } = string.Empty;
        public double Puntaje { get; set; }

        // ---- NUEVO H11 (Tus propiedades) ----
        public DateTime? UltimaFechaContacto { get; set; }

        public string ContactoTexto
        {
            get
            {
                if (UltimaFechaContacto == null)
                    return "Sin contactar";
                return $"Contactado: {UltimaFechaContacto.Value:dd/MM/yyyy}";
            }
        }
        // -------------------------------------

        public int Prioridad => Nivel == "Alto" ? 0 : Nivel == "Bajo" ? 1 : 2;
    }
}