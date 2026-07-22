namespace SRV11_AutoRegistro.Services
{
    public interface IEmailService
    {
        Task EnviarCorreoConfirmacionAsync(
            string destino,
            string enlaceConfirmacion);
    }
}