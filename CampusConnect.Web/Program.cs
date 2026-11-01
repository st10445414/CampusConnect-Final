using CampusConnect.Web.Data;
using CampusConnect.Web.Hubs;
using CampusConnect.Web.Models;
using CampusConnect.Web.Repositories;
using CampusConnect.Web.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Database
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequiredLength = 8;
    options.Password.RequireNonAlphanumeric = false;
    options.User.RequireUniqueEmail = true;
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

builder.Services.ConfigureApplicationCookie(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.SecurePolicy = Microsoft.AspNetCore.Http.CookieSecurePolicy.SameAsRequest;
    options.Cookie.SameSite = Microsoft.AspNetCore.Http.SameSiteMode.Lax;
    options.ExpireTimeSpan = TimeSpan.FromHours(24);
});

// Repositories (Scoped because they hold DbContext)
builder.Services.AddScoped<IListingRepository, ListingRepository>();
builder.Services.AddScoped<IAccommodationRepository, AccommodationRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IReportRepository, ReportRepository>();

// Favourites and Messaging repositories
builder.Services.AddScoped<IFavouriteRepository, FavouriteRepository>();
builder.Services.AddScoped<IConversationRepository, ConversationRepository>();

// Services
builder.Services.AddScoped<IMarketplaceService, MarketplaceService>();
builder.Services.AddScoped<IImageStorageService, BlobImageStorageService>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Seed roles and demo data
using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        if (app.Environment.IsDevelopment())
        {
            await context.Database.EnsureDeletedAsync();
        }
        await context.Database.EnsureCreatedAsync();

        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var roles = new[] { "Student", "Admin" };
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new IdentityRole(role));
        }

        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        var adminEmail = "admin@campusconnect.edu";
        var adminUser = await userManager.FindByEmailAsync(adminEmail);
        if (adminUser == null)
        {
            adminUser = new ApplicationUser
            {
                UserName = adminEmail,
                Email = adminEmail,
                FullName = "Dr. Sarah Mbeki",
                Faculty = "Administration",
                YearOfStudy = "Staff",
                IsVerified = true,
                JoinedDate = new DateTime(2023, 1, 1)
            };
            await userManager.CreateAsync(adminUser, "Admin123!");
            await userManager.AddToRoleAsync(adminUser, "Admin");
        }

        if (!context.Listings.Any())
        {
            var studentEmail = "aisha.patel@university.edu";
            var student = await userManager.FindByEmailAsync(studentEmail);
            if (student == null)
            {
                student = new ApplicationUser
                {
                    UserName = studentEmail,
                    Email = studentEmail,
                    FullName = "Aisha Patel",
                    Faculty = "Faculty of Engineering",
                    YearOfStudy = "2nd Year",
                    IsVerified = true,
                    JoinedDate = new DateTime(2024, 2, 12)
                };
                await userManager.CreateAsync(student, "Student123!");
                await userManager.AddToRoleAsync(student, "Student");
            }

            var liamEmail = "liam.chen@university.edu";
            var liam = await userManager.FindByEmailAsync(liamEmail);
            if (liam == null)
            {
                liam = new ApplicationUser
                {
                    UserName = liamEmail,
                    Email = liamEmail,
                    FullName = "Liam Chen",
                    Faculty = "Faculty of Science",
                    YearOfStudy = "3rd Year",
                    IsVerified = true,
                    JoinedDate = new DateTime(2023, 8, 30)
                };
                await userManager.CreateAsync(liam, "Student123!");
                await userManager.AddToRoleAsync(liam, "Student");
            }

            context.Listings.AddRange(
                new Listing { Title = "Calculus: Early Transcendentals (8th Ed.)", Price = 320, Category = "Textbooks", Condition = "Good", Description = "Used for MATH 114 last semester. Some highlighting in first 4 chapters.", Location = "Main Campus", Status = ListingStatus.Available, PostedDate = DateTime.UtcNow, SellerId = student.Id },
                new Listing { Title = "IKEA Desk Lamp - LED", Price = 85, Category = "Furniture", Condition = "Like New", Description = "Bought last year, barely used. Adjustable arm, USB charging port.", Location = "North Residences", Status = ListingStatus.Available, PostedDate = DateTime.UtcNow, SellerId = liam.Id },
                new Listing { Title = "Canon EOS 4000D DSLR Camera", Price = 1800, Category = "Electronics", Condition = "Good", Description = "Includes 18-55mm lens, strap, and 32GB SD card.", Location = "Main Campus", Status = ListingStatus.Available, PostedDate = DateTime.UtcNow, SellerId = student.Id },
                new Listing { Title = "Scientific Calculator - Casio FX-991EX", Price = 120, Category = "Electronics", Condition = "Like New", Description = "Used for one semester. No scratches, battery still good.", Location = "Main Campus", Status = ListingStatus.Available, PostedDate = DateTime.UtcNow, SellerId = student.Id }
            );

            context.Accommodations.AddRange(
                new Accommodation { Title = "Modern Studio Near Main Campus", Type = "Studio", RentPerMonth = 4500, Location = "Rondebosch", Bedrooms = 1, Bathrooms = 1, IsFurnished = true, Description = "Bright studio within 10 minutes walk of campus. Kitchenette, study desk, uncapped WiFi.", AvailableFrom = DateTime.UtcNow, PostedDate = DateTime.UtcNow, PostedById = student.Id, ContactEmail = studentEmail },
                new Accommodation { Title = "2-Bedroom Flat - Shared with Student", Type = "Flat", RentPerMonth = 3200, Location = "Mowbray", Bedrooms = 2, Bathrooms = 1, IsFurnished = true, Description = "One room available in shared flat. Close to Jammie shuttle route.", AvailableFrom = DateTime.UtcNow, PostedDate = DateTime.UtcNow, PostedById = liam.Id, ContactEmail = liamEmail }
            );

            context.Services.AddRange(
                new Service { Title = "Maths & Physics Tutoring", Category = "Tutoring", Price = "R80/hour", Description = "3rd-year Physics major offering tutoring for MATH 114 and first-year Physics.", ImageUrl = "", PostedDate = DateTime.UtcNow, ProviderId = liam.Id },
                new Service { Title = "Notes & Study Guides - Commerce", Category = "Academic", Price = "R30-R150", Description = "Comprehensive notes for ACC 106, FIN 201, and ECO 101.", ImageUrl = "", PostedDate = DateTime.UtcNow, ProviderId = student.Id }
            );

            await context.SaveChangesAsync();
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("========== SEEDING ERROR ==========");
        Console.WriteLine(ex.ToString());
        Console.WriteLine("===================================");
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

if (!app.Environment.IsDevelopment())
    app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapHub<ChatHub>("/chathub");

app.Run();
