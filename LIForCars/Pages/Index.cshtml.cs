using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class IndexModel : PageModel
{
    public IActionResult OnGetLoginPartial() => Partial("Shared/Login");
    public IActionResult OnGetRegisterPartial() => Partial("Shared/Register");

    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync("AuthCookies");
        return new JsonResult(new { success = true });
    }
}
