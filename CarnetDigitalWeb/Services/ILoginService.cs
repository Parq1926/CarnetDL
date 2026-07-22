using CarnetDigitalWeb.Models;
using System.Threading.Tasks;

namespace CarnetDigitalWeb.Services
{
    public interface ILoginService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);
    }
}