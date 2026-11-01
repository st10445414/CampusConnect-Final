using CampusConnect.Web.Data;
using CampusConnect.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Web.Repositories
{
    public interface IAccommodationRepository : IRepository<Accommodation>
    {
        IEnumerable<Accommodation> SearchAccommodation(string? query, string? type, decimal? maxPrice, bool furnishedOnly);
    }

    public class AccommodationRepository : Repository<Accommodation>, IAccommodationRepository
    {
        public AccommodationRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<Accommodation> SearchAccommodation(string? query, string? type, decimal? maxPrice, bool furnishedOnly)
        {
            var results = _dbSet.AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                results = results.Where(a => a.Title.Contains(query) || a.Location.Contains(query));

            if (!string.IsNullOrWhiteSpace(type) && type != "All")
                results = results.Where(a => a.Type == type);

            if (maxPrice.HasValue)
                results = results.Where(a => a.RentPerMonth <= maxPrice.Value);

            if (furnishedOnly)
                results = results.Where(a => a.IsFurnished);

            return results.OrderByDescending(a => a.PostedDate).ToList();
        }
    }
}
