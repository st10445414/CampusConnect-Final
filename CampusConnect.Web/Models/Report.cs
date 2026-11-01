using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Web.Models
{
    public enum ReportStatus
    {
        Pending,
        Resolved,
        Dismissed
    }

    public class Report
    {
        public int ReportId { get; set; }

        [StringLength(30)]
        public string TargetType { get; set; } = string.Empty;

        public int TargetId { get; set; }

        [StringLength(500)]
        public string Reason { get; set; } = string.Empty;

        public ReportStatus Status { get; set; } = ReportStatus.Pending;

        public DateTime DateReported { get; set; } = DateTime.UtcNow;

        public string ReportedById { get; set; } = string.Empty;
        public ApplicationUser? ReportedBy { get; set; }

        [StringLength(1000)]
        public string AdminNotes { get; set; } = string.Empty;
    }
}
