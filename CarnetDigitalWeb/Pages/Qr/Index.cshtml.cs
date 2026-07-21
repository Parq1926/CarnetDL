using CarnetDigitalWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarnetDigitalWeb.Pages.Qr
{
    public class IndexModel : PageModel
    {
        private readonly ICarnetQRService _service;

        public IndexModel(ICarnetQRService service)
        {
            _service = service;
        }

        [BindProperty]
        public string? Identificacion { get; set; }

        public string? QrBase64 { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                Avisar("Debe indicar el token antes de continuar.", "warning");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Identificacion))
            {
                Avisar("Indique la identificación del usuario.", "warning");
                return Page();
            }

            var (ok, error, qr) = await _service.ObtenerQRAsync(Identificacion, token);
            if (!ok)
            {
                Avisar(error!, "danger");
                return Page();
            }

            QrBase64 = qr;
            return Page();
        }

        private void Avisar(string mensaje, string tipo)
        {
            TempData["Mensaje"] = mensaje;
            TempData["MensajeTipo"] = tipo;
        }
    }
}
