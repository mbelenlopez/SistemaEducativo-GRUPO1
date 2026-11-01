using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

public class ContactoRepositorioSQL : ContactoRepositorio
{
    private readonly Database _db;
    public ContactoRepositorioSQL(Database db) => _db = db;

    // El más reciente primero.
    public async Task<List<Contacto>> ObtenerPorEstudianteAsync(int estudianteId)
    {
        var filas = await _db.QueryAsync<ContactoRegistro>(
            "SELECT * FROM Contactos WHERE EstudianteId = ? ORDER BY Fecha DESC, Id DESC",
            estudianteId);
        return filas.Select(f => f.ToEntidad()).ToList();
    }

    public async Task AgregarAsync(Contacto contacto)
    {
        var registro = ContactoRegistro.Desde(contacto);
        await _db.InsertAsync(registro);
        contacto.Id = registro.Id;
    }
}
