using System;
using System.Threading.Tasks;
using LIForCars.Data.Interfaces;
using LIForCars.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class AuctionPageModel : PageModel
{
    private readonly IAuctionRepository _auctionRepository;
    private readonly IUserRepository _userRepository;

    public AuctionPageModel(IAuctionRepository auctionRepository, IUserRepository userRepository)
    {
        _auctionRepository = auctionRepository;
        _userRepository = userRepository;
    }

    [BindProperty(SupportsGet = true)]
    public int AuctionId { get; set; }

    public Auction Auction { get; private set; }
    public User User { get; private set; }

    public async Task OnGetAsync()
    {
        Auction = await _auctionRepository.GetAuctionAsync(AuctionId);
        if (Auction != null)
        {
            User = await _userRepository.GetUserByIdAsync(Auction.UserId);
        }
        else
        {
            throw new Exception("Auction not found");
        }
    }
}
