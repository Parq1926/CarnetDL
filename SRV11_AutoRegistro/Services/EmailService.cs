using MailKit.Net.Smtp;
using MimeKit;

namespace SRV11_AutoRegistro.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task EnviarCorreoConfirmacionAsync(
            string destino,
            string enlaceConfirmacion)
        {
            var mensaje = new MimeMessage();

            mensaje.From.Add(
                MailboxAddress.Parse(
                    _configuration["Email:SmtpUser"]));

            mensaje.To.Add(
                MailboxAddress.Parse(destino));

            mensaje.Subject = "Confirmación de cuenta";

            mensaje.Body = new TextPart("html")
            {
                Text =
                    $@"<h2>Confirmación de cuenta</h2>
                       <p>Para activar la cuenta haga clic:</p>
                       <a href='{enlaceConfirmacion}'>
                           Confirmar cuenta
                       </a>"
            };

            using var smtp = new SmtpClient();

            await smtp.ConnectAsync(
                _configuration["Email:SmtpServer"],
                int.Parse(_configuration["Email:SmtpPort"]!),
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(
                _configuration["Email:SmtpUser"],
                _configuration["Email:SmtpPassword"]);

            await smtp.SendAsync(mensaje);

            await smtp.DisconnectAsync(true);
        }
    }
}