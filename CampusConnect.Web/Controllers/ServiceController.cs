using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using CampusConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class ServiceController : Controller
    {
        private readonly IServiceRepository _serviceRepo;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IImageStorageService _imageStorage;

        public ServiceController(
            IServiceRepository serviceRepo,
            UserManager<ApplicationUser> userManager,
            IImageStorageService imageStorage)
        {
            _serviceRepo = serviceRepo;
            _userManager = userManager;
            _imageStorage = imageStorage;
        }

        [AllowAnonymous]
        public IActionResult Index(string? query, string? category)
        {
            var results = _serviceRepo.SearchServices(query, category ?? "All");
            ViewBag.Query = query;
            ViewBag.Category = category ?? "All";
            return View(results);
        }

        [AllowAnonymous]
        public IActionResult Details(int id)
        {
            var service = _serviceRepo.GetServiceWithDetails(id);
            if (service == null) return NotFound();
            return View(service);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Service service, IFormFile? image)
        {
            if (!ModelState.IsValid) return View(service);

            var user = await _userManager.GetUserAsync(User);
            service.ProviderId = user?.Id ?? string.Empty;
            service.PostedDate = DateTime.UtcNow;

            if (image != null && image.Length > 0)
            {
                service.ImageUrl = await _imageStorage.UploadImageAsync(image, "services") ?? "";
            }

            _serviceRepo.Add(service);
            _serviceRepo.Save();
            TempData["Success"] = "Service listing posted successfully.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var service = _serviceRepo.GetById(id);
            if (service != null && service.ProviderId == _userManager.GetUserId(User))
            {
                _serviceRepo.Delete(id);
                _serviceRepo.Save();
                TempData["Success"] = "Service listing removed.";
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddReview(int serviceId, int rating, string comment)
        {
            if (rating < 1 || rating > 5)
            {
                ModelState.AddModelError("rating", "Rating must be between 1 and 5.");
                return RedirectToAction(nameof(Details), new { id = serviceId });
            }

            var service = _serviceRepo.GetById(serviceId);
            if (service == null) return NotFound();

            var review = new Review
            {
                Rating = rating,
                Comment = comment,
                ReviewerId = _userManager.GetUserId(User),
                ServiceId = serviceId,
                DatePosted = DateTime.UtcNow
            };

            _serviceRepo.AddReview(review);
            _serviceRepo.Save();
            TempData["Success"] = "Review posted.";
            return RedirectToAction(nameof(Details), new { id = serviceId });
        }
    }
}
