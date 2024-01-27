using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

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
    public int PageSize { get; set; } = 5;
    public int TotalCount { get; private set; }
    [BindProperty(SupportsGet = true)]
    public string OrderBy { get; set; } = "RemainingTimeAscending";

    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();
    public Dictionary<Auction, (int TotalBids, IEnumerable<Bid> Bids)> BidsMap { get; private set; } = new Dictionary<Auction, (int, IEnumerable<Bid>)>();


    public async Task OnGetAsync()
    {
        if (OrderBy=="RemainingTimeDescending") {
            // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "RemainingTimeDescending");
            Auctions = result.auctions;
            TotalCount = result.totalCount;

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        } else if (OrderBy=="HighestBidAscending") {
            // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "RemainingTimeDescending");
            Auctions = result.auctions;
            TotalCount = result.totalCount;

            List<(float, Auction)> AuctionsAux = new List<(float, Auction)>();
            foreach (Auction a in Auctions)
            {
                var highestBid = await _bidRepository.GetHighestBidAuctionAsync(a.Id);
                if (highestBid==null) {
                    highestBid = (float?) a.BasePrice;
                }
                AuctionsAux.Add((highestBid.Value, a));
            }

            // Ordenar pelo maior valor
            var sortedAuctionsAux = AuctionsAux.OrderBy(tuple => tuple.Item1).ToList();

            // Deixar só as auctions
            Auctions = sortedAuctionsAux.Select(tuple => tuple.Item2).ToList();

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        } else if (OrderBy=="HighestBidDescending") {
             // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "RemainingTimeDescending");
            Auctions = result.auctions;
            TotalCount = result.totalCount;

            List<(float, Auction)> AuctionsAux = new List<(float, Auction)>();
            foreach (Auction a in Auctions)
            {
                var highestBid = await _bidRepository.GetHighestBidAuctionAsync(a.Id);
                if (highestBid==null) {
                    highestBid = (float?) a.BasePrice;
                }
                AuctionsAux.Add((highestBid.Value, a));
            }

            // Ordenar pelo menor valor
            var sortedAuctionsAux = AuctionsAux.OrderByDescending(tuple => tuple.Item1).ToList();

            // Deixar só as auctions
            Auctions = sortedAuctionsAux.Select(tuple => tuple.Item2).ToList();

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        } else {
            // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "RemainingTimeAscending");
            Auctions = result.auctions;
            TotalCount = result.totalCount;

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        }
        
    }
}
