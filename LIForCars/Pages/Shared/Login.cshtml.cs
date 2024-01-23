using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Models;
using LIForCars.Data.Interfaces;

public class LoginModel : PageModel
{
    [BindProperty]
    [Required]
    public string Username { get; set; } = null!;

    [BindProperty]
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    private readonly IUserRepository userRepository;

    public LoginModel(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public IActionResult OnGetLoginPartial()
    {
        return Partial("Login", this);
    }

    public IActionResult OnPost()
    {
        if (ModelState.IsValid)
        {
            if (userRepository.CheckPassword(Username, Password))
            {
                
                return new JsonResult(new { success = true });
            }
            else
            {
                ModelState.AddModelError("", "Invalid username or password");
            }
        }

        return Partial("Login", this);
    }
}
