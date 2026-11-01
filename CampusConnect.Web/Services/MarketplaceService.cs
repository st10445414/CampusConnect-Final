using CampusConnect.Web.Models;

namespace CampusConnect.Web.Services
{
    public interface IMarketplaceService
    {
        IEnumerable<Listing> GetRecentListings(int count = 8);
        IEnumerable<Listing> SearchListings(string? query, string? category, string? condition, string sortBy = "newest");
        Listing? GetListingById(int id);
        void CreateListing(Listing listing);
        void UpdateListing(Listing listing);
        void MarkAsSold(int id);
    }

    public class MarketplaceService : IMarketplaceService
    {
        private readonly Repositories.IListingRepository _listingRepo;

        public MarketplaceService(Repositories.IListingRepository listingRepo)
        {
            _listingRepo = listingRepo;
        }

        public IEnumerable<Listing> GetRecentListings(int count = 8)
        {
            return _listingRepo.GetAvailableListings().Take(count);
        }

        public IEnumerable<Listing> SearchListings(string? query, string? category, string? condition, string sortBy = "newest")
        {
            var results = _listingRepo.SearchListings(query, category, condition);

            return sortBy switch
            {
                "price-low" => results.OrderBy(l => l.Price),
                "price-high" => results.OrderByDescending(l => l.Price),
                _ => results.OrderByDescending(l => l.PostedDate)
            };
        }

        public Listing? GetListingById(int id) => _listingRepo.GetListingWithDetails(id);

        public void CreateListing(Listing listing)
        {
            listing.Status = ListingStatus.PendingReview;
            listing.PostedDate = DateTime.UtcNow;
            _listingRepo.Add(listing);
            _listingRepo.Save();
        }

        public void UpdateListing(Listing listing)
        {
            _listingRepo.Update(listing);
            _listingRepo.Save();
        }

        public void MarkAsSold(int id)
        {
            var listing = _listingRepo.GetById(id);
            if (listing != null)
            {
                listing.Status = ListingStatus.Sold;
                _listingRepo.Update(listing);
                _listingRepo.Save();
            }
        }
    }
}
