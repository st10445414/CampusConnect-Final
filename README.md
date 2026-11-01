# CampusConnect

A verified student-only platform for buying, selling, finding accommodation, and discovering services within a university community.

Built for INSY 7315 - Software Engineering Project.

---

## Table of Contents

1. [Features](#features)
2. [Technology Stack](#technology-stack)
3. [Prerequisites](#prerequisites)
4. [How to Run Locally](#how-to-run-locally)
5. [Demo Accounts](#demo-accounts)
6. [Database Setup](#database-setup)
7. [Architecture](#architecture)
8. [API Documentation](#api-documentation)
9. [Security](#security)
10. [Testing](#testing)
11. [GitHub Workflow (CI/CD)](#github-workflow-cicd)
12. [Hosting](#hosting)
13. [Branch Strategy](#branch-strategy)
14. [Task 2 Rubric Mapping](#task-2-rubric-mapping)

---

## Features

### Student Features
- **Marketplace**: Browse, search, filter, and post listings for textbooks, electronics, furniture, and more.
- **Accommodation**: Search for rooms, flats, and house shares near campus with filters for type, price, and furnishing.
- **Services**: Browse and offer services (tutoring, repairs, creative work). Rate and review services.
- **Profile**: View and edit your profile, see your listings and services in one place.
- **Reporting**: Report inappropriate listings for admin review.

### Admin Features
- **Dashboard**: Platform overview with user, listing, accommodation, service, and report counts.
- **Marketplace Management**: Approve, remove, and monitor all marketplace listings.
- **Accommodation Management**: Review and remove accommodation listings.
- **Services Management**: Review and remove service listings.
- **User Management**: Toggle user verification, promote users to admin role.
- **Reports & Moderation**: Review, resolve, and dismiss user reports.

---

## Technology Stack

| Layer | Technology |
|-------|-----------|
| Front End | ASP.NET Core MVC Razor Views, Bootstrap 5, Bootstrap Icons |
| Back End | C# / ASP.NET Core 8.0 MVC |
| Database | SQL Server (LocalDB for development) |
| ORM | Entity Framework Core 8.0 |
| Authentication | ASP.NET Core Identity |
| API | REST API controller with Swagger documentation |
| Testing | xUnit |
| CI/CD | GitHub Actions |
| Fonts | Inter + Poppins (Google Fonts) |

---

## Prerequisites

- **Visual Studio 2022** (17.8+) with the ASP.NET and web development workload
- **.NET 8 SDK** ([download](https://dotnet.microsoft.com/download/dotnet/8.0))
- **SQL Server LocalDB** (included with Visual Studio) or any SQL Server instance

---

## How to Run Locally

### Option A: Visual Studio (recommended)

1. Open `CampusConnect/CampusConnect.sln` in Visual Studio 2022.
2. Wait for NuGet packages to restore (happens automatically).
3. Press **F5** or **Ctrl+F5** to run.
4. The browser will open at `https://localhost:7xxx` automatically.
5. The database will be created automatically on first run via Entity Framework migrations and seeding.

### Option B: Command Line

```bash
# Navigate to the web project
cd CampusConnect/CampusConnect.Web

# Restore packages
dotnet restore

# Apply database migrations (creates the database)
dotnet ef database update

# Run the application
dotnet run
```

The app will be available at the URL shown in the terminal (typically `https://localhost:5001` or `http://localhost:5000`).

---

## Demo Accounts

The application seeds these accounts automatically on first run:

| Role | Email | Password | Use For |
|------|-------|----------|---------|
| Admin | admin@campusconnect.edu | Admin123! | Admin panel access |
| Student | aisha.patel@university.edu | Student123! | Student features |
| Student | liam.chen@university.edu | Student123! | Student features |

The database also seeds sample marketplace listings, accommodation, and services for demonstration.

---

## Database Setup

### Development (LocalDB - automatic)

The default connection string uses SQL Server LocalDB, which is installed with Visual Studio. The database is created automatically when you first run the application.

Connection string (in `appsettings.json`):
```
Server=(localdb)\MSSQLLocalDB;Database=CampusConnectDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

### Production

Update the connection string in `appsettings.json` (or better, via environment variables) to point to your hosted SQL Server instance:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=your-server;Database=CampusConnectDb;User Id=youruser;Password=yourpassword;TrustServerCertificate=True"
}
```

Run `dotnet ef database update` once on the production database to create all tables.

### Entity Framework Migrations

To create a new migration after model changes:
```bash
cd CampusConnect/CampusConnect.Web
dotnet ef migrations add MigrationName
dotnet ef database update
```

---

## Architecture

```
CampusConnect.Web/
├── Controllers/          # MVC controllers (handle HTTP requests)
│   ├── AccountController.cs      # Login, register, logout
│   ├── HomeController.cs         # Landing page, student dashboard
│   ├── MarketplaceController.cs # Browse, create, report listings
│   ├── AccommodationController.cs # Browse, create accommodation
│   ├── ServiceController.cs      # Browse, create, review services
│   ├── ProfileController.cs      # View/edit profile, submit reports
│   ├── AdminController.cs        # Admin panel: manage all content
│   └── Api/
│       └── MarketplaceApiController.cs  # REST API for listings
├── Models/                # Data models (Entity Framework entities)
│   ├── ApplicationUser.cs        # Extended Identity user
│   ├── Listing.cs                # Marketplace listing + images
│   ├── Accommodation.cs         # Accommodation listing
│   ├── Service.cs               # Service listing + reviews
│   └── Report.cs                 # User-submitted reports
├── Repositories/          # Repository pattern for data access
│   ├── IRepository.cs            # Generic interface
│   ├── Repository.cs            # Generic implementation
│   ├── ListingRepository.cs     # Listing-specific queries
│   ├── AccommodationRepository.cs
│   ├── ServiceRepository.cs
│   └── ReportRepository.cs
├── Services/              # Business logic layer
│   └── MarketplaceService.cs    # Marketplace business logic
├── Data/
│   └── ApplicationDbContext.cs   # EF Core database context
├── Views/                 # Razor view templates
│   ├── Account/            # Login, Register
│   ├── Home/               # Landing page, Dashboard
│   ├── Marketplace/        # Index, Details, Create
│   ├── Accommodation/      # Index, Details, Create
│   ├── Service/            # Index, Details, Create
│   ├── Profile/            # Index, Edit
│   ├── Admin/              # Dashboard, Marketplace, Reports, Accommodation, Services, Users
│   └── Shared/             # Layouts (_Layout, _StudentLayout, _AdminLayout)
└── wwwroot/
    └── css/site.css        # Custom design system

CampusConnect.Tests/        # xUnit test project
```

### Design Patterns Used
- **Repository Pattern**: Abstracts data access behind interfaces (`IRepository<T>`, `IListingRepository`, etc.)
- **Service Layer**: Encapsulates business logic (`MarketplaceService`)
- **Dependency Injection**: All controllers receive dependencies via constructor injection
- **MVC**: Model-View-Controller separation of concerns

---

## API Documentation

The application includes a REST API for marketplace listings, documented with Swagger.

### Access Swagger UI
Run the application in Development mode and navigate to:
```
https://localhost:PORT/swagger
```

### Endpoints

| Method | Route | Description | Auth Required |
|--------|-------|-------------|---------------|
| GET | `/api/marketplaceapi` | Get all listings (with optional filters) | No |
| GET | `/api/marketplaceapi/{id}` | Get a specific listing by ID | No |

### Query Parameters (GET /api/marketplaceapi)
- `query` - Search term for title/description
- `category` - Filter by category (Textbooks, Electronics, Furniture, etc.)
- `condition` - Filter by condition (New, Like New, Good, Fair)
- `sortBy` - Sort order: `newest` (default), `price-low`, `price-high`

### Example Response
```json
[
  {
    "id": 1,
    "title": "Calculus: Early Transcendentals (8th Ed.)",
    "price": 320,
    "category": "Textbooks",
    "condition": "Good",
    "location": "Main Campus",
    "status": "Available",
    "postedDate": "2026-09-22T10:00:00Z",
    "seller": "Aisha Patel"
  }
]
```

---

## Security

### Authentication
- ASP.NET Core Identity with email/password authentication
- Password requirements: minimum 8 characters
- University email validation (`.edu` or `.ac.za` domains)
- Anti-forgery tokens on all POST forms (CSRF protection)

### Authorization
- **Role-based**: `Student` and `Admin` roles
- Admin controllers use `[Authorize(Roles = "Admin")]`
- Student controllers use `[Authorize]`
- Unauthenticated users can browse marketplace, accommodation, and services (read-only)

### Input Validation
- Data annotations on all models (`[Required]`, `[StringLength]`, `[Range]`, `[EmailAddress]`)
- Server-side ModelState validation
- Client-side validation via Bootstrap

### Data Safety
- Users can only delete their own listings and services
- Reports are submitted with the authenticated user's ID
- Admin actions require admin role authorization

---

## Testing

### Test Project
`CampusConnect.Tests` contains xUnit tests covering:

- **Listing model**: Default status, date initialization, image collection, property assignment
- **Service model**: Average rating calculation with and without reviews
- **Accommodation model**: Default values, full property assignment
- **Report model**: Default status, creation with various values
- **Theory tests**: Data-driven tests for multiple input scenarios

### Run Tests

In Visual Studio:
1. Open Test Explorer (Test > Test Explorer)
2. Click "Run All"
3. Or press Ctrl+R, T

Command line:
```bash
dotnet test CampusConnect/CampusConnect.sln
```

---

## GitHub Workflow (CI/CD)

The repository includes a GitHub Actions workflow (`.github/workflows/build-deploy.yml`) that:

### On every push or pull request to `main` or `develop`:
1. Restores NuGet packages
2. Builds the solution in Release configuration
3. Runs all xUnit tests
4. Fails the pipeline if any build or test fails

### On push to `develop` (staging):
1. Builds and tests
2. Publishes the application
3. Deploys to staging environment

### On push to `main` (production):
1. Builds and tests
2. Publishes the application
3. Deploys to production environment

### Configuring Deployment
The deploy steps are placeholder templates. To enable deployment:
1. Choose your hosting provider (Azure App Service, Render, Railway, Fly.io, etc.)
2. Add the required secrets to your GitHub repository settings
3. Uncomment and configure the deployment step in the workflow file

---

## Hosting

### Requirements (per POE)
- Hosted website accessible via public URL
- Hosted database connected to the application
- API accessible in the hosted environment
- Application stable and responsive

### Hosting Options

| Provider | Free Tier | ASP.NET Support | Database |
|----------|-----------|-----------------|----------|
| Azure App Service | Free F1 (limited) | Yes | Azure SQL (free tier) |
| Render | Free web service | Yes (Docker) | PostgreSQL (free) |
| Railway | Trial credits | Yes | PostgreSQL |
| Fly.io | Free allowance | Yes (Docker) | SQLite or external |

### Deployment Steps
1. Set the production connection string as an environment variable
2. Run `dotnet ef database update` on the production database
3. Publish the application: `dotnet publish -c Release -o ./publish`
4. Deploy the published folder to your hosting provider
5. Verify the application loads and the database is connected
6. Test login, registration, and core features on the deployed version

---

## Branch Strategy

| Branch | Purpose |
|--------|---------|
| `main` | Production-ready code. Merges trigger production deployment. |
| `develop` | Integration branch for team work. Merges trigger staging deployment. |
| `feature/*` | Individual feature branches (e.g., `feature/marketplace-search`) |
| `bugfix/*` | Bug fix branches |

### Workflow
1. Create a feature branch from `develop`
2. Make changes and commit
3. Open a pull request to `develop`
4. CI pipeline runs build and tests
5. After review, merge to `develop`
6. When ready for release, merge `develop` to `main`

---

## Task 2 Rubric Mapping

### 9.4.1 Front End
- **Look and Feel**: Custom design system with 6 color ramps, Inter/Poppins fonts, consistent cards, badges, and spacing
- **Usability**: Clear navigation, search/filter bars, logical grouping, breadcrumbs via active nav states
- **Affordance**: Buttons with icons and hover states, links distinguishable, cards clickable with hover lift
- **Branding**: CampusConnect logo, brand colors (blue primary, green accent), consistent across all screens
- **Responsive Design**: Mobile breakpoints at 768px and 1024px, responsive grids, hidden sidebar on mobile
- **Accessibility**: Focus-visible outlines, screen-reader-only utility class, prefers-reduced-motion support, ARIA labels
- **Colour and Typography**: 6 color ramps + neutrals, Inter body (1.5 line-height), Poppins headings (1.2 line-height)
- **Consistency**: Shared layouts (_StudentLayout, _AdminLayout), reusable CSS classes, consistent form styling
- **Feedback**: Success messages (TempData), validation errors, empty states, loading via form submission, confirmation dialogs
- **Performance**: Minimal JavaScript, CSS-only animations, no heavy frameworks, AsNoTracking on reads

### 9.4.2 Back End
- **Basic**: Control structures, data types, loops, functions throughout controllers and services
- **Intermediate**: Modular code, classes, error handling, repository pattern, service layer
- **Advanced**: Design patterns (Repository, DI, Service Layer), role-based authorization, API controller
- **Database Integration**: Entity Framework Core, 5 entities with relationships, seeded data, migrations
- **APIs**: REST API controller with Swagger, clear input/output DTOs, error handling
- **Security**: ASP.NET Identity, role-based auth, anti-forgery tokens, input validation, university email restriction
- **Data Flow**: Controller -> Service -> Repository -> DbContext -> Database, with proper error handling

### 9.4.3 Hosting
- Application configured for deployment with separate development/production settings
- Connection string configurable via environment variables
- Database migrations can be run against any SQL Server instance
- API accessible in hosted environment

### 9.4.4 GitHub/Pipelines
- Branch strategy documented (main, develop, feature branches)
- Automated build and test on every push/PR
- Automated deployment to staging (develop) and production (main)
- CI/CD maintains code quality by failing on test failures

---

## Task 3 Final Submission Materials

The final Task 3 documents are included in this repository:

- [Final Report (HTML)](FINAL_REPORT_TASK3.html)
- [Final Report (Markdown)](FINAL_REPORT_TASK3.md)
- [User Guide (HTML)](USER_GUIDE_TASK3.html)
- [User Guide (Markdown)](USER_GUIDE_TASK3.md)
- [Task 1 Documentation](../CampusConnect_Task1_Documentation.html)

Before submitting, complete the following project-specific details in the final report:

- [ ] Add the final GitHub repository URL.
- [x] Live application URL: https://campusconnect-and4cuhjevaud8ed.uaenorth-01.azurewebsites.net
- [ ] Confirm the actual hosting and database status.
- [ ] Add each member's verified individual contribution and evidence.
- [ ] Add the authenticity declaration and peer evaluations.
- [ ] Add the final presentation slides or recording link.
- [ ] Attach screenshots showing the live application, GitHub workflow, tests and API.
- [ ] Replace any deployment placeholders in the GitHub Actions workflow with the selected provider's real deployment configuration.

The final report deliberately identifies incomplete or unverified features, including image uploads, favourites, real-time messaging and provider-specific deployment configuration. These should only be marked complete after the team has tested them in the final environment.

## License

This is an academic project for INSY 7315 at the Independent Institute of Education.
