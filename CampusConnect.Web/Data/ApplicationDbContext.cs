using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CampusConnect.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext<Models.ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<Models.Listing> Listings { get; set; } = null!;
        public DbSet<Models.ListingImage> ListingImages { get; set; } = null!;
        public DbSet<Models.Accommodation> Accommodations { get; set; } = null!;
        public DbSet<Models.Service> Services { get; set; } = null!;
        public DbSet<Models.Review> Reviews { get; set; } = null!;
        public DbSet<Models.Report> Reports { get; set; } = null!;
        public DbSet<Models.Favourite> Favourites { get; set; } = null!;
        public DbSet<Models.Conversation> Conversations { get; set; } = null!;
        public DbSet<Models.Message> Messages { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<Models.Listing>(entity =>
            {
                entity.HasOne(l => l.Seller)
                      .WithMany()
                      .HasForeignKey(l => l.SellerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(l => l.Status).HasConversion<string>();
                entity.Property(l => l.Price).HasPrecision(10, 2);
            });

            builder.Entity<Models.ListingImage>(entity =>
            {
                entity.HasKey(li => li.ImageId);
            });

            builder.Entity<Models.Accommodation>(entity =>
            {
                entity.HasOne(a => a.PostedBy)
                      .WithMany()
                      .HasForeignKey(a => a.PostedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(a => a.RentPerMonth).HasPrecision(10, 2);
            });

            builder.Entity<Models.Service>(entity =>
            {
                entity.HasOne(s => s.Provider)
                      .WithMany()
                      .HasForeignKey(s => s.ProviderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            builder.Entity<Models.Review>(entity =>
            {
                entity.HasOne(r => r.Reviewer)
                      .WithMany()
                      .HasForeignKey(r => r.ReviewerId)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(r => r.Service)
                      .WithMany(s => s.Reviews)
                      .HasForeignKey(r => r.ServiceId)
                      .OnDelete(DeleteBehavior.Cascade);
            });

            builder.Entity<Models.Report>(entity =>
            {
                entity.HasOne(r => r.ReportedBy)
                      .WithMany()
                      .HasForeignKey(r => r.ReportedById)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.Property(r => r.Status).HasConversion<string>();
            });

            builder.Entity<Models.Favourite>(entity =>
            {
                entity.HasOne(f => f.User)
                      .WithMany()
                      .HasForeignKey(f => f.UserId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(f => f.Listing)
                      .WithMany()
                      .HasForeignKey(f => f.ListingId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasIndex(f => new { f.UserId, f.ListingId }).IsUnique();
            });

            builder.Entity<Models.Conversation>(entity =>
            {
                entity.HasOne(c => c.User1)
                      .WithMany()
                      .HasForeignKey(c => c.User1Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(c => c.User2)
                      .WithMany()
                      .HasForeignKey(c => c.User2Id)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasIndex(c => new { c.User1Id, c.User2Id }).IsUnique();
            });

            builder.Entity<Models.Message>(entity =>
            {
                entity.HasOne(m => m.Conversation)
                      .WithMany(c => c.Messages)
                      .HasForeignKey(m => m.ConversationId)
                      .OnDelete(DeleteBehavior.Cascade);

                entity.HasOne(m => m.Sender)
                      .WithMany()
                      .HasForeignKey(m => m.SenderId)
                      .OnDelete(DeleteBehavior.Restrict);
            });
        }
    }
}
