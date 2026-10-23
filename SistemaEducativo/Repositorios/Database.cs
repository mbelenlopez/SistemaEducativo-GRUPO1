using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

// Es la ÚNICA clase que abre el archivo de SQLite. Todos los repositorios la comparten.
public class Database
{
    // Máximo de valores "?" por consulta (SQLite tiene un tope de variables por consulta).
    private const int MaximoIdsPorConsulta = 500;

    private readonly string? _rutaArchivo;
    private readonly SemaphoreSlim _inicializacion = new(1, 1);
    private SQLiteAsyncConnection? _connection;

    // rutaArchivo solo se usa en las pruebas (una base temporal). En la app queda en null.
    public Database(string? rutaArchivo = null)
    {
        _rutaArchivo = rutaArchivo;
    }

    private async Task<SQLiteAsyncConnection> GetConnectionAsync()
    {
        if (_connection != null)
            return _connection;   // ya estaba lista: se reutiliza

        // Si dos llamadas llegan a la vez en la primera vez, solo una inicializa; la otra espera.
        await _inicializacion.WaitAsync();
        try
        {
            if (_connection != null)
                return _connection;   // otra llamada la inicializó mientras esperábamos

            var path = _rutaArchivo ?? Path.Combine(FileSystem.AppDataDirectory, "seguimiento.db3");

            // storeDateTimeAsTicks: false -> los DateTime se guardan como TEXTO en formato ISO (año-mes-día
            // y hora), no como un número. Se leen igual y ORDER BY los ordena cronológicamente.
            var conexion = new SQLiteAsyncConnection(path, storeDateTimeAsTicks: false);

            // Crea cada tabla SI NO EXISTE (si ya existe, no hace nada).
            await conexion.CreateTableAsync<Comision>();
            await conexion.CreateTableAsync<Estudiante>();
            await conexion.CreateTableAsync<Clase>();
            await conexion.CreateTableAsync<Asistencia>();
            await conexion.CreateTableAsync<Trabajo>();
            await conexion.CreateTableAsync<Entrega>();

            await SeedAsync(conexion);

            // Recién ahora se publica: nadie puede recibir una conexión sin tablas o sin datos iniciales.
            _connection = conexion;
            return conexion;
        }
        finally
        {
            _inicializacion.Release();
        }
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

    public async Task<int> UpdateAsync<T>(T item) where T : new()
    {
        var db = await GetConnectionAsync();
        return await db.UpdateAsync(item);
    }

    // Ejecuta un SELECT y devuelve las filas como objetos. Los "?" se reemplazan por los valores de args.
    public async Task<List<T>> QueryAsync<T>(string query, params object[] args) where T : new()
    {
        var db = await GetConnectionAsync();
        return await db.QueryAsync<T>(query, args);
    }

    // SELECT * FROM tabla WHERE columna IN (id1, id2, ...): una sola consulta para muchos ids
    // (en lugar de una consulta por cada id). Los ids viajan como parámetros "?".
    // "tabla" y "columna" son nombres fijos escritos en el código de los repositorios, nunca texto del usuario.
    // Lo usa el panel de riesgo (H6/H8); se incluye ahora para que esa rama no tenga que tocar este archivo.
    public async Task<List<T>>  QueryPorIdsAsync<T>(string tabla, string columna, IReadOnlyCollection<int> ids) where T : new()
    {
        var resultado = new List<T>();

        foreach (var grupo in ids.Chunk(MaximoIdsPorConsulta))
        {
            var marcadores = string.Join(",", grupo.Select(_ => "?"));
            var filas = await QueryAsync<T>(
                $"SELECT * FROM {tabla} WHERE {columna} IN ({marcadores})",
                grupo.Cast<object>().ToArray());
            resultado.AddRange(filas);
        }

        return resultado;
    }
}
