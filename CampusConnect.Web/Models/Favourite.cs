using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Web.Models
{
    public class Favourite
    {
        public int FavouriteId { get; set; }

        public string UserId { get; set; } = string.Empty;
        public ApplicationUser? User { get; set; }

        public int ListingId { get; set; }
        public Listing? Listing { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
