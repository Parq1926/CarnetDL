using SRV13_Fotografia.Entities;

namespace SRV13_Fotografia.Services
{
    public interface IFotografiaService
    {
        // null = el usuario no tiene fotografia
        Task<string?> ObtenerFotografiaAsync(int usuarioId);

        // -1 usuario no existe | -2 imagen invalida (ver mensaje) | 0 no se aplico | 1 correcto
        Task<(int Resultado, string? Mensaje, FotografiaUsuario? Fotografia)> ActualizarFotografiaAsync(
            int usuarioId, string fotografiaBase64);

        // -1 usuario no existe | 0 sin fotografia | 1 correcto
        Task<(int Resultado, FotografiaUsuario? Fotografia)> EliminarFotografiaAsync(int usuarioId);
    }
}
