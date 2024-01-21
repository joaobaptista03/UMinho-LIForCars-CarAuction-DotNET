// Pages/User.cshtml.cs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class UserPageModel : PageModel
{
    [BindProperty(SupportsGet = true)]
    public int UserId { get; set; }
    public string UserName { get; set; } = string.Empty;
    public string UserEmail { get; set; } = string.Empty;

    public void OnGet()
    {
        // Simulated user data based on the provided UserId (you would retrieve this from a database in a real scenario)
        // For simplicity, using hardcoded data for demonstration purposes.
        UserName = "John Doe";
        UserEmail = "john.doe@example.com";
    }
}
