using QRCoder;
using SRV14_CarnetQR.Entities;
using SRV14_CarnetQR.Repository;
using System.Text.Json;

namespace SRV14_CarnetQR.Services
{
    public class CarnetQRService : ICarnetQRService
    {
        private readonly CarnetQRRepository _repository;

        public CarnetQRService(CarnetQRRepository repository)
        {
            _repository = repository;
        }

        public async Task<string?> GenerarQRAsync(string identificacion)
        {
            var usuario = await _repository.ObtenerUsuarioAsync(identificacion);
            if (usuario is null)
                return null;

            // Estudiante -> carreras asociadas | Funcionario -> areas asociadas
            var esEstudiante = usuario.TipoUsuario.Contains("Estudiante", StringComparison.OrdinalIgnoreCase);

            var carrerasOAreas = esEstudiante
                ? await _repository.ObtenerCarrerasAsync(usuario.Id)
                : await _repository.ObtenerAreasAsync(usuario.Id);

            var instituciones = await _repository.ObtenerInstitucionesAsync(usuario.Id);

            var datosCarnet = new CarnetQRData
            {
                NombreCompleto   = usuario.NombreCompleto,
                Identificacion   = usuario.NumeroIdentificacion,
                TipoUsuario      = usuario.TipoUsuario,
                CarrerasOAreas   = carrerasOAreas,
                Institucion      = string.Join(", ", instituciones),
                FechaVencimiento = new DateOnly(DateTime.Today.Year, 12, 31)
            };

            var jsonContenido = JsonSerializer.Serialize(datosCarnet, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return GenerarQRBase64(jsonContenido);
        }

        private static string GenerarQRBase64(string contenido)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            var pngBytes = qrCode.GetGraphic(10);
            return Convert.ToBase64String(pngBytes);
        }
    }
}
