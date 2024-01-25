using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Models;
using LIForCars.Data.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public class HeaderModel : PageModel
{    
    public async Task<IActionResult> OnPostAsync()
    {
        await HttpContext.SignOutAsync("AuthCookies");
        return new JsonResult(new { success = true });
    }
}