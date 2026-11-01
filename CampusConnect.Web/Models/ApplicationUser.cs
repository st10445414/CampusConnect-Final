using Microsoft.AspNetCore.Identity;

namespace CampusConnect.Web.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = string.Empty;
        public string Faculty { get; set; } = string.Empty;
        public string YearOfStudy { get; set; } = string.Empty;
        public bool IsVerified { get; set; }
        public DateTime JoinedDate { get; set; } = DateTime.UtcNow;
    }
}
