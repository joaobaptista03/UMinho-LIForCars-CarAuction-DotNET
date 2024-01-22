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

    public UserPageModel(IUserRepository userRepository, IAuctionRepository auctionRepository)
    {
        _userRepository = userRepository;
        _auctionRepository = auctionRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int CurrentPage { get; set; } = 1;

    public new User? User { get; private set; }
    public int PageSize { get; set; } = 10;
    public int TotalCount { get; private set; }
    public IEnumerable<Auction> Auctions { get; private set; } = Enumerable.Empty<Auction>();


    public async Task OnGetAsync(int UserId)
    {
        if (UserId <= 0)
        {
            Console.WriteLine(UserId);
            return;
        }
        User = await _userRepository.GetUserByIdAsync(UserId);
        var result = await _auctionRepository.GetAuctionsUserAsync(CurrentPage, PageSize, UserId);
        Console.WriteLine("Result -> " + result.totalCount);
        foreach (Auction a in result.auctions) {
            Console.WriteLine("Auction -> " + a);
        }
        Auctions = result.auctions;
        TotalCount = result.totalCount;
    }
}
