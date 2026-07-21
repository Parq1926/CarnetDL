using SRV13_Fotografia.Entities;

namespace SRV13_Fotografia.Services
{
    public interface IFotografiaService
    {
        Task<FotografiaUsuario?> ObtenerFotografiaAsync(string identificacion);
        Task<int> ActualizarFotografiaAsync(string identificacion, string fotografiaBase64);
        Task<int> EliminarFotografiaAsync(string identificacion);
    }
}
