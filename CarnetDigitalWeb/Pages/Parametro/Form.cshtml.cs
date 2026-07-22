using CarnetDigitalWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarnetDigitalWeb.Pages.Parametro
{
    public class FormModel : PageModel
    {
        private readonly IParametroService _service;

        public FormModel(IParametroService service)
        {
            _service = service;
        }

        [BindProperty]
        public string? Id { get; set; }

        [BindProperty]
        public string? Valor { get; set; }

        [BindProperty]
        public bool EsEdicion { get; set; }

        public async Task<IActionResult> OnGetAsync(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
                return Page();

            EsEdicion = true;

            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                Avisar("Debe indicar el token antes de continuar.", "warning");
                return RedirectToPage("Index");
            }

            var (ok, error, data) = await _service.GetByIdAsync(id, token);
            if (!ok || data is null)
            {
                Avisar(error ?? "No se encontró el parámetro.", "danger");
                return RedirectToPage("Index");
            }

            Id = data.Id;
            Valor = data.Valor;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                Avisar("Debe indicar el token antes de continuar.", "warning");
                return Page();
            }

            if (string.IsNullOrWhiteSpace(Id) || string.IsNullOrWhiteSpace(Valor))
            {
                Avisar("El código y el valor son requeridos.", "warning");
                return Page();
            }

            var (ok, error) = EsEdicion
                ? await _service.UpdateAsync(Id, Valor, token)
                : await _service.CreateAsync(Id, Valor, token);

            if (!ok)
            {
                Avisar(error!, "danger");
                return Page();
            }

            Avisar("El parámetro se guardó correctamente.", "success");
            return RedirectToPage("Index");
        }

        private void Avisar(string mensaje, string tipo)
        {
            TempData["Mensaje"] = mensaje;
            TempData["MensajeTipo"] = tipo;
        }
    }
}
