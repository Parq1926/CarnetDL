using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CarnetDigitalWeb.Pages
{
    public class IndexModel : PageModel
    {
        public IActionResult OnGet() => RedirectToPage("/EstadoUsuario/Index");
    }
}
