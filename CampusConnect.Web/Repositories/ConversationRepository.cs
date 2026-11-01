using CampusConnect.Web.Models;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Web.Repositories
{
    public interface IConversationRepository : IRepository<Conversation>
    {
        Conversation? GetOrCreateConversation(string user1Id, string user2Id);
        Conversation? GetConversationById(int id);
        IEnumerable<Conversation> GetConversationsForUser(string userId);
        IEnumerable<Message> GetMessages(int conversationId);
        void AddMessage(Message message);
        void MarkMessagesAsRead(int conversationId, string readerId);
        int GetUnreadCount(string userId);
    }

    public class ConversationRepository : Repository<Conversation>, IConversationRepository
    {
        public ConversationRepository(Data.ApplicationDbContext context) : base(context) { }

        public Conversation? GetOrCreateConversation(string user1Id, string user2Id)
        {
            if (user1Id == user2Id) return null;

            var existing = _context.Conversations
                .Include(c => c.Messages)
                .FirstOrDefault(c =>
                    (c.User1Id == user1Id && c.User2Id == user2Id) ||
                    (c.User1Id == user2Id && c.User2Id == user1Id));

            if (existing != null) return existing;

            var sorted = string.Compare(user1Id, user2Id, StringComparison.Ordinal) < 0;
            var conversation = new Conversation
            {
                User1Id = sorted ? user1Id : user2Id,
                User2Id = sorted ? user2Id : user1Id,
                CreatedAt = DateTime.UtcNow
            };

            _context.Conversations.Add(conversation);
            _context.SaveChanges();

            return conversation;
        }

        public Conversation? GetConversationById(int id)
        {
            return _context.Conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .FirstOrDefault(c => c.ConversationId == id);
        }

        public IEnumerable<Conversation> GetConversationsForUser(string userId)
        {
            return _context.Conversations
                .Include(c => c.User1)
                .Include(c => c.User2)
                .Include(c => c.Messages)
                .Where(c => c.User1Id == userId || c.User2Id == userId)
                .OrderByDescending(c => c.Messages.Any()
                    ? c.Messages.Max(m => m.SentAt)
                    : c.CreatedAt)
                .ToList();
        }

        public IEnumerable<Message> GetMessages(int conversationId)
        {
            return _context.Messages
                .Include(m => m.Sender)
                .Where(m => m.ConversationId == conversationId)
                .OrderBy(m => m.SentAt)
                .ToList();
        }

        public void AddMessage(Message message)
        {
            _context.Messages.Add(message);
            _context.SaveChanges();
        }

        public void MarkMessagesAsRead(int conversationId, string readerId)
        {
            var unread = _context.Messages
                .Where(m => m.ConversationId == conversationId && m.SenderId != readerId && !m.IsRead)
                .ToList();

            foreach (var msg in unread)
                msg.IsRead = true;

            _context.SaveChanges();
        }

        public int GetUnreadCount(string userId)
        {
            return _context.Messages
                .Include(m => m.Conversation)
                .Where(m => m.SenderId != userId && !m.IsRead &&
                            (m.Conversation!.User1Id == userId || m.Conversation.User2Id == userId))
                .Count();
        }
    }
}
