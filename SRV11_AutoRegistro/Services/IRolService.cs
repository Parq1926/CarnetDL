using SRV11_AutoRegistro.Entities;

namespace SRV11_AutoRegistro.Services
{
    public interface IRolService
    {
        Task<Rol?> GetById(int id);

        Task<List<Rol>> GetAll();
    }
}