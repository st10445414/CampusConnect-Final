using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Web.Models
{
    public enum ListingStatus
    {
        Draft,
        PendingReview,
        Available,
        PendingSale,
        Sold,
        Removed
    }

    public class Listing
    {
        public int ListingId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        [Range(0, 999999)]
        public decimal Price { get; set; }

        [Required]
        [StringLength(50)]
        public string Category { get; set; } = string.Empty;

        [StringLength(30)]
        public string Condition { get; set; } = string.Empty;

        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public ListingStatus Status { get; set; } = ListingStatus.PendingReview;

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public string SellerId { get; set; } = string.Empty;
        public ApplicationUser? Seller { get; set; }

        public ICollection<ListingImage> Images { get; set; } = new List<ListingImage>();
    }

    public class ListingImage
    {
        public int ImageId { get; set; }
        public string Url { get; set; } = string.Empty;
        public DateTime UploadedAt { get; set; } = DateTime.UtcNow;

        public int ListingId { get; set; }
        public Listing? Listing { get; set; }
    }
}
