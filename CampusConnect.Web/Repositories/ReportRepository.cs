using CampusConnect.Web.Data;
using CampusConnect.Web.Models;

namespace CampusConnect.Web.Repositories
{
    public interface IReportRepository : IRepository<Report>
    {
        IEnumerable<Report> GetByStatus(ReportStatus status);
    }

    public class ReportRepository : Repository<Report>, IReportRepository
    {
        public ReportRepository(ApplicationDbContext context) : base(context) { }

        public IEnumerable<Report> GetByStatus(ReportStatus status)
        {
            return _dbSet.Where(r => r.Status == status).OrderByDescending(r => r.DateReported).ToList();
        }
    }
}
