using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using LIForCars.Models;
using LIForCars.Data.Interfaces;

public class AuctionModel : PageModel
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionModel(IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }

    public IActionResult OnGetHeaderPartial() => Partial("Shared/Header");
    public IActionResult OnGetFooterPartial() => Partial("Shared/Footer");
    public IActionResult OnGetAboutPartial() => Partial("Shared/About");
    public IActionResult OnGetContactPartial() => Partial("Shared/Contact");
    public IActionResult OnGetLoginPartial() => Partial("Shared/Login");
    public IActionResult OnGetRegisterPartial() => Partial("Shared/Register");

    public Auction? AuctionDetails { get; private set; }
    public Bid? CurrentBid { get; private set; }
    
    [BindProperty]
    public int auctionId { get; set; }

    [BindProperty]
    public decimal bidValue { get; set; }

    public async Task<IActionResult> OnGetAsync(int id)
    {
        AuctionDetails = _auctionRepository.GetById(id);
        CurrentBid = await _bidRepository.GetCurrentBidForAuctionAsync(id);

        return Page();
    }

    public async Task<IActionResult> OnPostPlaceBid() {
        Console.WriteLine("AuctionId: " + auctionId);
            
        AuctionDetails = _auctionRepository.GetById(auctionId);
        CurrentBid = await _bidRepository.GetCurrentBidForAuctionAsync(auctionId);

        if (AuctionDetails == null) return new JsonResult(new { success = false, errors = new List<string> { "Auction not found" } });
        if (CurrentBid == null) return new JsonResult(new { success = false, errors = new List<string> { "Bid not found" } });

        if (bidValue < CurrentBid.BidValue + AuctionDetails.MinIncrement) {
            ModelState.AddModelError("BidValue", "Value must be at least " + CurrentBid.BidValue + AuctionDetails.MinIncrement);
        }

        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });
        
        
        var bid = new Bid {
            AuctionId = auctionId,
            UserId = 0,
            BidValue = bidValue,
            bidTime = DateTime.Now
        };

        _bidRepository.Create(bid);
        
        return new JsonResult(new { success = true });
    }

}
