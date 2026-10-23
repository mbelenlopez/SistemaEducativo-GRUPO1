using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public class EstudianteRepositorioSQL : EstudianteRepositorio
{
    private readonly Database _db;
    public EstudianteRepositorioSQL(Database db) => _db = db;

    public Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId) =>
        _db.QueryAsync<Estudiante>(
            "SELECT * FROM Estudiantes WHERE ComisionId = ? ORDER BY Apellido, Nombre",
            comisionId);

    public async Task<Estudiante?> ObtenerPorLegajoAsync(string legajo)
    {
        var lista = await _db.QueryAsync<Estudiante>(
            "SELECT * FROM Estudiantes WHERE Legajo = ?",
            legajo);
        return lista.FirstOrDefault();
    }

    public Task AgregarAsync(Estudiante estudiante) => _db.InsertAsync(estudiante);
}