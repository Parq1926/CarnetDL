using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AreasSRV7.Pages.Areas
{
    [AllowAnonymous]
    public class IndexModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}