using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using System.Globalization;

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
        if (!ModelState.IsValid) return Page();

        if (userRepository.NifExists(NewUser.Nif)) ModelState.AddModelError("Nif", "NIF already exists");
        if (Math.Abs(NewUser.Nif).ToString().Length != 9) ModelState.AddModelError("Nif", "NIF must have 9 digits");
        if (userRepository.CcExists(NewUser.CC)) ModelState.AddModelError("Cc", "CC already exists");
        if (Math.Abs(NewUser.CC).ToString().Length != 8) ModelState.AddModelError("Cc", "CC must have 8 digits");
        if (userRepository.PhoneExists(NewUser.Phone)) ModelState.AddModelError("Phone", "Phone already exists");
        if (Math.Abs(NewUser.Phone).ToString().Length != 9) ModelState.AddModelError("Phone", "Phone must have 9 digits");
        if (NewUser.Password != ConfirmPassword) ModelState.AddModelError("Password", "Passwords don't match");
        if (userRepository.UsernameExists(NewUser.Username)) ModelState.AddModelError("Username", "Username already exists");
        foreach (char c in NewUser.Username) if (!char.IsLetterOrDigit(c)) ModelState.AddModelError("Username", "Username must only contain letters and numbers");
        if (userRepository.EmailExists(NewUser.Email)) ModelState.AddModelError("Email", "Email already exists");
        if (!IsValidEmail(NewUser.Email)) ModelState.AddModelError("Email", "Invalid email");

        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });

        var createUserResult = await userRepository.CreateAsync(NewUser);
        if (createUserResult) {
            return new JsonResult(new { success = true });
        } else {
            ModelState.AddModelError("", "An error occurred while creating the account.");
            return new JsonResult(new { success = false });
        }
    }

    bool IsValidEmail(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return false;

        try {
            email = Regex.Replace(email, @"(@)(.+)$", DomainMapper, RegexOptions.None, TimeSpan.FromMilliseconds(200));

            string DomainMapper(Match match) {
                var idn = new IdnMapping();
                string domainName = idn.GetAscii(match.Groups[2].Value);
                return match.Groups[1].Value + domainName;
            }
        } catch (RegexMatchTimeoutException) {
            return false;
        } catch (ArgumentException) {
            return false;
        }

        try {
            return Regex.IsMatch(email,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase, TimeSpan.FromMilliseconds(250));
        } catch (RegexMatchTimeoutException) {
            return false;
        }
    }

}