using LIForCars.Models;
using LIForCars.Data.Interfaces;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace LIForCars.Data.Components
{
    public class AuctionRepository : IAuctionRepository
    {
        private readonly MyLIForCarsDBContext _context;

        public AuctionRepository(MyLIForCarsDBContext context)
        {
            _context = context;
        }

        public IEnumerable<Auction> GetAll()
        {
            return _context.Auction.ToList();
        }

        public Auction? GetById(int id)
        {
            return _context.Auction.FirstOrDefault(a => a.Id == id);
        }

        public bool Create(Auction auction)
        {
            try 
            {
                _context.Auction.Add(auction);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool Update(Auction auction)
        {
            try {
                _context.Auction.Update(auction);
                _context.SaveChanges();
                return true;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try 
            {
                var auction = _context.Auction.Find(id);
                if (auction != null)
                {
                    _context.Auction.Remove(auction);
                    _context.SaveChanges();
                    return true;
                }
                return false;
            }
            catch (DbUpdateException)
            {
                return false;
            }
        }

        public bool CarIdExists(int carId) => _context.Auction.Any(a => a.CarId == carId);

        public async Task<Auction?> GetAuctionAsync(int idAuction) {
            var query = _context.Auction
                                .Include(a => a.Car)
                                .FirstOrDefault(a => a.Id == idAuction);

            return query;
        }

        public async Task<(IEnumerable<Auction> auctions, int totalCount)> GetCurrentAuctionsAsync(int page, int pageSize, string orderBy, string filterBy)
        {
            var query = _context.Auction
                .Include(a => a.Car)
                .Where(a => a.Autorized && a.InitDateTime <= DateTime.Now && a.EndDateTime >= DateTime.Now);

            IEnumerable<Auction> auctions = Enumerable.Empty<Auction>();
            if (orderBy=="RemainingTimeDescending") {
                if (filterBy=="") {
                    auctions = await query.OrderByDescending(a => a.EndDateTime)
                                                            .Skip((page - 1) * pageSize)
                                                            .Take(pageSize)
                                                            .ToListAsync();
                } else {
                    var separarFiltro = filterBy.Split(':');
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    var filtro = textInfo.ToTitleCase(separarFiltro[0].ToLower());
                    filtro = filtro.Trim();

                    switch (filtro) {
                        case "Make":
                            var makeName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Make == makeName)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Model":
                            var modelName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Model == modelName)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Year":
                            var yearValue = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.LaunchYear == int.Parse(yearValue))
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Origin":
                            var originName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Origin == originName)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Motor":
                            var motorName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Motor == motorName)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Status":
                            var statusName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.StatusDescription == statusName)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Kms":
                            var limitesKms = separarFiltro[1].Trim().Split("-");
                            var minKmsString = limitesKms[0].Trim();
                            var maxKmsString = limitesKms[1].Trim();
                            var minKms = int.Parse(minKmsString);
                            var maxKms = int.Parse(maxKmsString);
                            auctions = await query.Where(a => a.Car!= null && a.Car.Kms >= minKms && a.Car.Kms <= maxKms)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Base price":
                            var limitesBasePrice = separarFiltro[1].Trim().Split("-");
                            var minBasePriceString = limitesBasePrice[0].Trim();
                            var maxBasePriceString = limitesBasePrice[1].Trim();
                            var minBasePrice = int.Parse(minBasePriceString);
                            var maxBasePrice = int.Parse(maxBasePriceString);
                            auctions = await query.Where(a => a.BasePrice >= minBasePrice && a.BasePrice <= maxBasePrice)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Price":
                            var limitesPrice = separarFiltro[1].Trim().Split("-");
                            var minPriceString = limitesPrice[0].Trim();
                            var maxPriceString = limitesPrice[1].Trim();
                            var minPrice = int.Parse(minPriceString);
                            var maxPrice = int.Parse(maxPriceString);
                            auctions = await query.Where(a => a.BuyNowPrice >= minPrice && a.BuyNowPrice <= maxPrice)
                                                  .OrderByDescending(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                    }
                }
            } else {
                if (string.IsNullOrEmpty(filterBy)) {
                    auctions = await query.OrderBy(a => a.EndDateTime)
                                                            .Skip((page - 1) * pageSize)
                                                            .Take(pageSize)
                                                            .ToListAsync();
                } else {
                    var separarFiltro = filterBy.Split(':');
                    TextInfo textInfo = CultureInfo.CurrentCulture.TextInfo;
                    var filtro = textInfo.ToTitleCase(separarFiltro[0].ToLower());
                    filtro = filtro.Trim();

                    switch (filtro) {
                        case "Make":
                            var makeName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Make == makeName)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Model":
                            var modelName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Model == modelName)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Year":
                            var yearValue = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.LaunchYear == int.Parse(yearValue))
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Origin":
                            var originName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Origin == originName)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Motor":
                            var motorName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.Motor == motorName)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Status":
                            var statusName = separarFiltro[1].Trim();
                            auctions = await query.Where(a => a.Car!= null && a.Car.StatusDescription == statusName)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Kms":
                            var limitesKms = separarFiltro[1].Trim().Split("-");
                            var minKmsString = limitesKms[0].Trim();
                            var maxKmsString = limitesKms[1].Trim();
                            var minKms = int.Parse(minKmsString);
                            var maxKms = int.Parse(maxKmsString);
                            auctions = await query.Where(a => a.Car!= null && a.Car.Kms >= minKms && a.Car.Kms <= maxKms)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Base price":
                            var limitesBasePrice = separarFiltro[1].Trim().Split("-");
                            var minBasePriceString = limitesBasePrice[0].Trim();
                            var maxBasePriceString = limitesBasePrice[1].Trim();
                            var minBasePrice = int.Parse(minBasePriceString);
                            var maxBasePrice = int.Parse(maxBasePriceString);
                            auctions = await query.Where(a => a.BasePrice >= minBasePrice && a.BasePrice <= maxBasePrice)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                        case "Price":
                            var limitesPrice = separarFiltro[1].Trim().Split("-");
                            var minPriceString = limitesPrice[0].Trim();
                            var maxPriceString = limitesPrice[1].Trim();
                            var minPrice = int.Parse(minPriceString);
                            var maxPrice = int.Parse(maxPriceString);
                            auctions = await query.Where(a => a.BuyNowPrice >= minPrice && a.BuyNowPrice <= maxPrice)
                                                  .OrderBy(a => a.EndDateTime)
                                                  .Skip((page - 1) * pageSize)
                                                  .Take(pageSize)
                                                  .ToListAsync();
                            break;
                    }
                }
            }

            var totalCount = auctions.Count();

            return (auctions, totalCount);
        }

        public async Task<(IEnumerable<Auction> auctions, int totalCount)> GetAuctionsUserAsync(int page, int pageSize, int idUser)
        {
            var query = _context.Auction
                .Include(a => a.Car)
                .Where(a => a.Autorized && a.UserId == idUser);

            var totalCount = await query.CountAsync();
            var auctions = await query.OrderBy(a => a.InitDateTime)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            return (auctions, totalCount);
        }

        public async Task<IEnumerable<Auction>> GetAuctionsUserWaitingApprovalAsync(int page, int pageSize, int idUser)
        {
            var query = _context.Auction
                .Include(a => a.Car)
                .Where(a => !a.Autorized && a.UserId == idUser);

            var auctions = await query.OrderBy(a => a.InitDateTime)
                                      .Skip((page - 1) * pageSize)
                                      .Take(pageSize)
                                      .ToListAsync();

            return auctions;
        }

        public async Task<IEnumerable<Auction>> GetAllAuctionsUserAsync(int idUser)
        {
            var query = await _context.Auction
                .Where(a => a.UserId == idUser)
                .ToListAsync();

            return query;
        }
    }
}
