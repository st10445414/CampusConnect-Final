using CampusConnect.Web.Models;
using Xunit;

namespace CampusConnect.Tests
{
    public class ListingModelTests
    {
        [Fact]
        public void Listing_DefaultStatus_IsPendingReview()
        {
            var listing = new Listing();
            Assert.Equal(ListingStatus.PendingReview, listing.Status);
        }

        [Fact]
        public void Listing_PostedDate_DefaultsToUtcNow()
        {
            var listing = new Listing();
            Assert.True((DateTime.UtcNow - listing.PostedDate).TotalSeconds < 5);
        }

        [Fact]
        public void Listing_Images_Collection_InitializedEmpty()
        {
            var listing = new Listing();
            Assert.Empty(listing.Images);
        }

        [Theory]
        [InlineData("Textbooks", 250, "Good")]
        [InlineData("Electronics", 1800, "Like New")]
        [InlineData("Furniture", 85, "Fair")]
        public void Listing_CanBeCreatedWithVariousValues(string category, decimal price, string condition)
        {
            var listing = new Listing
            {
                Title = "Test Item",
                Category = category,
                Price = price,
                Condition = condition
            };
            Assert.Equal(category, listing.Category);
            Assert.Equal(price, listing.Price);
            Assert.Equal(condition, listing.Condition);
        }
    }

    public class ServiceModelTests
    {
        [Fact]
        public void Service_GetAverageRating_ReturnsZeroWhenNoReviews()
        {
            var service = new Service();
            Assert.Equal(0, service.GetAverageRating());
        }

        [Fact]
        public void Service_GetAverageRating_CalculatesCorrectly()
        {
            var service = new Service
            {
                Reviews = new List<Review>
                {
                    new Review { Rating = 4 },
                    new Review { Rating = 5 },
                    new Review { Rating = 3 }
                }
            };
            Assert.Equal(4.0, service.GetAverageRating());
        }
    }

    public class AccommodationModelTests
    {
        [Fact]
        public void Accommodation_DefaultValues_AreSet()
        {
            var acc = new Accommodation();
            Assert.Equal(string.Empty, acc.Title);
            Assert.Equal(string.Empty, acc.Type);
            Assert.Equal(0, acc.RentPerMonth);
            Assert.Equal(0, acc.Bedrooms);
        }

        [Fact]
        public void Accommodation_CanBeCreatedWithAllFields()
        {
            var acc = new Accommodation
            {
                Title = "Test Studio",
                Type = "Studio",
                RentPerMonth = 4500,
                Location = "Rondebosch",
                Bedrooms = 1,
                Bathrooms = 1,
                IsFurnished = true,
                Description = "Nice place"
            };
            Assert.Equal("Test Studio", acc.Title);
            Assert.Equal(4500, acc.RentPerMonth);
            Assert.True(acc.IsFurnished);
        }
    }

    public class ReportModelTests
    {
        [Fact]
        public void Report_DefaultStatus_IsPending()
        {
            var report = new Report();
            Assert.Equal(ReportStatus.Pending, report.Status);
        }

        [Theory]
        [InlineData("Spam listing", "Listing", 1)]
        [InlineData("Inappropriate content", "Service", 5)]
        public void Report_CanBeCreatedWithValues(string reason, string targetType, int targetId)
        {
            var report = new Report
            {
                Reason = reason,
                TargetType = targetType,
                TargetId = targetId
            };
            Assert.Equal(reason, report.Reason);
            Assert.Equal(targetType, report.TargetType);
            Assert.Equal(targetId, report.TargetId);
        }
    }
}
