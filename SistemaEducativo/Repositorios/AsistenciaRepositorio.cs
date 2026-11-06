using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Repositorios
{
    public interface AsistenciaRepositorio
    {
        Task<List<Asistencia>> ObtenerPorClaseAsync(int claseId);
        Task<List<Asistencia>> ObtenerPorEstudianteAsync(int estudianteId);
        Task AgregarAsync(Asistencia asistencia);
        Task CorregirAsistenciaAsync(Asistencia asistencia);
    }
}
