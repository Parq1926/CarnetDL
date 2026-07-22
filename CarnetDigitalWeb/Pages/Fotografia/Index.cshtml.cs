using CarnetDigitalWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarnetDigitalWeb.Pages.Fotografia
{
    public class IndexModel : PageModel
    {
        private const long TamanoMaximoBytes = 1024 * 1024; // 1 MB

        private readonly IFotografiaService _service;

        public IndexModel(IFotografiaService service)
        {
            _service = service;
        }

        [BindProperty]
        public int? UsuarioId { get; set; }

        [BindProperty]
        public IFormFile? Foto { get; set; }

        public string? FotoBase64 { get; set; }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostObtenerAsync()
        {
            var token = Token();
            if (string.IsNullOrWhiteSpace(token)) { Avisar("Debe indicar el token antes de continuar.", "warning"); return Page(); }
            if (!UsuarioId.HasValue) { Avisar("Indique el identificador del usuario.", "warning"); return Page(); }

            var (ok, error, foto) = await _service.ObtenerAsync(UsuarioId.Value, token);
            if (!ok) { Avisar(error!, "danger"); return Page(); }

            FotoBase64 = foto;
            return Page();
        }

        public async Task<IActionResult> OnPostActualizarAsync()
        {
            var token = Token();
            if (string.IsNullOrWhiteSpace(token)) { Avisar("Debe indicar el token antes de continuar.", "warning"); return Page(); }
            if (!UsuarioId.HasValue || Foto is null) { Avisar("Indique el identificador del usuario y seleccione una imagen.", "warning"); return Page(); }
            if (Foto.Length > TamanoMaximoBytes) { Avisar("La fotografía no debe superar 1 MB.", "warning"); return Page(); }

            using var ms = new MemoryStream();
            await Foto.CopyToAsync(ms);
            var base64 = Convert.ToBase64String(ms.ToArray());

            var (ok, error, data) = await _service.ActualizarAsync(UsuarioId.Value, base64, token);
            if (!ok) { Avisar(error!, "danger"); return Page(); }

            FotoBase64 = data?.FotografiaBase64;
            Avisar("La fotografía se actualizó correctamente.", "success");
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync()
        {
            var token = Token();
            if (string.IsNullOrWhiteSpace(token)) { Avisar("Debe indicar el token antes de continuar.", "warning"); return Page(); }
            if (!UsuarioId.HasValue) { Avisar("Indique el identificador del usuario.", "warning"); return Page(); }

            var (ok, error) = await _service.EliminarAsync(UsuarioId.Value, token);
            Avisar(ok ? "La fotografía se eliminó correctamente." : error!, ok ? "success" : "danger");
            return Page();
        }

        private string? Token() => HttpContext.Session.GetString("Token");

        private void Avisar(string mensaje, string tipo)
        {
            TempData["Mensaje"] = mensaje;
            TempData["MensajeTipo"] = tipo;
        }
    }
}
