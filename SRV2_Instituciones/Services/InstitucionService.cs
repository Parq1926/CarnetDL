using SRV2_Instituciones.Entities;
using SRV2_Instituciones.Repository;
using System.Text.RegularExpressions;

namespace SRV2_Instituciones.Services;

public class InstitucionService : IInstitucionService
{
    private readonly IInstitucionRepository _repository;

    public InstitucionService(IInstitucionRepository repository)
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

    private bool IsValidDomain(string dominios)
    {
        if (string.IsNullOrWhiteSpace(dominios)) return false;

        var domains = dominios.Split(',');
        var allowedDomains = new[] { "cuc.ac.cr", "cuc.cr" };

        foreach (var domain in domains)
        {
            var domainTrimmed = domain.Trim().ToLower();

            bool isValid = false;
            foreach (var allowed in allowedDomains)
            {
                if (domainTrimmed == allowed)
                {
                    isValid = true;
                    break;
                }
            }

            if (!isValid)
                return false;
        }

        return true;
    }

    public async Task<IEnumerable<Institucion>> GetAll()
    {
        return await _repository.GetAll();
    }

    public async Task<Institucion?> GetById(int id)
    {
        if (id <= 0) return null;
        return await _repository.GetById(id);
    }

    public async Task<(bool success, string message, int? id)> Create(CreateInstitucionRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido", null);

        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            return (false, "El email no es valido. Solo se permiten correos @cuc.ac.cr o @cuc.cr", null);

        if (string.IsNullOrWhiteSpace(request.Telefono) || !IsValidPhone(request.Telefono))
            return (false, "El telefono solo debe contener numeros", null);

        if (string.IsNullOrWhiteSpace(request.Dominios) || !IsValidDomain(request.Dominios))
            return (false, "Los dominios no son validos. Solo se permiten: cuc.ac.cr o cuc.cr", null);

        if (await _repository.ExistsByNombre(request.Nombre))
            return (false, $"Ya existe una institucion con el nombre '{request.Nombre}'", null);

        var institucion = new Institucion
        {
            Nombre = request.Nombre.Trim(),
            Email = request.Email.Trim().ToLower(),
            Telefono = request.Telefono.Trim(),
            Dominios = request.Dominios.Trim().ToLower()
        };

        var id = await _repository.Create(institucion);
        return (true, "Institucion creada exitosamente", id);
    }

    public async Task<(bool success, string message)> Update(UpdateInstitucionRequest request)
    {
        if (request.ID <= 0)
            return (false, "ID invalido");

        var existing = await _repository.GetById(request.ID);
        if (existing == null)
            return (false, "Institucion no encontrada");

        if (string.IsNullOrWhiteSpace(request.Nombre))
            return (false, "El nombre es requerido");

        if (string.IsNullOrWhiteSpace(request.Email) || !IsValidEmail(request.Email))
            return (false, "El email no es valido. Solo se permiten correos @cuc.ac.cr o @cuc.cr");

        if (string.IsNullOrWhiteSpace(request.Telefono) || !IsValidPhone(request.Telefono))
            return (false, "El telefono solo debe contener numeros");

        if (string.IsNullOrWhiteSpace(request.Dominios) || !IsValidDomain(request.Dominios))
            return (false, "Los dominios no son validos. Solo se permiten: cuc.ac.cr o cuc.cr");

        if (await _repository.ExistsByNombre(request.Nombre, request.ID))
            return (false, $"Ya existe otra institucion con el nombre '{request.Nombre}'");

        existing.Nombre = request.Nombre.Trim();
        existing.Email = request.Email.Trim().ToLower();
        existing.Telefono = request.Telefono.Trim();
        existing.Dominios = request.Dominios.Trim().ToLower();

        var updated = await _repository.Update(existing);
        return updated ? (true, "Institucion actualizada exitosamente") : (false, "No se pudo actualizar la institucion");
    }

    public async Task<(bool success, string message)> Delete(int id)
    {
        if (id <= 0)
            return (false, "ID invalido");

        var existing = await _repository.GetById(id);
        if (existing == null)
            return (false, "Institucion no encontrada");

        var deleted = await _repository.Delete(id);
        return deleted ? (true, "Institucion eliminada exitosamente") : (false, "No se pudo eliminar la institucion");
    }
}