using System.ComponentModel.DataAnnotations;

namespace CampusConnect.Web.Models
{
    public class Conversation
    {
        public int ConversationId { get; set; }

        public string User1Id { get; set; } = string.Empty;
        public ApplicationUser? User1 { get; set; }

        public string User2Id { get; set; } = string.Empty;
        public ApplicationUser? User2 { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

    public class Message
    {
        public int MessageId { get; set; }

        public int ConversationId { get; set; }
        public Conversation? Conversation { get; set; }

        public string SenderId { get; set; } = string.Empty;
        public ApplicationUser? Sender { get; set; }

        [Required]
        [StringLength(2000)]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;
    }
}
