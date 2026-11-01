using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

// Adaptador: implementa el puerto ComisionRepositorio usando SQLite.
public class ComisionRepositorioSQL : ComisionRepositorio
{
    private readonly Database _db;
    public ComisionRepositorioSQL(Database db) => _db = db;

    public async Task<List<Comision>> ObtenerTodasAsync()
    {
        var filas = await _db.QueryAsync<ComisionRegistro>("SELECT * FROM Comisiones ORDER BY Nombre");
        return filas.Select(f => f.ToEntidad()).ToList();
    }
}
