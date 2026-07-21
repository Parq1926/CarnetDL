using RolSRV8.Entities;

namespace RolSRV8.Services;

public interface IRolService
{
    Task<IEnumerable<Rol>> ObtenerTodosAsync();

    Task<Rol?> ObtenerPorIdAsync(int id);

    Task<(bool ok, string error)> CrearAsync(
        RolRequest request);

    Task<(bool ok, string error)> ActualizarAsync(
        int id,
        RolRequest request);

    Task<(bool ok, string error)> EliminarAsync(
        int id);

    Task<int> ContarUsuariosAsync(int id); 
}