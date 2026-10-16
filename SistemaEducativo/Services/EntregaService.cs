using SistemaEducativo.Entidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Services
{
    public class EntregaService
    {
        private readonly EntregaRepositorio _entregas;
        private readonly EstudianteRepositorio _estudiantes;

        public EntregaService(EntregaRepositorio entragas, EstudianteRepositorio estudiantes)
        {
            _entregas = entragas;
            _estudiantes = estudiantes;
        }
        public Task<List<Entrega>> ObtenerPorTrabajoAsync(int trabajoId) =>
            _entregas.ObtenerPorTrabajoAsync(trabajoId);

        // H5: registra quién entregó un trabajo. Quien no está marcado queda como "No entregado".
        public async Task<(bool Exito, string Mensaje)> RegistrarEntregasAsync(int trabajoId, int comisionId, IEnumerable<int> entregaron)
        {
            if ((await ObtenerPorTrabajoAsync(trabajoId)).Count > 0)
                return (false, "Las entregas de ese trabajo ya fueron registradas.");

            var ids = entregaron.ToHashSet();
            var estudiantes = await _estudiantes.ObtenerPorComisionAsync(comisionId);
            if (estudiantes.Count == 0)
                return (false, "La comisión todavía no tiene estudiantes.");

            foreach (var estudiante in estudiantes)
            {
                await _entregas.AgregarAsync(new Entrega
                {
                    TrabajoId = trabajoId,
                    EstudianteId = estudiante.Id,
                    Condicion = ids.Contains(estudiante.Id) ? "Entregado" : "No entregado"
                });
            }
            return (true, " Entregas Regustradas correctamente");
        }
        
    }
}

