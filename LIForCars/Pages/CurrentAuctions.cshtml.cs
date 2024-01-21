using System.Collections.Generic;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Threading.Tasks;

public class CurrentAuctionsModel : PageModel
{
    private readonly IAuctionRepository _auctionRepository;

    public CurrentAuctionsModel(IAuctionRepository auctionRepository)
    {
        _auctionRepository = auctionRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    [BindProperty(SupportsGet = true)]
    public int PageSize { get; set; } = 1;
    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();
    public int TotalCount { get; private set; }

    public async Task OnGetAsync()
    {
        var result = await _auctionRepository.GetCurrentAuctionsAsync(CurrentPage, PageSize);
        Auctions = result.auctions;
        TotalCount = result.totalCount;
    }
}