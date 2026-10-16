using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.ViewModel
{
    public class EntregaItem
    {
        public EntregaItem(Estudiante estudiante, bool entregado, string estado)
        {
            Estudiante = estudiante;
            Entregado = entregado;
            Estado = estado;
        }
        public Estudiante Estudiante { get; }
        public string NombreCompleto => Estudiante.NombreCompleto;
        public bool Entregado { get; set; }
        public string Estado { get; }

    }
}
