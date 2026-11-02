using SistemaEducativo.Entidades;

namespace SistemaEducativo.Services;

public class ComisionService
{
    private readonly ComisionRepositorio _repositorio;
    public ComisionService(ComisionRepositorio repositorio) => _repositorio = repositorio;

    public Task<List<Comision>> ObtenerTodasAsync() => _repositorio.ObtenerTodasAsync();
}
