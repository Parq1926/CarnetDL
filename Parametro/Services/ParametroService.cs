using SRV15_Parametro.Entities;
using SRV15_Parametro.Repository;
using System.Text.RegularExpressions;

namespace SRV15_Parametro.Services
{
    public class ParametroService : IParametroService
    {
        private readonly ParametroRepository _repository;

        public ParametroService(ParametroRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Parametro>> GetAllAsync() =>
            await _repository.GetAllAsync();

        public async Task<Parametro?> GetByIdAsync(string id) =>
            await _repository.GetByIdAsync(id);

        public async Task<(bool ok, string error)> CreateAsync(ParametroRequest request)
        {
            var validacion = ValidarRequest(request);
            if (validacion is not null)
                return (false, validacion);

            var existe = await _repository.GetByIdAsync(request.Id);
            if (existe is not null)
                return (false, $"Ya existe un parámetro con el identificador '{request.Id}'");

            var parametro = new Parametro { Id = request.Id, Valor = request.Valor };
            await _repository.CreateAsync(parametro);
            return (true, string.Empty);
        }

        public async Task<(bool ok, string error)> UpdateAsync(string id, ParametroRequest request)
        {
            var validacion = ValidarRequest(request);
            if (validacion is not null)
                return (false, validacion);

            var existe = await _repository.GetByIdAsync(id);
            if (existe is null)
                return (false, $"No se encontró el parámetro con identificador '{id}'");

            var parametro = new Parametro { Id = id, Valor = request.Valor };
            await _repository.UpdateAsync(parametro);
            return (true, string.Empty);
        }

        public async Task<(bool ok, string error)> DeleteAsync(string id)
        {
            var existe = await _repository.GetByIdAsync(id);
            if (existe is null)
                return (false, $"No se encontró el parámetro con identificador '{id}'");

            await _repository.DeleteAsync(id);
            return (true, string.Empty);
        }

        // ----------------------------------------------------------
        // Validaciones según HU SRV15:
        // - ID: requerido, máximo 10 caracteres, solo letras mayúsculas
        // - Valor: requerido, máximo 500 caracteres, formato libre
        // ----------------------------------------------------------
        private static string? ValidarRequest(ParametroRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
                return "El identificador es requerido";

            if (request.Id.Length > 10)
                return "El identificador no puede superar 10 caracteres";

            if (!Regex.IsMatch(request.Id, @"^[A-Z]+$"))
                return "El identificador solo permite letras en mayúscula";

            if (string.IsNullOrWhiteSpace(request.Valor))
                return "El valor es requerido";

            if (request.Valor.Length > 500)
                return "El valor no puede superar 500 caracteres";

            return null;
        }
    }
}
