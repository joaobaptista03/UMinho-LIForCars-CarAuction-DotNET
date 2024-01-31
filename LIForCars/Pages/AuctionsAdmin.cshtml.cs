using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;
using Microsoft.VisualBasic;

public class AuctionsAdminModel : PageModel
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IBidRepository _bidRepository;

    public AuctionsAdminModel (IAuctionRepository auctionRepository, IBidRepository bidRepository)
    {
        _auctionRepository = auctionRepository;
        _bidRepository = bidRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; private set; }

    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();


    public async Task OnGetAsync()
    {
        var result = await _auctionRepository.GetAllWaittingAuctionsAsync(CurrentPage, PageSize);
        Auctions = result.auctions;
        TotalCount = result.totalCount;
    }

    public void OnPostRemoveAuction(int idAuction) {
        _auctionRepository.Delete(idAuction);
    }

    public void OnPostAcceptAuction(int idAuction) {
        _auctionRepository.UpdateAuthorizationStatus(idAuction);
    }
}
