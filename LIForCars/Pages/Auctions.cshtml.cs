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
    private readonly ICarRepository _carRepository;
    private readonly IUserRepository _userRepository;

    public AuctionsModel (IAuctionRepository auctionRepository, IBidRepository bidRepository, ICarRepository carRepository, IUserRepository userRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
        _carRepository = carRepository;
        _userRepository = userRepository;
    }

    [BindProperty]
    public Car NewCar { get; set; } = null!;
    [BindProperty]
    public Auction NewAuction { get; set; } = null!;


    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 2;
    public int TotalCount { get; private set; }
    [BindProperty(SupportsGet = true)]
    public string OrderBy { get; set; } = "RemainingTimeAscending";
    [BindProperty(SupportsGet = true)]
    public string FilterBy { get; set; } = "";

    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();
    public Dictionary<Auction, (int TotalBids, IEnumerable<Bid> Bids)> BidsMap { get; private set; } = new Dictionary<Auction, (int, IEnumerable<Bid>)>();


    public async Task<IActionResult> OnGetAsync()
    {
        if (User.IsInRole("Administrator")) {
            return RedirectToPage("/AuctionsAdmin");
        }

        if (OrderBy=="RemainingTimeDescending") {

            // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "RemainingTimeDescending", FilterBy);
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
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "HighestBidAscending", FilterBy);
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
            Auctions = sortedAuctionsAux.Select(tuple => tuple.Item2).ToList().Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        } else if (OrderBy=="HighestBidDescending") {
             // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "HighestBidDescending", FilterBy);
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
            Auctions = sortedAuctionsAux.Select(tuple => tuple.Item2).ToList().Skip((CurrentPage - 1) * PageSize).Take(PageSize);

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        } else {
            // Ir buscar os leilões do user
            var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize, "RemainingTimeAscending", FilterBy);
            Auctions = result.auctions;
            TotalCount = result.totalCount;

            // Ir buscar as bids de um leilão
            foreach (Auction a in Auctions)
            {
                var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
                BidsMap[a] = bids;
            }
        }
        
        return Page();
    }

    public IActionResult OnPostNewAuction()
    {

        _carRepository.Create(NewCar);

        var car = _carRepository.GetByPlate(NewCar.Plate);
        NewAuction.Car = car;
        NewAuction.CarId = (int) car.Id;

        NewAuction.AdministratorId = 2;

        NewAuction.UserId = (int)_userRepository.GetIdByUsername(User.Identity.Name);

        _auctionRepository.Create(NewAuction);

        return new JsonResult(new { success = true });
    }
}
