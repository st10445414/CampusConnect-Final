using CampusConnect.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Web.Repositories
{
    public interface IListingRepository : IRepository<Listing>
    {
        IEnumerable<Listing> GetAvailableListings();
        IEnumerable<Listing> SearchListings(string? query, string? category, string? condition);
        IEnumerable<Listing> GetListingsBySeller(string sellerId);
        Listing? GetListingWithDetails(int id);
    }

    public class ListingRepository : Repository<Listing>, IListingRepository
    {
        public ListingRepository(Data.ApplicationDbContext context) : base(context) { }

        public IEnumerable<Listing> GetAvailableListings()
        {
            return _dbSet
                .Include(l => l.Seller)
                .Include(l => l.Images)
                .Where(l => l.Status == ListingStatus.Available)
                .OrderByDescending(l => l.PostedDate)
                .ToList();
        }

        public IEnumerable<Listing> SearchListings(string? query, string? category, string? condition)
        {
            var results = _dbSet
                .Include(l => l.Seller)
                .Include(l => l.Images)
                .Where(l => l.Status == ListingStatus.Available);

            if (!string.IsNullOrWhiteSpace(query))
                results = results.Where(l => l.Title.Contains(query) || l.Description.Contains(query));

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
                results = results.Where(l => l.Category == category);

            if (!string.IsNullOrWhiteSpace(condition) && condition != "All")
                results = results.Where(l => l.Condition == condition);

            return results.OrderByDescending(l => l.PostedDate).ToList();
        }

        public IEnumerable<Listing> GetListingsBySeller(string sellerId)
        {
            return _dbSet
                .Include(l => l.Images)
                .Where(l => l.SellerId == sellerId).ToList();
        }

        public Listing? GetListingWithDetails(int id)
        {
            return _dbSet
                .Include(l => l.Seller)
                .Include(l => l.Images)
                .FirstOrDefault(l => l.ListingId == id);
        }
    }
}
