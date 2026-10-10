using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

// Es la ÚNICA clase que abre el archivo de SQLite. Todos los repositorios la comparten.
public class Database
{
    private SQLiteAsyncConnection? _connection;

    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection != null)
            return _connection;   // ya estaba abierta: se reutiliza

        var path = Path.Combine(FileSystem.AppDataDirectory, "seguimiento.db3");
        _connection = new SQLiteAsyncConnection(path, storeDateTimeAsTicks: false);

        // Crea cada tabla SI NO EXISTE (si ya existe, no hace nada).
        await _connection.CreateTableAsync<Comision>();
        await _connection.CreateTableAsync<Estudiante>();
        await _connection.CreateTableAsync<Clase>();
        await _connection.CreateTableAsync<Asistencia>();
        await _connection.CreateTableAsync<Trabajo>();
        await _connection.CreateTableAsync<Entrega>();

        await SeedAsync(_connection);
        return _connection;
    }

    // Datos iniciales: si no hay comisiones, crea dos de ejemplo.
    private static async Task SeedAsync(SQLiteAsyncConnection db)
    {
        if (await db.Table<Comision>().CountAsync() == 0)
        {
            await db.InsertAllAsync(new[]
            {
                new Comision { Nombre = "Ingeniería 2026", CicloLectivo = 2026, Cuatrimestre = 1 },
                new Comision { Nombre = "Informática 2026", CicloLectivo = 2026, Cuatrimestre = 1 }
            });
        }
    }

    // Guarda un objeto como una fila nueva.
    public async Task<int> InsertAsync<T>(T item) where T : new()
    {
        var db = await GetConnectionAsync();
        return await db.InsertAsync(item);
    }

    // Ejecuta un SELECT y devuelve las filas como objetos. Los "?" se reemplazan por los valores de args.
    public async Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new()
    {
        var db = await GetConnectionAsync();
        return await db.QueryAsync<T>(query, args);
    }
}
