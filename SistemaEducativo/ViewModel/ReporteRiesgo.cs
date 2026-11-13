using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.ViewModel
{
    public class ReporteRiesgo
    {
        public int TotalEstudiantes { get; }
        public int EstudiantesEnRiesgo { get; }
        public bool HayDatos { get; }

        public ReporteRiesgo(int total, int enRiesgo, bool hayDatos)
        {
            this.TotalEstudiantes = total;
            this.EstudiantesEnRiesgo = enRiesgo;
            this.HayDatos = hayDatos;
        }
    }
}
