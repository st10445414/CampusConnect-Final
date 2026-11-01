using Microsoft.AspNetCore.SignalR;

namespace CampusConnect.Web.Hubs
{
    public class ChatHub : Hub
    {
        public async Task JoinConversation(int conversationId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, $"conversation-{conversationId}");
        }

        public async Task LeaveConversation(int conversationId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"conversation-{conversationId}");
        }

        public async Task SendMessage(int conversationId, string senderId, string senderName, string content)
        {
            await Clients.Group($"conversation-{conversationId}")
                .SendAsync("ReceiveMessage", new
                {
                    conversationId,
                    senderId,
                    senderName,
                    content,
                    sentAt = DateTime.UtcNow.ToString("o")
                });
        }

        public async Task MarkAsRead(int conversationId, string readerId)
        {
            await Clients.Group($"conversation-{conversationId}")
                .SendAsync("MessagesRead", new { conversationId, readerId });
        }
    }
}
