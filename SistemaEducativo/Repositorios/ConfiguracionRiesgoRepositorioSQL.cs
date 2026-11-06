using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public class ConfiguracionRiesgoRepositorioSQL : ConfiguracionRiesgoRepositorio
{
    private readonly Database _db;
    public ConfiguracionRiesgoRepositorioSQL(Database db) => _db = db;

    public async Task<ConfiguracionRiesgo?> ObtenerAsync()
    {
        var filas = await _db.QueryAsync<ConfiguracionRiesgoRegistro>(
            "SELECT * FROM ConfiguracionRiesgo WHERE Id = ?",
            ConfiguracionRiesgo.IdGlobal);
        return filas.FirstOrDefault()?.ToEntidad();
    }

    public async Task GuardarAsync(ConfiguracionRiesgo configuracion)
    {
        var registro = ConfiguracionRiesgoRegistro.Desde(configuracion);

        if (await ObtenerAsync() is null)
            await _db.InsertAsync(registro);
        else
            await _db.UpdateAsync(registro);
    }
}
