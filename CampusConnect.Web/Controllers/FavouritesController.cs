using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class FavouritesController : Controller
    {
        private readonly IFavouriteRepository _favouriteRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public FavouritesController(IFavouriteRepository favouriteRepo, UserManager<ApplicationUser> userManager)
        {
            _favouriteRepo = favouriteRepo;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var userId = _userManager.GetUserId(User);
            var favourites = _favouriteRepo.GetFavouritesByUser(userId);
            return View(favourites);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Toggle(int listingId)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null) return Unauthorized();

            var existing = _favouriteRepo.GetFavourite(userId, listingId);
            if (existing != null)
            {
                _favouriteRepo.Delete(existing.FavouriteId);
                _favouriteRepo.Save();
                TempData["Info"] = "Removed from your saved items.";
            }
            else
            {
                _favouriteRepo.Add(new Favourite
                {
                    UserId = userId,
                    ListingId = listingId,
                    CreatedAt = DateTime.UtcNow
                });
                _favouriteRepo.Save();
                TempData["Success"] = "Added to your saved items.";
            }

            return RedirectToAction("Details", "Marketplace", new { id = listingId });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int favouriteId)
        {
            _favouriteRepo.Delete(favouriteId);
            _favouriteRepo.Save();
            TempData["Info"] = "Removed from your saved items.";
            return RedirectToAction(nameof(Index));
        }
    }
}
