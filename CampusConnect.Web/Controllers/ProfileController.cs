using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IListingRepository _listingRepo;
        private readonly IServiceRepository _serviceRepo;

        public ProfileController(UserManager<ApplicationUser> userManager, IListingRepository listingRepo, IServiceRepository serviceRepo)
        {
            _userManager = userManager;
            _listingRepo = listingRepo;
            _serviceRepo = serviceRepo;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            var listings = _listingRepo.GetListingsBySeller(user.Id);
            var services = _serviceRepo.Find(s => s.ProviderId == user.Id);
            var roles = await _userManager.GetRolesAsync(user);

            ViewBag.Listings = listings;
            ViewBag.Services = services;
            ViewBag.Roles = roles;
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(ApplicationUser model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null) return NotFound();

            user.FullName = model.FullName;
            user.Faculty = model.Faculty;
            user.YearOfStudy = model.YearOfStudy;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                TempData["Success"] = "Profile updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            foreach (var error in result.Errors)
                ModelState.AddModelError(string.Empty, error.Description);
            return View(user);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Report(int targetId, string targetType, string reason)
        {
            var report = new Report
            {
                TargetType = targetType,
                TargetId = targetId,
                Reason = reason,
                ReportedById = _userManager.GetUserId(User),
                DateReported = DateTime.UtcNow,
                Status = ReportStatus.Pending
            };

            var reportRepo = HttpContext.RequestServices.GetService<IReportRepository>();
            if (reportRepo != null)
            {
                reportRepo.Add(report);
                reportRepo.Save();
            }
            TempData["Success"] = "Report submitted. An admin will review it shortly.";
            return RedirectToAction(nameof(Index));
        }
    }
}
