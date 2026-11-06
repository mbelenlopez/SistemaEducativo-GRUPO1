using SistemaEducativo.Entidades;
using SistemaEducativo.Repositorios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SistemaEducativo.Tests
{
    internal class ConfiguracionRiesgoRepositorioFake : ConfiguracionRiesgoRepositorio
    {
        private ConfiguracionRiesgo? config { get; set; }

        public Task GuardarAsync(ConfiguracionRiesgo configuracion)
        {
            config = new ConfiguracionRiesgo
            {
                Id = ConfiguracionRiesgo.IdGlobal,
                UmbralInasistencias = configuracion.UmbralInasistencias,
                UmbralNoEntregados = configuracion.UmbralNoEntregados
            };

            return Task.CompletedTask;
        }

        public Task<ConfiguracionRiesgo?> ObtenerAsync()
        {
            return Task.FromResult(config);

        }
    }
}
