using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SRV3_Carreras.Pages.Carreras
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}