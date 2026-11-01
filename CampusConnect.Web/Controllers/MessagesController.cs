using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class MessagesController : Controller
    {
        private readonly IConversationRepository _conversationRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesController(IConversationRepository conversationRepo, UserManager<ApplicationUser> userManager)
        {
            _conversationRepo = conversationRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var conversations = _conversationRepo.GetConversationsForUser(userId);
            ViewBag.UnreadCount = _conversationRepo.GetUnreadCount(userId);
            return View(conversations);
        }

        public IActionResult Conversation(int id)
        {
            var userId = _userManager.GetUserId(User);
            var conversation = _conversationRepo.GetConversationById(id);
            if (conversation == null) return NotFound();

            if (conversation.User1Id != userId && conversation.User2Id != userId)
                return Forbid();

            var messages = _conversationRepo.GetMessages(id);
            _conversationRepo.MarkMessagesAsRead(id, userId);

            ViewBag.CurrentUserId = userId;
            ViewBag.ConversationId = id;
            var otherUser = conversation.User1Id == userId ? conversation.User2 : conversation.User1;
            ViewBag.OtherUserName = otherUser?.FullName ?? "Unknown";

            return View(messages);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StartConversation(string recipientId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == recipientId)
            {
                TempData["Error"] = "You can't start a conversation with yourself.";
                return RedirectToAction(nameof(Index));
            }

            var conversation = _conversationRepo.GetOrCreateConversation(userId, recipientId);
            if (conversation == null)
            {
                TempData["Error"] = "Unable to start conversation.";
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Conversation), new { id = conversation.ConversationId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult SendMessage(int conversationId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return RedirectToAction(nameof(Conversation), new { id = conversationId });

            var userId = _userManager.GetUserId(User);
            var conversation = _conversationRepo.GetConversationById(conversationId);
            if (conversation == null) return NotFound();

            if (conversation.User1Id != userId && conversation.User2Id != userId)
                return Forbid();

            var message = new Message
            {
                ConversationId = conversationId,
                SenderId = userId,
                Content = content.Trim(),
                SentAt = DateTime.UtcNow,
                IsRead = false
            };

            _conversationRepo.AddMessage(message);
            return RedirectToAction(nameof(Conversation), new { id = conversationId });
        }

        [HttpGet]
        public async Task<IActionResult> UnreadCount()
        {
            var userId = _userManager.GetUserId(User);
            var count = _conversationRepo.GetUnreadCount(userId);
            return Json(new { count });
        }
    }
}
