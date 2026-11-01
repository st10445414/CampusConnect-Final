using CampusConnect.Web.Models;
using CampusConnect.Web.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CampusConnect.Web.Controllers.Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarketplaceApiController : ControllerBase
    {
        private readonly IMarketplaceService _marketplaceService;

        public MarketplaceApiController(IMarketplaceService marketplaceService)
        {
            _marketplaceService = marketplaceService;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult GetListings([FromQuery] string? query, [FromQuery] string? category, [FromQuery] string? condition, [FromQuery] string sortBy = "newest")
        {
            var listings = _marketplaceService.SearchListings(query, category, condition, sortBy);
            var dto = listings.Select(l => new
            {
                id = l.ListingId,
                title = l.Title,
                price = l.Price,
                category = l.Category,
                condition = l.Condition,
                location = l.Location,
                status = l.Status.ToString(),
                postedDate = l.PostedDate,
                seller = l.Seller?.FullName ?? "Unknown"
            });
            return Ok(dto);
        }

        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetListing(int id)
        {
            var listing = _marketplaceService.GetListingById(id);
            if (listing == null) return NotFound(new { error = "Listing not found." });

            var dto = new
            {
                id = listing.ListingId,
                title = listing.Title,
                description = listing.Description,
                price = listing.Price,
                category = listing.Category,
                condition = listing.Condition,
                location = listing.Location,
                status = listing.Status.ToString(),
                postedDate = listing.PostedDate,
                seller = listing.Seller?.FullName ?? "Unknown",
                sellerEmail = listing.Seller?.Email ?? ""
            };
            return Ok(dto);
        }
    }
}
