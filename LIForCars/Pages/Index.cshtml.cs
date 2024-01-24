using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    public IActionResult OnGetUserHeaderPartial() => Partial("Shared/UserHeader");
    public IActionResult OnGetAnonHeaderPartial() => Partial("Shared/AnonHeader");
    public IActionResult OnGetFooterPartial() => Partial("Shared/Footer");
    public IActionResult OnGetAboutPartial() => Partial("Shared/About");
    public IActionResult OnGetContactPartial() => Partial("Shared/Contact");
    public IActionResult OnGetLoginPartial() => Partial("Shared/Login");
    public IActionResult OnGetRegisterPartial() => Partial("Shared/Register");
}
