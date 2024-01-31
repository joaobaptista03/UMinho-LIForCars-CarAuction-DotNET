using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.ComponentModel.DataAnnotations;

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
    public bool IsAdmin { get; private set; }
    public int PageSize { get; set; } = 100;
    public int TotalCount { get; private set; }
    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();
    public IEnumerable<Auction> AuctionsWaitingApproval { get; private set; } = Enumerable.Empty<Auction>();
    public Dictionary<Auction, (int TotalBids, IEnumerable<Bid> Bids)> BidsMap { get; private set; } = new Dictionary<Auction, (int, IEnumerable<Bid>)>();
    public Dictionary<Auction, IEnumerable<Bid>> AuctionsUserBidded { get; private set; } = new Dictionary<Auction, IEnumerable<Bid>>();
    public Dictionary<(int M, int A), int> AuctionsPerMonth { get; private set; } = new Dictionary<(int M, int A), int>();
    public Dictionary<(int M, int A), (int q, float t)> AuctionsPerMonthAdmin { get; private set; } = new Dictionary<(int M, int A), (int q, float t)>();
    public int NrTotalAuctions { get; private set; }
    public int NrWaitingForApprovalAuctions { get; private set; }
    public int NrFutureApprovedAuctions { get; private set; }
    public int NrFinishedAuctions { get; private set; }
    public int NrOnGoingAuctions { get; private set; }
    public float? MeanSellValueAuctions { get; private set; }
    public float MeanNrBidsPerAuction { get; private set; }
    public float? TotalEarnedAuctions { get; private set; }

    public async Task OnGetAsync()
    {
        User? user = _userRepository.GetByUsername(base.User.Identity.Name);
        if (user==null)
        {
            return;
        }
        int UserId = user.Id;
        
        // Ir buscar o user
        User = await _userRepository.GetUserByIdAsync(UserId);
        IsAdmin = await _userRepository.IsAdminAsync(user.Username);

        // Ir buscar os leilões do user
        var result = await _auctionRepository.GetAuctionsUserAsync(CurrentPage, PageSize, UserId);
        Auctions = result.auctions;
        TotalCount = result.totalCount;

        if (!IsAdmin) {
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

            var auctionsGroupedById = await _bidRepository.GetBidsAuctionsUserAsync(allAuctions);
            Dictionary<Auction, List<Bid>> auctionsGroupedByIdAux = new Dictionary<Auction, List<Bid>>();
            if (auctionsGroupedById!=null && auctionsGroupedById.Any()) {
                foreach (IGrouping<Auction, Bid> g in auctionsGroupedById) {
                    List<Bid> bidsAuction = new List<Bid>();
                    foreach (Bid b in g) {
                        bidsAuction.Add(b);
                    }
                    auctionsGroupedByIdAux[g.Key] = bidsAuction;
                }
            }

            foreach (Auction a in allAuctions) {
                if (!auctionsGroupedByIdAux.ContainsKey(a)) {
                    auctionsGroupedByIdAux[a] = new List<Bid>();
                }
            }

            List<(Auction, List<Bid>)> bidsGroupedByAuction = auctionsGroupedByIdAux.Select(kv => (kv.Key, kv.Value))
                                                                                    .OrderBy(a => a.Key.EndDateTime) // Use OrderBy for sorting
                                                                                    .ToList();

            MeanSellValueAuctions = 0;
            MeanNrBidsPerAuction = 0;
            TotalEarnedAuctions = 0;
            if (bidsGroupedByAuction!=null && bidsGroupedByAuction.Any()) {

                var firstDate = true;
                DateTime maxDate = DateTime.MinValue;
                var checkIfLastDate = 0;
                foreach ((Auction a , List<Bid> bids) g in bidsGroupedByAuction) {

                    int month = g.a.EndDateTime.Month;
                    int year = g.a.EndDateTime.Year;
                    if (firstDate) {
                        AuctionsPerMonth[(month, year)] = 1;
                        firstDate = false;
                        maxDate = new DateTime(year, month, 1);
                        maxDate = maxDate.AddMonths(1);
                    } else {
                        if (AuctionsPerMonth.ContainsKey((month, year))) {
                            AuctionsPerMonth[(month, year)] += 1;
                        } else {
                            while (maxDate<g.a.EndDateTime) {
                                AuctionsPerMonth[(maxDate.Month,maxDate.Year)] = 0;
                                maxDate = maxDate.AddMonths(1);
                            }
                            if (AuctionsPerMonth.ContainsKey((month, year))) {
                                AuctionsPerMonth[(month, year)] += 1;
                            } else {
                                AuctionsPerMonth[(month, year)] = 1;
                            }
                        }
                    }

                    checkIfLastDate++;
                    if (checkIfLastDate==bidsGroupedByAuction.Count()) {
                        DateTime presentDate = DateTime.Now;
                        while(presentDate>g.a.EndDateTime) {
                            g.a.EndDateTime = g.a.EndDateTime.AddMonths(1);
                            AuctionsPerMonth[(g.a.EndDateTime.Month, g.a.EndDateTime.Year)] = 0;
                        }
                    }

                    if (g.a.Autorized && g.a.EndDateTime<DateTime.Now) {

                        MeanNrBidsPerAuction += g.bids.Count();
                        var i = 0;
                        foreach (Bid bid in g.bids) {
                            if (i==0) {
                                TotalEarnedAuctions += (float) bid.BidValue;
                            }
                            i++;
                        }
                    }
                    
                }

                var nrAuctions = bidsGroupedByAuction.Count();
                MeanSellValueAuctions = TotalEarnedAuctions / nrAuctions;
                MeanNrBidsPerAuction = MeanNrBidsPerAuction / nrAuctions;
            }
        } else {
            var allAuctions = _auctionRepository.GetAll();

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

            var auctionsGroupedById = await _bidRepository.GetBidsAuctionsUserAsync(allAuctions);
            Dictionary<Auction, List<Bid>> auctionsGroupedByIdAux = new Dictionary<Auction, List<Bid>>();
            if (auctionsGroupedById!=null && auctionsGroupedById.Any()) {
                foreach (IGrouping<Auction, Bid> g in auctionsGroupedById) {
                    List<Bid> bidsAuction = new List<Bid>();
                    foreach (Bid b in g) {
                        bidsAuction.Add(b);
                    }
                    auctionsGroupedByIdAux[g.Key] = bidsAuction;
                }
            }

            foreach (Auction a in allAuctions) {
                if (!auctionsGroupedByIdAux.ContainsKey(a)) {
                    auctionsGroupedByIdAux[a] = new List<Bid>();
                }
            }

            List<(Auction, List<Bid>)> bidsGroupedByAuction = auctionsGroupedByIdAux.Select(kv => (kv.Key, kv.Value))
                                                                                    .OrderBy(a => a.Key.EndDateTime) // Use OrderBy for sorting
                                                                                    .ToList();

            MeanSellValueAuctions = 0;
            MeanNrBidsPerAuction = 0;
            TotalEarnedAuctions = 0;
            if (bidsGroupedByAuction!=null && bidsGroupedByAuction.Any()) {

                var firstDate = true;
                DateTime maxDate = DateTime.MinValue;
                var checkIfLastDate = 0;
                foreach ((Auction a , List<Bid> bids) g in bidsGroupedByAuction) {

                    if (g.a.EndDateTime<DateTime.Now) {
                        int month = g.a.EndDateTime.Month;
                        int year = g.a.EndDateTime.Year;
                        if (firstDate) {
                            AuctionsPerMonthAdmin[(month, year)] = (1, 0);
                            firstDate = false;
                            maxDate = new DateTime(year, month, 1);
                            maxDate = maxDate.AddMonths(1);
                        } else {
                            if (AuctionsPerMonthAdmin.ContainsKey((month, year))) {
                                var value = AuctionsPerMonthAdmin[(month, year)];
                                value.q += 1;
                                AuctionsPerMonthAdmin[(month, year)] = value;
                            } else {
                                while (maxDate<g.a.EndDateTime) {
                                    AuctionsPerMonthAdmin[(maxDate.Month,maxDate.Year)] = (0, 0);
                                    maxDate = maxDate.AddMonths(1);
                                }
                                if (AuctionsPerMonthAdmin.ContainsKey((month, year))) {
                                    var value = AuctionsPerMonthAdmin[(month, year)];
                                    value.q += 1;
                                    AuctionsPerMonthAdmin[(month, year)] = value;
                                } else {
                                    AuctionsPerMonthAdmin[(month, year)] = (1, 0);
                                }
                            }
                        }

                        checkIfLastDate++;
                        if (checkIfLastDate==bidsGroupedByAuction.Count()) {
                            DateTime presentDate = DateTime.Now;
                            while(presentDate>g.a.EndDateTime) {
                                g.a.EndDateTime = g.a.EndDateTime.AddMonths(1);
                                AuctionsPerMonthAdmin[(g.a.EndDateTime.Month, g.a.EndDateTime.Year)] = (0, 0);
                            }
                        }

                        if (g.a.Autorized && g.a.EndDateTime<DateTime.Now) {

                            MeanNrBidsPerAuction += g.bids.Count();
                            var i = 0;
                            foreach (Bid bid in g.bids) {
                                if (i==0) {
                                    TotalEarnedAuctions += (float) bid.BidValue;
                                    var value = AuctionsPerMonthAdmin[(month, year)];
                                    value.t += (float) bid.BidValue;
                                    AuctionsPerMonthAdmin[(month, year)] = value;
                                }
                                i++;
                            }
                        }
                    }
                }

                var nrAuctions = bidsGroupedByAuction.Count();
                MeanSellValueAuctions = TotalEarnedAuctions / nrAuctions;
                MeanNrBidsPerAuction = MeanNrBidsPerAuction / nrAuctions;
            }
        }
    }

    public void OnPostDeleteAuction(int idAuction) {
        Console.WriteLine("TESTE");
        _auctionRepository.Delete(idAuction);
    }
}
