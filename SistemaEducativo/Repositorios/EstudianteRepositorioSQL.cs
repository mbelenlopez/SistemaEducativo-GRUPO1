using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public class EstudianteRepositorioSQL : EstudianteRepositorio
{
    private readonly Database _db;
    public EstudianteRepositorioSQL(Database db) => _db = db;

    public async Task<List<Estudiante>> ObtenerPorComisionAsync(int comisionId)
    {
        var filas = await _db.QueryAsync<EstudianteRegistro>(
            "SELECT * FROM Estudiantes WHERE ComisionId = ? ORDER BY Apellido, Nombre",
            comisionId);
        return filas.Select(f => f.ToEntidad()).ToList();
    }

    public async Task<Estudiante?> ObtenerPorLegajoAsync(string legajo)
    {
        var lista = await _db.QueryAsync<EstudianteRegistro>(
            "SELECT * FROM Estudiantes WHERE Legajo = ?",
            legajo);
        return lista.FirstOrDefault()?.ToEntidad();
    }

    public async Task<Estudiante?> ObtenerPorIdAsync(int id)
    {
        var lista = await _db.QueryAsync<EstudianteRegistro>(
            "SELECT * FROM Estudiantes WHERE Id = ?",
            id);
        return lista.FirstOrDefault()?.ToEntidad();
    }

    public async Task AgregarAsync(Estudiante estudiante)
    {
        var registro = EstudianteRegistro.Desde(estudiante);
        await _db.InsertAsync(registro);
        estudiante.Id = registro.Id;   // el Id nuevo lo genera la base: se le devuelve a la entidad
    }
}
