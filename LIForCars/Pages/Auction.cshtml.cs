using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

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

    public async Task OnGetAsync(int AuctionId)
    {
        Auction = await _auctionRepository.GetAuctionAsync(AuctionId);
    
        if (Auction != null) 
        {
            var result = await _bidRepository.GetBidsAuctionAsync(AuctionId);
            Bids = result.auctions;
        }
    }
}
