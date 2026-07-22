using SRV2_Instituciones.Entities;

namespace SRV2_Instituciones.Services;

public interface IInstitucionService
{
    Task<IEnumerable<Institucion>> GetAll();
    Task<Institucion?> GetById(int id);
    Task<(bool success, string message, int? id)> Create(CreateInstitucionRequest request);
    Task<(bool success, string message)> Update(UpdateInstitucionRequest request);
    Task<(bool success, string message)> Delete(int id);
}

public class CreateInstitucionRequest
{
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Dominios { get; set; } = string.Empty;
}

public class UpdateInstitucionRequest
{
    public int ID { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefono { get; set; } = string.Empty;
    public string Dominios { get; set; } = string.Empty;
}