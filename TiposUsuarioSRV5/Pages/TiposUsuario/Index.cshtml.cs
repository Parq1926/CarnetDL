using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace TiposUsuarioSRV5.Pages.TiposUsuario
{
    [AllowAnonymous] // ✅ Permitir acceso sin autenticación
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}