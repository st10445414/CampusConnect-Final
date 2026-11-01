using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using CampusConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class MarketplaceController : Controller
    {
        private readonly Services.IMarketplaceService _marketplaceService;
        private readonly IReportRepository _reportRepo;
        private readonly IFavouriteRepository _favouriteRepo;
        private readonly IImageStorageService _imageStorage;
        private readonly UserManager<ApplicationUser> _userManager;

        public MarketplaceController(
            Services.IMarketplaceService marketplaceService,
            IReportRepository reportRepo,
            IFavouriteRepository favouriteRepo,
            IImageStorageService imageStorage,
            UserManager<ApplicationUser> userManager)
        {
            _marketplaceService = marketplaceService;
            _reportRepo = reportRepo;
            _favouriteRepo = favouriteRepo;
            _imageStorage = imageStorage;
            _userManager = userManager;
        }

        [AllowAnonymous]
        public IActionResult Index(string? query, string? category, string? condition, string sortBy = "newest")
        {
            var listings = _marketplaceService.SearchListings(query, category, condition, sortBy);
            ViewBag.Query = query;
            ViewBag.Category = category ?? "All";
            ViewBag.Condition = condition ?? "All";
            ViewBag.SortBy = sortBy;
            return View(listings);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var listing = _marketplaceService.GetListingById(id);
            if (listing == null) return NotFound();

            var userId = _userManager.GetUserId(User);
            if (userId != null)
                ViewBag.IsFavourite = _favouriteRepo.IsFavourite(userId, id);
            else
                ViewBag.IsFavourite = false;

            return View(listing);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Listing listing, List<IFormFile>? images)
        {
            if (!ModelState.IsValid) return View(listing);

            listing.SellerId = _userManager.GetUserId(User);
            listing.PostedDate = DateTime.UtcNow;

            _marketplaceService.CreateListing(listing);

            if (images != null && images.Count > 0)
            {
                var validImages = images.Take(5).ToList();
                foreach (var image in validImages)
                {
                    var url = await _imageStorage.UploadImageAsync(image, "listings");
                    if (!string.IsNullOrEmpty(url))
                    {
                        listing.Images.Add(new ListingImage { Url = url, ListingId = listing.ListingId });
                    }
                }
                _marketplaceService.UpdateListing(listing);
            }

            TempData["Success"] = "Listing posted! It will appear once approved by an admin.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult MarkSold(int id)
        {
            _marketplaceService.MarkAsSold(id);
            return RedirectToAction(nameof(Details), new { id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Report(int id, string reason)
        {
            var listing = _marketplaceService.GetListingById(id);
            if (listing == null) return NotFound();

            var report = new Report
            {
                TargetType = "Listing",
                TargetId = id,
                Reason = reason,
                ReportedById = _userManager.GetUserId(User),
                DateReported = DateTime.UtcNow,
                Status = ReportStatus.Pending
            };
            _reportRepo.Add(report);
            _reportRepo.Save();
            TempData["Success"] = "Listing reported. An admin will review it shortly.";
            return RedirectToAction(nameof(Details), new { id });
        }
    }
}
