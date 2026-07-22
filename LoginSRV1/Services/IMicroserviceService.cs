using LoginSRV1.DTOs;

namespace LoginSRV1.Services
{
    public interface IMicroserviceService
    {
        MicroservicesConfigDto GetConfig();
        string GetMicroserviceUrl(string serviceName);
    }
}