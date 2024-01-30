using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class UserPageModel : PageModel
{
    private readonly IUserRepository _userRepository;
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public UserPageModel(IUserRepository userRepository, IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _userRepository = userRepository;
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public new User? User { get; private set; }
    public int PageSize { get; set; } = 100;
    public int TotalCount { get; private set; }
    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();
    public IEnumerable<Auction> AuctionsWaitingApproval { get; private set; } = Enumerable.Empty<Auction>();
    public Dictionary<Auction, (int TotalBids, IEnumerable<Bid> Bids)> BidsMap { get; private set; } = new Dictionary<Auction, (int, IEnumerable<Bid>)>();
    public Dictionary<Auction, IEnumerable<Bid>> AuctionsUserBidded { get; private set; } = new Dictionary<Auction, IEnumerable<Bid>>();
    public Dictionary<(int M, int A), int> AuctionsPerMonth { get; private set; } = new Dictionary<(int M, int A), int>();
    public int NrTotalAuctions { get; private set; }
    public int NrWaitingForApprovalAuctions { get; private set; }
    public int NrFutureApprovedAuctions { get; private set; }
    public int NrFinishedAuctions { get; private set; }
    public int NrOnGoingAuctions { get; private set; }
    public float? MeanSellValueAuctions { get; private set; }
    public float MeanNrBidsPerAuction { get; private set; }
    public float? TotalEarnedAuctions { get; private set; }

    public async Task OnGetAsync(String Username)
    {
        User? user = _userRepository.GetByUsername(Username);
        if (user==null)
        {
            Console.WriteLine(Username);
            return;
        }
        int UserId = user.Id;
        
        // Ir buscar o user
        User = await _userRepository.GetUserByIdAsync(UserId);

        // Ir buscar os leilões do user
        var result = await _auctionRepository.GetAuctionsUserAsync(CurrentPage, PageSize, UserId);
        Auctions = result.auctions;
        TotalCount = result.totalCount;

        AuctionsWaitingApproval = await _auctionRepository.GetAuctionsUserWaitingApprovalAsync(CurrentPage, PageSize, UserId);

        // Ir buscar as bids de um leilão
        foreach (Auction a in Auctions)
        {
            var bids = await _bidRepository.GetBidsAuctionAsync(a.Id);
            BidsMap[a] = bids;
        }

        // Ir buscar as bids de todos os usuários de onde o User participou
        var bidsParticipated = await _bidRepository.GetBidsUserParticipatedAsync(UserId);
        foreach(Bid b in bidsParticipated) {
            if (!AuctionsUserBidded.ContainsKey(b.Auction))
            {
                AuctionsUserBidded[b.Auction] = new List<Bid>();
            }
            ((List<Bid>)AuctionsUserBidded[b.Auction]).Add(b);
        }

        var allAuctions = await _auctionRepository.GetAllAuctionsUserAsync(UserId);

        NrTotalAuctions = 0;
        NrWaitingForApprovalAuctions = 0;
        NrFutureApprovedAuctions = 0;
        NrFinishedAuctions = 0;
        NrOnGoingAuctions = 0;
        
        DateTime currentDateTime = DateTime.Now;
        foreach (Auction a in allAuctions) {
            NrTotalAuctions++;
            if (a.Autorized==true) {
                if (a.InitDateTime<=currentDateTime && a.EndDateTime>=currentDateTime) {
                    NrOnGoingAuctions++;
                } else if (a.InitDateTime>currentDateTime) {
                    NrFutureApprovedAuctions++;
                } else {
                    NrFinishedAuctions++;
                }
            } else {
                NrWaitingForApprovalAuctions++;
            }
        }

        var auctionsGroupedById = await _bidRepository.GetMeanSellValueAuctionsUserAsync(allAuctions);

        MeanSellValueAuctions = 0;
        MeanNrBidsPerAuction = 0;
        TotalEarnedAuctions = 0;
        if (auctionsGroupedById!=null && auctionsGroupedById.Any()) {

            auctionsGroupedById = auctionsGroupedById.OrderBy(group => group.Key.InitDateTime).ToList();

            var firstDate = true;
            DateTime maxDate = DateTime.MinValue;
            foreach (IGrouping<Auction, Bid> g in auctionsGroupedById) {

                int month = g.Key.EndDateTime.Month;
                int year = g.Key.EndDateTime.Year;
                if (firstDate) {
                    AuctionsPerMonth[(month, year)] = 1;
                    firstDate = false;
                    maxDate = new DateTime(year, month, 1);
                } else {
                    if (AuctionsPerMonth.ContainsKey((month, year))) {
                        AuctionsPerMonth[(month, year)] += 1;
                    } else {
                        while (maxDate<g.Key.EndDateTime) {
                            AuctionsPerMonth[(maxDate.Month,maxDate.Year)] = 0;
                        }
                        if (AuctionsPerMonth.ContainsKey((month, year))) {
                            AuctionsPerMonth[(month, year)] += 1;
                        } else {
                            AuctionsPerMonth[(month, year)] = 1;
                        }
                    }
                }

                MeanNrBidsPerAuction += g.Count();
                var i = 0;
                foreach (Bid bid in g) {
                    if (i==0) {
                        TotalEarnedAuctions += (float) bid.BidValue;
                    }
                    i++;
                }
            }

            var nrAuctions = auctionsGroupedById.Count();
            MeanSellValueAuctions = TotalEarnedAuctions / nrAuctions;
            MeanNrBidsPerAuction = MeanNrBidsPerAuction / nrAuctions;
        }


    }
}
