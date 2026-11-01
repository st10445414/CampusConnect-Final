using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Web.Models
{
    public class Service
    {
        public int ServiceId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(50)]
        public string Price { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public string ProviderId { get; set; } = string.Empty;
        public ApplicationUser? Provider { get; set; }

        public ICollection<Review> Reviews { get; set; } = new List<Review>();

        public double GetAverageRating()
        {
            return Reviews.Count > 0 ? Reviews.Average(r => r.Rating) : 0;
        }
    }

    public class Review
    {
        public int ReviewId { get; set; }

        [Range(1, 5)]
        public int Rating { get; set; }

        [StringLength(1000)]
        public string Comment { get; set; } = string.Empty;

        public DateTime DatePosted { get; set; } = DateTime.UtcNow;

        public string ReviewerId { get; set; } = string.Empty;
        public ApplicationUser? Reviewer { get; set; }

        public int ServiceId { get; set; }
        public Service? Service { get; set; }

        public int? ListingId { get; set; }
        public Listing? Listing { get; set; }
    }
}
