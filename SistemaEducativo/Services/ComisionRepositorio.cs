using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services;

// Puerto (interfaz): define QUÉ datos necesita el sistema sobre comisiones,
// sin decir CÓMO se guardan. La implementación real está en Repositorios/ComisionRepositorioSQL.cs.
public interface ComisionRepositorio
{
    Task<List<Comision>> ObtenerTodasAsync();
}
