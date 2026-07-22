using BitacoraSRV9.Entities;
using BitacoraSRV9.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BitacoraSRV9.Pages.Bitacora
{
    public class IndexModel : PageModel
    {
        private readonly IBitacoraService _service;

        public IndexModel(IBitacoraService service)
        {
            _service = service;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnGetFiltros(
            DateTime? fechaInicio,
            DateTime? fechaFin,
            string? usuario,
            string? accion,
            int pagina = 1,
            int tamanoPagina = 15,
            bool soloErrores = false)
        {
            var filtros = new BitacoraFiltrosRequest
            {
                FechaInicio = fechaInicio,
                FechaFin = fechaFin,
                Usuario = usuario,
                Accion = accion,
                Pagina = pagina,
                TamanoPagina = tamanoPagina,
                SoloErrores = soloErrores
            };

            var resultado = await _service.ObtenerConFiltrosAsync(filtros);

            return new JsonResult(resultado);
        }
    }
}