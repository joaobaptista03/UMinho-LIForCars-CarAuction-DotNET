using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    public IActionResult OnGetHeaderPartial() => Partial("Shared/Header");
    public IActionResult OnGetLoginPartial() => Partial("Shared/Login");
    public IActionResult OnGetRegisterPartial() => Partial("Shared/Register");
}
