using LoginSRV1.DTOs;

namespace LoginSRV1.Services
{
    public class MicroserviceService : IMicroserviceService
    {
        private readonly MicroservicesConfigDto _config;

        public MicroserviceService(IConfiguration configuration)
        {
            _config = configuration.GetSection("Microservices").Get<MicroservicesConfigDto>()
                ?? new MicroservicesConfigDto();
        }

        public MicroservicesConfigDto GetConfig()
        {
            return _config;
        }

        public string GetMicroserviceUrl(string serviceName)
        {
            var property = typeof(MicroservicesConfigDto).GetProperty(serviceName);
            if (property == null)
            {
                throw new ArgumentException($"Microservicio '{serviceName}' no encontrado");
            }

            var service = property.GetValue(_config) as MicroserviceConfig;
            return service?.FullUrl ?? string.Empty;
        }
    }
}