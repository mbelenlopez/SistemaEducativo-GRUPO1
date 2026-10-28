using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public class ContactoRepositorioSQL : ContactoRepositorio
{
	private readonly Database _db;
	public ContactoRepositorioSQL(Database db) => _db = db;

	// El más reciente primero.
	public Task<List<Contacto>> ObtenerPorEstudianteAsync(int estudianteId) =>
		_db.QueryAsync<Contacto>(
			"SELECT * FROM Contactos WHERE EstudianteId = ? ORDER BY Fecha DESC, Id DESC",
			estudianteId);

	public Task AgregarAsync(Contacto contacto) => _db.InsertAsync(contacto);
}
