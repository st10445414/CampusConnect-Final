using CampusConnect.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Web.Repositories
{
    public interface IFavouriteRepository : IRepository<Favourite>
    {
        IEnumerable<Favourite> GetFavouritesByUser(string userId);
        Favourite? GetFavourite(string userId, int listingId);
        bool IsFavourite(string userId, int listingId);
    }

    public class FavouriteRepository : Repository<Favourite>, IFavouriteRepository
    {
        public FavouriteRepository(Data.ApplicationDbContext context) : base(context) { }

        public IEnumerable<Favourite> GetFavouritesByUser(string userId)
        {
            return _context.Favourites
                .Include(f => f.Listing!)
                    .ThenInclude(l => l.Seller)
                .Include(f => f.Listing!.Images)
                .Where(f => f.UserId == userId)
                .OrderByDescending(f => f.CreatedAt)
                .ToList();
        }

        public Favourite? GetFavourite(string userId, int listingId)
        {
            return _context.Favourites
                .FirstOrDefault(f => f.UserId == userId && f.ListingId == listingId);
        }

        public bool IsFavourite(string userId, int listingId)
        {
            return _context.Favourites.Any(f => f.UserId == userId && f.ListingId == listingId);
        }
    }
}
