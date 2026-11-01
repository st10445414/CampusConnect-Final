using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using CampusConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class AccommodationController : Controller
    {
        private readonly IAccommodationRepository _accommodationRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageStorageService _imageStorage;

        public AccommodationController(
            IAccommodationRepository accommodationRepo,
            UserManager<ApplicationUser> userManager,
            IImageStorageService imageStorage)
        {
            _accommodationRepo = accommodationRepo;
            _userManager = userManager;
            _imageStorage = imageStorage;
        }

        [AllowAnonymous]
        public IActionResult Index(string? query, string? type, decimal? maxPrice, bool furnishedOnly = false)
        {
            var results = _accommodationRepo.SearchAccommodation(query, type ?? "All", maxPrice, furnishedOnly);
            ViewBag.Query = query;
            ViewBag.Type = type ?? "All";
            ViewBag.MaxPrice = maxPrice;
            ViewBag.FurnishedOnly = furnishedOnly;
            return View(results);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var accommodation = _accommodationRepo.GetById(id);
            if (accommodation == null) return NotFound();
            return View(accommodation);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Accommodation accommodation, IFormFile? image)
        {
            if (!ModelState.IsValid) return View(accommodation);

            var user = await _userManager.GetUserAsync(User);
            accommodation.PostedById = user?.Id ?? string.Empty;
            accommodation.PostedDate = DateTime.UtcNow;
            accommodation.AvailableFrom = DateTime.UtcNow;

            if (image != null && image.Length > 0)
            {
                accommodation.ImageUrl = await _imageStorage.UploadImageAsync(image, "accommodation") ?? "";
            }

            _accommodationRepo.Add(accommodation);
            _accommodationRepo.Save();
            TempData["Success"] = "Accommodation listing posted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var accommodation = _accommodationRepo.GetById(id);
            if (accommodation != null && accommodation.PostedById == _userManager.GetUserId(User))
            {
                _accommodationRepo.Delete(id);
                _accommodationRepo.Save();
                TempData["Success"] = "Accommodation listing removed.";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}
