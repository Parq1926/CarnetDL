using SRV13_Fotografia.Entities;
using SRV13_Fotografia.Repository;

namespace SRV13_Fotografia.Services
{
    public class FotografiaService : IFotografiaService
    {
        // HU SRV13: la imagen no debe ser superior a 1 MB
        private const int MaxBytes = 1024 * 1024;

        private readonly FotografiaRepository _repository;

        public FotografiaService(FotografiaRepository repository) { _repository = repository; }

        public async Task<string?> ObtenerFotografiaAsync(int usuarioId)
        {
            var bytes = await _repository.ObtenerAsync(usuarioId);
            return bytes is null || bytes.Length == 0 ? null : Convert.ToBase64String(bytes);
        }

        public async Task<(int, string?, FotografiaUsuario?)> ActualizarFotografiaAsync(int usuarioId, string fotografiaBase64)
        {
            if (!await _repository.ExisteUsuarioAsync(usuarioId))
                return (-1, null, null);

            byte[] bytes;
            try
            {
                bytes = Convert.FromBase64String(fotografiaBase64.Trim());
            }
            catch (FormatException)
            {
                return (-2, "La fotografia no esta en formato Base 64 valido", null);
            }

            if (bytes.Length == 0)
                return (-2, "La fotografia esta vacia", null);

            if (bytes.Length > MaxBytes)
                return (-2, "La fotografia no debe ser superior a 1 MB", null);

            var filas = await _repository.ActualizarFotografiaAsync(usuarioId, bytes);
            if (filas <= 0) return (0, null, null);

            return (1, null, new FotografiaUsuario
            {
                UsuarioId = usuarioId,
                FotografiaBase64 = Convert.ToBase64String(bytes)
            });
        }

        public async Task<(int, FotografiaUsuario?)> EliminarFotografiaAsync(int usuarioId)
        {
            if (!await _repository.ExisteUsuarioAsync(usuarioId))
                return (-1, null);

            var filas = await _repository.EliminarFotografiaAsync(usuarioId);
            if (filas <= 0) return (0, null);

            return (1, new FotografiaUsuario { UsuarioId = usuarioId, FotografiaBase64 = null });
        }
    }
}
