using CampusConnect.Web.Data;
using CampusConnect.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Web.Repositories
{
    public interface IServiceRepository : IRepository<Service>
    {
        IEnumerable<Service> SearchServices(string? query, string? category);
        void AddReview(Review review);
        Service? GetServiceWithDetails(int id);
    }

    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<Service> SearchServices(string? query, string? category)
        {
            var results = _dbSet.Include(s => s.Reviews).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query))
                results = results.Where(s => s.Title.Contains(query) || s.Provider.FullName.Contains(query));

            if (!string.IsNullOrWhiteSpace(category) && category != "All")
                results = results.Where(s => s.Category == category);

            return results.OrderByDescending(s => s.PostedDate).ToList();
        }

        public void AddReview(Review review)
        {
            _context.Reviews.Add(review);
        }

        public Service? GetServiceWithDetails(int id)
        {
            return _dbSet
                .Include(s => s.Provider)
                .Include(s => s.Reviews)
                    .ThenInclude(r => r.Reviewer)
                .FirstOrDefault(s => s.ServiceId == id);
        }
    }
}
