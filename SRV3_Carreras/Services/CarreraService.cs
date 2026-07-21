using SRV3_Carreras.Entities;
using SRV3_Carreras.Repository;
using System.Text.RegularExpressions;

namespace SRV3_Carreras.Services;

public class CarreraService : ICarreraService
{
    private readonly ICarreraRepository _repository;

    public CarreraService(ICarreraRepository repository)
    {
        _repository = repository;
    }

    private bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email)) return false;

        email = email.Trim().ToLower();

        bool esValido = email.EndsWith("@cuc.ac.cr") || email.EndsWith("@cuc.cr");

        if (esValido)
        {
            if (email.EndsWith("@cuc.ac.cr"))
            {
                var localPart = email.Substring(0, email.Length - "@cuc.ac.cr".Length);
                return !string.IsNullOrWhiteSpace(localPart) && !localPart.Contains(" ");
            }

            if (email.EndsWith("@cuc.cr"))
            {
                var localPart = email.Substring(0, email.Length - "@cuc.cr".Length);
                return !string.IsNullOrWhiteSpace(localPart) && !localPart.Contains(" ");
            }
        }

        return false;
    }

    private bool IsValidPhone(string telefono)
    {
        return !string.IsNullOrWhiteSpace(telefono) && Regex.IsMatch(telefono, @"^\d+$");
    }

    public async Task<IEnumerable<Carrera>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Carrera?> GetById(int id)
    {
        if (id <= 0) return null;
        return await _repository.GetById(id);
    }

    public async Task<(bool success, string message, int? id)> Create(CreateCarreraRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido", null);

        if (string.IsNullOrWhiteSpace(request.Director))
            return (false, "El director es requerido", null);

        if (string.IsNullOrWhiteSpace(request.Email))
            return (false, "El email es requerido", null);

        if (!IsValidEmail(request.Email))
            return (false, "El email no es valido. Solo se permiten correos @cuc.ac.cr o @cuc.cr", null);

        if (string.IsNullOrWhiteSpace(request.Telefono) || !IsValidPhone(request.Telefono))
            return (false, "El telefono solo debe contener numeros", null);

        if (request.InstitucionID <= 0)
            return (false, "La institucion es requerida", null);

        if (string.IsNullOrWhiteSpace(request.InstitucionNombre))
            return (false, "El nombre de la institucion es requerido", null);

        if (await _repository.ExistsByNombre(request.Nombre))
            return (false, $"Ya existe una carrera con el nombre '{request.Nombre}'", null);

        var carrera = new Carrera
        {
            Nombre = request.Nombre.Trim(),
            Director = request.Director.Trim(),
            Email = request.Email.Trim().ToLower(),
            Telefono = request.Telefono.Trim(),
            InstitucionID = request.InstitucionID,
            InstitucionNombre = request.InstitucionNombre.Trim()
        };

        var id = await _repository.Create(carrera);
        return (true, "Carrera creada exitosamente", id);
    }

    public async Task<(bool success, string message)> Update(UpdateCarreraRequest request)
    {
        if (request.ID <= 0)
            return (false, "ID invalido");

        var existing = await _repository.GetById(request.ID);
        if (existing == null)
            return (false, "Carrera no encontrada");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido");

        if (string.IsNullOrWhiteSpace(request.Director))
            return (false, "El director es requerido");

        if (string.IsNullOrWhiteSpace(request.Email))
            return (false, "El email es requerido");

        if (!IsValidEmail(request.Email))
            return (false, "El email no es valido. Solo se permiten correos @cuc.ac.cr o @cuc.cr");

        if (string.IsNullOrWhiteSpace(request.Telefono) || !IsValidPhone(request.Telefono))
            return (false, "El telefono solo debe contener numeros");

        if (request.InstitucionID <= 0)
            return (false, "La institucion es requerida");

        if (string.IsNullOrWhiteSpace(request.InstitucionNombre))
            return (false, "El nombre de la institucion es requerido");

        if (await _repository.ExistsByNombre(request.Nombre, request.ID))
            return (false, $"Ya existe otra carrera con el nombre '{request.Nombre}'");

        existing.Nombre = request.Nombre.Trim();
        existing.Director = request.Director.Trim();
        existing.Email = request.Email.Trim().ToLower();
        existing.Telefono = request.Telefono.Trim();
        existing.InstitucionID = request.InstitucionID;
        existing.InstitucionNombre = request.InstitucionNombre.Trim();

        var updated = await _repository.Update(existing);
        return updated ? (true, "Carrera actualizada exitosamente") : (false, "No se pudo actualizar la carrera");
    }

    public async Task<(bool success, string message)> Delete(int id)
    {
        if (id <= 0)
            return (false, "ID invalido");

        var existing = await _repository.GetById(id);
        if (existing == null)
            return (false, "Carrera no encontrada");

        var deleted = await _repository.Delete(id);
        return deleted ? (true, "Carrera eliminada exitosamente") : (false, "No se pudo eliminar la carrera");
    }
}