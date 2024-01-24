using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class AuctionsModel : PageModel
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionsModel (IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 20;

    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();
    public Dictionary<Auction, (int TotalBids, IEnumerable<Bid> Bids)> BidsMap { get; private set; } = new Dictionary<Auction, (int, IEnumerable<Bid>)>();


    public async Task OnGetAsync()
    {

        // Ir buscar os leilões do user
        var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize);
        Auctions = result.auctions;

        // Ir buscar as bids de um leilão
        foreach (Auction a in Auctions)
        {
            Console.WriteLine("AUCTION -> " + a.Id);
            var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
            BidsMap[a] = bids;
        }
    }
}
