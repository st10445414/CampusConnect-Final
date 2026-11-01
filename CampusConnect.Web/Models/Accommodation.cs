using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Web.Models
{
    public class Accommodation
    {
        public int AccommodationId { get; set; }

        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(30)]
        public string Type { get; set; } = string.Empty;

        [Range(0, 999999)]
        public decimal RentPerMonth { get; set; }

        [StringLength(100)]
        public string Location { get; set; } = string.Empty;

        public int Bedrooms { get; set; }

        public int Bathrooms { get; set; }

        public bool IsFurnished { get; set; }

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(500)]
        public string ImageUrl { get; set; } = string.Empty;

        public DateTime AvailableFrom { get; set; } = DateTime.UtcNow;

        public DateTime PostedDate { get; set; } = DateTime.UtcNow;

        public string PostedById { get; set; } = string.Empty;
        public ApplicationUser? PostedBy { get; set; }

        [StringLength(256)]
        public string ContactEmail { get; set; } = string.Empty;
    }
}
