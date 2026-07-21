namespace SRV4_Areas.Services
{
    public interface IBitacoraService
    {
        Task Registrar(string usuario, string accion);
    }

    public class BitacoraService : IBitacoraService
    {
        private readonly HttpClient _httpClient;
        private readonly string _bitacoraUrl;

        public BitacoraService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _bitacoraUrl = configuration["BitacoraUrl"] ?? "http://localhost:5209/bitacora";
        }

        public async Task Registrar(string usuario, string accion)
        {
            try
            {
                await _httpClient.PostAsJsonAsync(_bitacoraUrl, new
                {
                    Usuario = usuario,
                    Accion = accion,
                    Fecha = DateTime.Now
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al registrar en bitacora: {ex.Message}");
            }
        }
    }
}
