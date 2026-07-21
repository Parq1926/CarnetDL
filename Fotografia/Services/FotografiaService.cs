using SRV13_Fotografia.Entities;
using SRV13_Fotografia.Repository;

namespace SRV13_Fotografia.Services
{
    public class FotografiaService : IFotografiaService
    {
        private readonly FotografiaRepository _repository;

        public FotografiaService(FotografiaRepository repository) { _repository = repository; }

        public async Task<FotografiaUsuario?> ObtenerFotografiaAsync(string identificacion) =>
            await _repository.ObtenerPorIdentificacionAsync(identificacion);

        public async Task<int> ActualizarFotografiaAsync(string identificacion, string fotografiaBase64) =>
            await _repository.ActualizarFotografiaAsync(identificacion, fotografiaBase64);

        public async Task<int> EliminarFotografiaAsync(string identificacion) =>
            await _repository.EliminarFotografiaAsync(identificacion);
    }
}
