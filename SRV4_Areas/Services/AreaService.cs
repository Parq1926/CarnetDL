using AreasSRV7.DTOs;
using SRV4_Areas.DTOs;
using SRV4_Areas.Entities;
using SRV4_Areas.Repository;

namespace SRV4_Areas.Services;

public class AreaService : IAreaService
{
    private readonly IAreaRepository _repository;

    public AreaService(IAreaRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<AreaDto>> GetAll()
    {
        var areas = await _repository.GetAll();
        return areas.Select(a => new AreaDto
        {
            Id = a.ID,
            Nombre = a.Nombre,
            InstitucionId = a.InstitucionID,
            InstitucionNombre = a.InstitucionNombre,
            Activo = a.Activo,
            FechaCreacion = a.FechaCreacion,
            FechaModificacion = a.FechaModificacion
        });
    }

    public async Task<AreaDto?> GetById(int id)
    {
        if (id <= 0) return null;
        var area = await _repository.GetById(id);
        if (area == null) return null;

        return new AreaDto
        {
            Id = area.ID,
            Nombre = area.Nombre,
            InstitucionId = area.InstitucionID,
            InstitucionNombre = area.InstitucionNombre,
            Activo = area.Activo,
            FechaCreacion = area.FechaCreacion,
            FechaModificacion = area.FechaModificacion
        };
    }

    public async Task<(bool success, string message, int? id)> Create(CreateAreaRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido", null);

        if (request.InstitucionID <= 0)
            return (false, "La institucion es requerida", null);

        if (string.IsNullOrWhiteSpace(request.InstitucionNombre))
            return (false, "El nombre de la institucion es requerido", null);

        if (await _repository.ExistsByNombre(request.Nombre))
            return (false, $"Ya existe un area con el nombre '{request.Nombre}'", null);

        var area = new AreaTrabajo
        {
            Nombre = request.Nombre.Trim(),
            InstitucionID = request.InstitucionID,
            InstitucionNombre = request.InstitucionNombre.Trim()
        };

        var id = await _repository.Create(area);
        return (true, "Area creada exitosamente", id);
    }

    public async Task<(bool success, string message)> Update(UpdateAreaRequest request)
    {
        if (request.ID <= 0)
            return (false, "ID invalido");

        var existing = await _repository.GetById(request.ID);
        if (existing == null)
            return (false, "Area no encontrada");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido");

        if (request.InstitucionID <= 0)
            return (false, "La institucion es requerida");

        if (string.IsNullOrWhiteSpace(request.InstitucionNombre))
            return (false, "El nombre de la institucion es requerido");

        if (await _repository.ExistsByNombre(request.Nombre, request.ID))
            return (false, $"Ya existe otra area con el nombre '{request.Nombre}'");

        existing.Nombre = request.Nombre.Trim();
        existing.InstitucionID = request.InstitucionID;
        existing.InstitucionNombre = request.InstitucionNombre.Trim();

        var updated = await _repository.Update(existing);
        return updated ? (true, "Area actualizada exitosamente") : (false, "No se pudo actualizar el area");
    }

    public async Task<(bool success, string message)> Delete(int id)
    {
        if (id <= 0)
            return (false, "ID invalido");

        var existing = await _repository.GetById(id);
        if (existing == null)
            return (false, "Area no encontrada");

        var deleted = await _repository.Delete(id);
        return deleted ? (true, "Area eliminada exitosamente") : (false, "No se pudo eliminar el area");
    }
}