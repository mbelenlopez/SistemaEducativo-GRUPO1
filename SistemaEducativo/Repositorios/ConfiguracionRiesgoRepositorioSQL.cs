using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public class ConfiguracionRiesgoRepositorioSQL : ConfiguracionRiesgoRepositorio
{
    private readonly Database _db;
    public ConfiguracionRiesgoRepositorioSQL(Database db) => _db = db;

    public async Task<ConfiguracionRiesgo?> ObtenerAsync()
    {
        var filas = await _db.QueryAsync<ConfiguracionRiesgo>(
            "SELECT * FROM ConfiguracionRiesgo WHERE Id = ?",
            ConfiguracionRiesgo.IdGlobal);
        return filas.FirstOrDefault();
    }

    public async Task GuardarAsync(ConfiguracionRiesgo configuracion)
    {
        configuracion.Id = ConfiguracionRiesgo.IdGlobal;

        if (await ObtenerAsync() is null)
            await _db.InsertAsync(configuracion);
        else
            await _db.UpdateAsync(configuracion);
    }
}
