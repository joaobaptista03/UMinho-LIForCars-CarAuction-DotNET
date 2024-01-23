using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Models;
using LIForCars.Data.Interfaces;

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

    public IActionResult OnPostAsync()
    {
        if (!userRepository.CheckPasswordAsync(Username, Password)) ModelState.AddModelError("", "Invalid username or password");
        
        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });

        return new JsonResult(new { success = true });
    }
}
