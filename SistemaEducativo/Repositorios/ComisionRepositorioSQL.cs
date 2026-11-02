using SistemaEducativo.Entidades;
using SistemaEducativo.Services;

namespace SistemaEducativo.Repositorios;

// Adaptador: implementa el puerto ComisionRepositorio usando SQLite.
public class ComisionRepositorioSQL : ComisionRepositorio
{
    private readonly Database _db;
    public ComisionRepositorioSQL(Database db) => _db = db;

    public Task<List<Comision>> ObtenerTodasAsync() =>
        _db.QueryAsync<Comision>("SELECT * FROM Comisiones ORDER BY Nombre");
}
