using CarnetDigitalWeb.Models;
using CarnetDigitalWeb.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarnetDigitalWeb.Pages.Parametro
{
    public class IndexModel : PageModel
    {
        // Cantidad de elementos por pagina, parametrizable segun HU Web17
        private const int PorPagina = 15;

        private readonly IParametroService _service;

        public IndexModel(IParametroService service)
        {
            _service = service;
        }

        public List<Models.Parametro> Items { get; set; } = new();
        public int Pagina { get; set; } = 1;
        public int TotalPaginas { get; set; } = 1;

        public async Task<IActionResult> OnGetAsync(int pagina = 1)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                Avisar("Debe indicar el token antes de continuar.", "warning");
                return Page();
            }

            var (ok, error, data) = await _service.GetAllAsync(token);
            if (!ok)
            {
                Avisar(error!, "danger");
                return Page();
            }

            var lista = data ?? new List<Models.Parametro>();
            TotalPaginas = Math.Max(1, (int)Math.Ceiling(lista.Count / (double)PorPagina));
            Pagina = Math.Clamp(pagina, 1, TotalPaginas);
            Items = lista.Skip((Pagina - 1) * PorPagina).Take(PorPagina).ToList();
            return Page();
        }

        public async Task<IActionResult> OnPostEliminarAsync(string id)
        {
            var token = HttpContext.Session.GetString("Token");
            if (string.IsNullOrWhiteSpace(token))
            {
                Avisar("Debe indicar el token antes de continuar.", "warning");
                return RedirectToPage();
            }

            var (ok, error) = await _service.DeleteAsync(id, token);
            Avisar(ok ? "El parámetro se eliminó correctamente." : error!, ok ? "success" : "danger");
            return RedirectToPage();
        }

        private void Avisar(string mensaje, string tipo)
        {
            TempData["Mensaje"] = mensaje;
            TempData["MensajeTipo"] = tipo;
        }
    }
}
