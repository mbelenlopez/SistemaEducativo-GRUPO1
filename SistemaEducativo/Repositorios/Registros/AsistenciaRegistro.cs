using SQLite;
using SistemaEducativo.Entidades;

namespace SistemaEducativo.Repositorios;

[Table("Asistencias")]
public class AsistenciaRegistro
{
	[PrimaryKey, AutoIncrement]
	public int Id { get; set; }

	[Indexed, NotNull]
	public int ClaseId { get; set; }

	[Indexed, NotNull]
	public int EstudianteId { get; set; }

	[NotNull]
	public string Condicion { get; set; } = CondicionAsistencia.Ausente;

	public Asistencia ToEntidad() => new()
	{
		Id = Id,
		ClaseId = ClaseId,
		EstudianteId = EstudianteId,
		Condicion = Condicion
	};

	public static AsistenciaRegistro Desde(Asistencia a) => new()
	{
		Id = a.Id,
		ClaseId = a.ClaseId,
		EstudianteId = a.EstudianteId,
		Condicion = a.Condicion
	};
}
