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
    private readonly IAdministratorRepository administratorRepository;

    public LoginModel(IUserRepository userRepository, IAdministratorRepository administratorRepository)
    {
        this.userRepository = userRepository;
        this.administratorRepository = administratorRepository;
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!userRepository.CheckPasswordAsync(Username, Password)) ModelState.AddModelError("", "Invalid username or password");
        
        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });

        bool isAdmin = await userRepository.IsAdminAsync(Username);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, Username),
            new Claim(ClaimTypes.NameIdentifier, userRepository.GetIdByUsername(Username).ToString()),
            new Claim(ClaimTypes.Role, isAdmin ? "Administrator" : "User")
        };

        var claimsIdentity = new ClaimsIdentity(claims, "AuthCookies");

        await HttpContext.SignInAsync("AuthCookies", new ClaimsPrincipal(claimsIdentity));

        return new JsonResult(new { success = true, isAdmin = isAdmin });
    }
}