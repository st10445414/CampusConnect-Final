using CampusConnect.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly Services.IMarketplaceService _marketplaceService;

        public HomeController(Services.IMarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        [AllowAnonymous]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashboard()
        {
            var recentListings = _marketplaceService.GetRecentListings(4);
            return View(recentListings);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error");
        }
    }
}
