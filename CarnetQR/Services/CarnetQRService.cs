using QRCoder;
using SRV14_CarnetQR.Entities;
using SRV14_CarnetQR.Repository;
using System.Text.Json;

// Nota: CarnetQRData y CarnetQR se usan internamente para armar el contenido del QR.

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
            var datosCarnet = ObtenerDatosSimulados(identificacion);
            if (datosCarnet is null)
                return null;

            var jsonContenido = JsonSerializer.Serialize(datosCarnet, new JsonSerializerOptions
            {
                WriteIndented = false,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var qrBase64 = GenerarQRBase64(jsonContenido);

            await _repository.UpsertCarnetQRAsync(identificacion, qrBase64);

            return qrBase64;
        }

        private static string GenerarQRBase64(string contenido)
        {
            using var qrGenerator = new QRCodeGenerator();
            var qrData = qrGenerator.CreateQrCode(contenido, QRCodeGenerator.ECCLevel.Q);
            using var qrCode = new PngByteQRCode(qrData);
            var pngBytes = qrCode.GetGraphic(10);
            return Convert.ToBase64String(pngBytes);
        }

        private static CarnetQRData? ObtenerDatosSimulados(string identificacion)
        {
            var usuarios = new Dictionary<string, CarnetQRData>(StringComparer.OrdinalIgnoreCase)
            {
                ["1-0234-5678"] = new CarnetQRData
                {
                    NombreCompleto   = "Ana María Solís Herrera",
                    Identificacion   = "1-0234-5678",
                    TipoUsuario      = "Estudiante",
                    CarrerasOAreas   = new List<string> { "Ingeniería en Tecnologías de Información" },
                    Institucion      = "Colegio Universitario de Cartago",
                    FechaVencimiento = new DateTime(2026, 12, 31)
                },
                ["2-0456-7890"] = new CarnetQRData
                {
                    NombreCompleto   = "Carlos Eduardo Mora Jiménez",
                    Identificacion   = "2-0456-7890",
                    TipoUsuario      = "Funcionario",
                    CarrerasOAreas   = new List<string> { "Departamento de Registro", "Departamento de TI" },
                    Institucion      = "Colegio Universitario de Cartago",
                    FechaVencimiento = new DateTime(2026, 12, 31)
                },
                ["3-0678-9012"] = new CarnetQRData
                {
                    NombreCompleto   = "Laura Beatriz Vargas Campos",
                    Identificacion   = "3-0678-9012",
                    TipoUsuario      = "Estudiante",
                    CarrerasOAreas   = new List<string> { "Administración de Empresas", "Contabilidad" },
                    Institucion      = "Colegio Universitario de Cartago",
                    FechaVencimiento = new DateTime(2026, 12, 31)
                },
                ["4-0891-2345"] = new CarnetQRData
                {
                    NombreCompleto   = "Roberto Alvarado Navarro",
                    Identificacion   = "4-0891-2345",
                    TipoUsuario      = "Administrador",
                    CarrerasOAreas   = new List<string> { "Dirección General" },
                    Institucion      = "Colegio Universitario de Cartago",
                    FechaVencimiento = new DateTime(2026, 12, 31)
                }
            };

            return usuarios.TryGetValue(identificacion, out var datos) ? datos : null;
        }
    }
}
