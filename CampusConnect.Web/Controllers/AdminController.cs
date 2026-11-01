using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IListingRepository _listingRepo;
        private readonly IAccommodationRepository _accommodationRepo;
        private readonly IServiceRepository _serviceRepo;
        private readonly IReportRepository _reportRepo;
        private readonly UserManager<ApplicationUser> _userManager;

        public AdminController(
            IListingRepository listingRepo,
            IAccommodationRepository accommodationRepo,
            IServiceRepository serviceRepo,
            IReportRepository reportRepo,
            UserManager<ApplicationUser> userManager)
        {
            _listingRepo = listingRepo;
            _accommodationRepo = accommodationRepo;
            _serviceRepo = serviceRepo;
            _reportRepo = reportRepo;
            _userManager = userManager;
        }

        public IActionResult Dashboard()
        {
            ViewBag.TotalUsers = _userManager.Users.Count();
            ViewBag.TotalListings = _listingRepo.GetAll().Count();
            ViewBag.TotalAccommodations = _accommodationRepo.GetAll().Count();
            ViewBag.TotalServices = _serviceRepo.GetAll().Count();
            ViewBag.PendingReports = _reportRepo.GetByStatus(ReportStatus.Pending).Count();

            var recentReports = _reportRepo.GetAll().Take(5);
            return View(recentReports);
        }

        public IActionResult Marketplace(string? query, string? status)
        {
            var listings = _listingRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(query))
                listings = listings.Where(l => l.Title.Contains(query, StringComparison.OrdinalIgnoreCase) || (l.Seller != null && l.Seller.FullName.Contains(query, StringComparison.OrdinalIgnoreCase)));

            if (!string.IsNullOrWhiteSpace(status) && status != "all")
            {
                if (Enum.TryParse<ListingStatus>(status, out var statusEnum))
                    listings = listings.Where(l => l.Status == statusEnum);
            }

            ViewBag.Query = query;
            ViewBag.Status = status ?? "all";
            return View(listings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ApproveListing(int id)
        {
            var listing = _listingRepo.GetById(id);
            if (listing != null)
            {
                listing.Status = ListingStatus.Available;
                _listingRepo.Update(listing);
                _listingRepo.Save();
            }
            return RedirectToAction(nameof(Marketplace));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveListing(int id)
        {
            _listingRepo.Delete(id);
            _listingRepo.Save();
            return RedirectToAction(nameof(Marketplace));
        }

        public IActionResult Reports(string? filter)
        {
            var reports = _reportRepo.GetAll();

            if (!string.IsNullOrWhiteSpace(filter) && filter != "all")
            {
                if (Enum.TryParse<ReportStatus>(filter, ignoreCase: true, out var statusEnum))
                    reports = reports.Where(r => r.Status == statusEnum);
            }

            ViewBag.Filter = filter ?? "all";
            return View(reports);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ResolveReport(int id)
        {
            var report = _reportRepo.GetById(id);
            if (report != null)
            {
                report.Status = ReportStatus.Resolved;
                _reportRepo.Update(report);
                _reportRepo.Save();
            }
            return RedirectToAction(nameof(Reports));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DismissReport(int id)
        {
            var report = _reportRepo.GetById(id);
            if (report != null)
            {
                report.Status = ReportStatus.Dismissed;
                _reportRepo.Update(report);
                _reportRepo.Save();
            }
            return RedirectToAction(nameof(Reports));
        }

        public IActionResult Accommodation()
        {
            var accommodations = _accommodationRepo.GetAll();
            return View(accommodations);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveAccommodation(int id)
        {
            _accommodationRepo.Delete(id);
            _accommodationRepo.Save();
            return RedirectToAction(nameof(Accommodation));
        }

        public IActionResult Services()
        {
            var services = _serviceRepo.GetAll();
            return View(services);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RemoveService(int id)
        {
            _serviceRepo.Delete(id);
            _serviceRepo.Save();
            return RedirectToAction(nameof(Services));
        }

        public IActionResult Users()
        {
            var users = _userManager.Users.ToList();
            return View(users);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ToggleVerified(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                user.IsVerified = !user.IsVerified;
                await _userManager.UpdateAsync(user);
            }
            return RedirectToAction(nameof(Users));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> MakeAdmin(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user != null && !await _userManager.IsInRoleAsync(user, "Admin"))
            {
                await _userManager.AddToRoleAsync(user, "Admin");
            }
            return RedirectToAction(nameof(Users));
        }
    }
}
