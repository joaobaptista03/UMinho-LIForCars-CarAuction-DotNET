using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.VisualBasic;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


public class AuctionModel : PageModel
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionModel(IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }


    [BindProperty(SupportsGet = true)]
    public int AuctionId { get; set; }
    public Auction? Auction { get; set; }
    public IEnumerable<Bid> Bids { get; private set; } = Enumerable.Empty<Bid>();

    [BindProperty]
    public decimal bidValue { get; set; }

    public async Task OnGetAsync(int AuctionId)
    {
        Auction = await _auctionRepository.GetAuctionAsync(AuctionId);
    
        if (Auction != null) 
        {
            var result = await _bidRepository.GetBidsAuctionAsync(AuctionId);
            Bids = result.auctions;
        }
    }

    public async Task<IActionResult> OnPostPlaceBid()
    {
        Auction AuctionDetails = _auctionRepository.GetById(AuctionId);

        var UserNameIdentifier = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (UserNameIdentifier == null) return new JsonResult(new { success = false, errors = new List<string> { "Unknown Error" } });

        if (int.Parse(UserNameIdentifier) == AuctionDetails.UserId)
        {
            ModelState.AddModelError("", "You can't bid on your own Auction!");
        }

        Bid CurrentBid = await _bidRepository.GetCurrentBidForAuctionAsync(AuctionId);

        if (AuctionDetails == null) return new JsonResult(new { success = false, errors = new List<string> { "Auction not found" } });
        decimal currBidValue = AuctionDetails.BasePrice;

        if (CurrentBid != null) currBidValue = CurrentBid.BidValue;

        if (bidValue < currBidValue + AuctionDetails.MinIncrement)
        {
            ModelState.AddModelError("BidValue", "Value must be at least " + (currBidValue + AuctionDetails.MinIncrement));
        }

        var modelErrors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
        if (!ModelState.IsValid) return new JsonResult(new { success = false, errors = modelErrors });


        var bid = new Bid
        {
            AuctionId = this.AuctionId,
            UserId = int.Parse(UserNameIdentifier),
            BidValue = bidValue,
            bidTime = DateTime.Now
        };

        _bidRepository.Create(bid);

        return new JsonResult(new { success = true });
    }
}
