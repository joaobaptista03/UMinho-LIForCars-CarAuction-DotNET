using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using System.ComponentModel.DataAnnotations;

public class RegisterModel : PageModel
{
    private readonly IUserRepository userRepository;

    public RegisterModel(IUserRepository _userRepository)
    {
        userRepository = _userRepository;
    }

    [BindProperty]
    public User NewUser { get; set; } = null!;

    [BindProperty]
    public string ConfirmPassword { get; set; } = null!;

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) {
            return Page();
        }

        if (userRepository.NifExists(NewUser.Nif)) ModelState.AddModelError("Nif", "NIF already exists");
        if (userRepository.CcExists(NewUser.CC)) ModelState.AddModelError("Cc", "CC already exists");
        if (userRepository.PhoneExists(NewUser.Phone)) ModelState.AddModelError("Phone", "Phone already exists");
        if (NewUser.Password != ConfirmPassword) ModelState.AddModelError("Password", "Passwords don't match");
        if (userRepository.UsernameExists(NewUser.Username)) ModelState.AddModelError("Username", "Username already exists");
        if (userRepository.EmailExists(NewUser.Email)) ModelState.AddModelError("Email", "Email already exists");
        
        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });

        var createUserResult = await userRepository.CreateAsync(NewUser);
        if (createUserResult) {
            return new JsonResult(new { success = true });
        } else {
            ModelState.AddModelError("", "An error occurred while creating the account.");
            return new JsonResult(new { success = false,  });
        }
    }
}