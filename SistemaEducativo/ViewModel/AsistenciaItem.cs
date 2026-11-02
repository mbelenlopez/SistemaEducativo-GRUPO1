
using SistemaEducativo.Entidades;

namespace SistemaEducativo.ViewModel
{
    public sealed class AsistenciaItem
    {
        public Estudiante Estudiante { get; }
        public string NombreCompleto => $"{Estudiante.Apellido}, {Estudiante.Nombre}";
        public bool Presente { get; set; }

        public AsistenciaItem(Estudiante estudiante, bool presente)
        {
            Estudiante = estudiante;
            Presente = presente;    
        }
    }
}
