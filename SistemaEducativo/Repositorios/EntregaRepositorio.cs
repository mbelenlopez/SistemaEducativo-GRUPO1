using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Repositorios
{
    public interface EntregaRepositorio
    {
        Task<List<Entrega>> ObtenerPorTrabajoAsync(int trabajoId);
        Task<List<Entrega>> ObtenerPorEstudianteAsync(int estudianteId);
        Task AgregarAsync(Entrega entrega);
    }
}
