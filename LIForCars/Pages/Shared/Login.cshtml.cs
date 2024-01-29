using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Models;
using LIForCars.Data.Interfaces;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;

public class LoginModel : PageModel
{
    [BindProperty]
    public string Username { get; set; } = null!;

    [BindProperty]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    private readonly IUserRepository userRepository;

    public LoginModel(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!userRepository.CheckPasswordAsync(Username, Password)) ModelState.AddModelError("", "Invalid username or password");
        
        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });

        bool isAdmin = await userRepository.IsAdminAsync(Username);

        var UserId = userRepository.GetIdByUsername(Username).ToString();
        if (UserId == null) return new JsonResult(new { success = false, errors = new List<string> { "Unknown Error" } });
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Username),
            new Claim(ClaimTypes.NameIdentifier, UserId),
            new Claim(ClaimTypes.Role, isAdmin ? "Administrator" : "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "AuthCookies");

        await HttpContext.SignInAsync("AuthCookies", new ClaimsPrincipal(claimsIdentity));

        return new JsonResult(new { success = true, isAdmin = isAdmin });
    }
}