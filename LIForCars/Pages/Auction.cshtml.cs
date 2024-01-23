using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Models;
using LIForCars.Data.Interfaces;

public class AuctionModel : PageModel
{
    private readonly IAuctionRepository _repository;

    public AuctionModel(IAuctionRepository repository)
    {
        _repository = repository;
    }

    public IActionResult OnGetHeaderPartial() => Partial("Shared/Header");
    public IActionResult OnGetFooterPartial() => Partial("Shared/Footer");
    public IActionResult OnGetAboutPartial() => Partial("Shared/About");
    public IActionResult OnGetContactPartial() => Partial("Shared/Contact");
    public IActionResult OnGetLoginPartial() => Partial("Shared/Login");
    public IActionResult OnGetRegisterPartial() => Partial("Shared/Register");

    public Auction? AuctionDetails { get; private set; }

    public IActionResult OnGet(int id)
    {
        AuctionDetails = _repository.GetById(id);
        return Page();
    }
}
