using RolSRV8.Entities;
using RolSRV8.Repository;

namespace RolSRV8.Services;

public class RolService : IRolService
{
    private readonly RolRepository _repository;

    public RolService(RolRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<Rol>> ObtenerTodosAsync()
    {
        return await _repository.ObtenerTodosAsync();
    }

    public async Task<Rol?> ObtenerPorIdAsync(int id)
    {
        return await _repository.ObtenerPorIdAsync(id);
    }

    public async Task<(bool ok, string error)> CrearAsync(
        RolRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido");

        if (string.IsNullOrWhiteSpace(request.Pantallas))
            return (false, "Las pantallas son requeridas");

        var rol = new Rol
        {
            Nombre = request.Nombre,
            Pantallas = request.Pantallas
        };

        await _repository.CrearAsync(rol);

        return (true, string.Empty);
    }

    public async Task<(bool ok, string error)> ActualizarAsync(
        int id,
        RolRequest request)
    {
        var rol = await _repository.ObtenerPorIdAsync(id);

        if (rol == null)
            return (false, "Rol no encontrado");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido");

        if (string.IsNullOrWhiteSpace(request.Pantallas))
            return (false, "Las pantallas son requeridas");

        rol.Nombre = request.Nombre;
        rol.Pantallas = request.Pantallas;

        await _repository.ActualizarAsync(rol);

        return (true, string.Empty);
    }

    public async Task<(bool ok, string error)> EliminarAsync(int id)
    {
        var rol =
            await _repository.ObtenerPorIdAsync(id);

        if (rol == null)
        {
            return (
                false,
                "Rol no encontrado");
        }

        var cantidad =
            await _repository.ContarUsuariosAsync(id);

        if (cantidad > 0)
        {
            return (
                false,
                $"No se puede eliminar el rol porque está asignado a {cantidad} usuario(s)");
        }

        await _repository.EliminarAsync(id);

        return (true, string.Empty);
    }

    public async Task<int> ContarUsuariosAsync(int id)
    {
        return await _repository.ContarUsuariosAsync(id);
    }
}