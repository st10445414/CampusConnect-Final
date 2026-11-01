# CampusConnect User Guide

## 1. About CampusConnect

CampusConnect is a student-focused platform for Emeris Sandton. It allows students to find and advertise marketplace items, accommodation and services in one moderated environment.

The application has two main user roles:

- **Student:** browses content, creates personal listings, reports content and manages profile information.
- **Administrator:** manages users, listings, accommodation, services and reports.

---

## 2. Before You Start

### 2.1 Hosted use

Use the live application URL supplied by the project team. The final submission must contain the correct URL. Open it in a modern browser such as Microsoft Edge, Google Chrome or Mozilla Firefox.

### 2.2 Local setup requirements

For local development, install:

- Visual Studio 2022 with the ASP.NET and web development workload.
- .NET 8 SDK.
- SQL Server LocalDB, SQL Server Express, or another compatible SQL Server instance.
- Git, if the project is being obtained from GitHub.

### 2.3 Local setup

1. Download or clone the project repository.
2. Open `CampusConnect/CampusConnect.sln` in Visual Studio.
3. Allow NuGet packages to restore.
4. Confirm the application database connection in the application's configuration. Use a local database for local testing and never commit production credentials.
5. Select the web project as the startup project.
6. Run the application using the HTTPS profile.
7. The database is created and demonstration data is seeded when the application starts successfully.

If the team has supplied a hosted database connection or deployment instructions, use those values instead of changing the source code.

---

## 3. Demo Accounts

The application includes demonstration accounts for evaluation when the database is seeded:

| Role | Email | Password |
|---|---|---|
| Administrator | `admin@campusconnect.edu` | `Admin123!` |
| Student | `aisha.patel@university.edu` | `Student123!` |
| Student | `liam.chen@university.edu` | `Student123!` |

These credentials are for academic demonstration only. Change or remove seeded passwords before any non-demonstration release.

---

## 4. Navigation

### 4.1 Public pages

Unauthenticated visitors can access the landing page and public browsing pages. Public marketplace details and the read-only marketplace API can be viewed without signing in.

### 4.2 Student navigation

After signing in, use the student navigation to open:

- Dashboard
- Marketplace
- Accommodation
- Services
- Profile
- Logout

Some actions, such as posting, reporting or editing, are only shown to authenticated students.

### 4.3 Administrator navigation

Administrators can open:

- Admin dashboard
- Marketplace management
- Accommodation management
- Services management
- Reports
- Users
- Logout

Do not share administrator credentials with regular users.

---

## 5. Registering an Account

1. Open the landing page.
2. Select **Register**.
3. Enter your full name.
4. Enter a university-style email address ending in `.edu` or `.ac.za`.
5. Enter a password of at least eight characters containing a digit.
6. Select your faculty and year of study.
7. Submit the form.
8. Correct any validation messages shown on the page.
9. After successful registration, the new account is assigned the Student role and the user is signed in.

### Common registration problems

- **Email rejected:** use an approved university-style domain.
- **Password rejected:** use at least eight characters and include a digit.
- **Email already used:** sign in with the existing account or use a different email address.
- **Missing information:** complete every required field.

---

## 6. Logging In and Out

1. Select **Login**.
2. Enter your email and password.
3. Select **Remember me** only on a private device.
4. Select **Login**.
5. Students are taken to the student dashboard. Administrators are taken to the admin dashboard.
6. Select **Logout** when finished.

After several failed password attempts, the account may be temporarily locked by the security settings. Wait for the lockout period or ask the project administrator to assist.

---

## 7. Using the Marketplace

### 7.1 Browse listings

1. Open **Marketplace**.
2. Review the available listing cards.
3. Use the search field to search titles or descriptions.
4. Choose a category or condition filter.
5. Choose a sort order such as newest, lowest price or highest price.
6. Select a listing to open its details page.

The details page shows the item title, description, price, category, condition, location, posting information and seller details where available.

### 7.2 Create a listing

1. Sign in as a student.
2. Open **Marketplace**.
3. Select **Post a listing**.
4. Enter a title, price, category, condition, description and location.
5. Submit the form.
6. Read the success message.
7. The signed-in account is recorded as the seller by the server.
8. If moderation is enabled for the listing status, an administrator must approve the listing before it becomes available.

### 7.3 Mark an item as sold

1. Open a listing you own.
2. Select **Mark as sold**.
3. Confirm the action if prompted.
4. Return to the listing details and check the updated status.

Only use this action for an item that is no longer available.

### 7.4 Report a listing

1. Open the listing details page.
2. Select **Report**.
3. Enter a clear reason.
4. Submit the report.
5. A pending report is created for administrator review.

Do not use reports for ordinary disagreements. Include enough information for an administrator to understand the concern.

---

## 8. Using Accommodation

### 8.1 Find accommodation

1. Open **Accommodation**.
2. Review the available properties.
3. Use the available filters for type, rent, location, furnishing or other displayed criteria.
4. Select a property for full details.
5. Review rent, bedrooms, bathrooms, furnishing, availability, description and contact information.

### 8.2 Create accommodation

1. Sign in as a student.
2. Select the accommodation creation action.
3. Enter the property title, type, rent, location, bedrooms, bathrooms, furnishing status, availability and description.
4. Add contact information where requested.
5. Submit the form.
6. Correct any displayed validation messages.
7. Confirm that the new accommodation record appears after saving.

Use accurate information and remove or update a listing when the property is no longer available.

---

## 9. Using Services

### 9.1 Find a service

1. Open **Services**.
2. Browse service cards.
3. Filter by category if available.
4. Select a service to view the description, price, provider and reviews.
5. Use the provider contact option where available.

### 9.2 Advertise a service

1. Sign in as a student.
2. Select **Create service**.
3. Enter the service title, category, price and description.
4. Submit the form.
5. Confirm the success message and review the service details.

### 9.3 Review a service

1. Open a service details page.
2. Enter a rating and review where the form is available.
3. Submit the review.
4. Check that the review and updated average rating are displayed.

Reviews should be honest, relevant and respectful.

---

## 10. Managing Your Profile

1. Open **Profile** after signing in.
2. Review your name, faculty, year of study and account information.
3. Select **Edit profile**.
4. Update the fields that need changing.
5. Submit the form.
6. Confirm that the updated information is displayed.

Do not enter another person's personal information into your profile.

---

## 11. Administrator Guide

### 11.1 Open the dashboard

1. Log in with an administrator account.
2. The admin dashboard displays platform counts, including users, listings, accommodation, services and pending reports.
3. Use the navigation to open each management area.

### 11.2 Manage marketplace listings

1. Open **Admin > Marketplace**.
2. Use the search field to find a title or seller.
3. Use the status filter to show pending, available or other listing states.
4. Select **Approve** to make a suitable pending listing available.
5. Select **Remove** only when a listing violates the platform rules or is no longer appropriate.

Review content before taking action and record the reason for any removal in the team's moderation process.

### 11.3 Manage accommodation and services

1. Open the accommodation or services management page.
2. Review the displayed records.
3. Remove records only when moderation or the owner's request requires it.
4. Return to the page to confirm the action.

### 11.4 Manage reports

1. Open **Admin > Reports**.
2. Use the status filters to show pending, resolved or dismissed reports.
3. Open or review the reported content and reason.
4. Select **Resolve** when the issue has been addressed.
5. Select **Dismiss** when the report does not require action.
6. Confirm that the status changes in the filtered list.

### 11.5 Manage users

1. Open **Admin > Users**.
2. Review the account list.
3. Use verification controls only after checking the account.
4. Use **Make admin** only for an authorised staff or project administrator account.
5. Never promote a regular account for testing on the live system without approval.

---

## 12. API Guide

The application includes a read-only marketplace API for programmatic access.

### Listing collection

`GET /api/marketplaceapi`

Optional query values:

- `query`: search listing titles or descriptions.
- `category`: filter by category.
- `condition`: filter by item condition.
- `sortBy`: use `newest`, `price-low` or `price-high`.

Example request:

`/api/marketplaceapi?category=Textbooks&sortBy=price-low`

### Single listing

`GET /api/marketplaceapi/{id}`

Replace `{id}` with a listing identifier. A valid listing returns JSON. A missing identifier returns a not-found response.

### Swagger

Swagger documentation is configured for the Development environment. Open `/swagger` on a locally running Development instance to view and test the available endpoints.

---

## 13. Troubleshooting

| Problem | What to check |
|---|---|
| The page does not load | Confirm the application is running and use the correct HTTPS URL. |
| Login fails | Check the email and password, then check whether the account is temporarily locked. |
| Admin page is blocked | Confirm that the signed-in account has the Admin role. |
| A new record is not visible | Check validation messages, refresh the page, and confirm the database connection. |
| Database error on startup | Check SQL Server availability and the configured connection string. |
| API returns not found | Confirm the route and listing ID. |
| Changes do not appear | Refresh the page and verify that the correct hosted environment is being used. |
| Deployment fails | Open GitHub Actions, read the failed step, and check the hosting provider settings and secrets. |

Do not paste passwords or database credentials into GitHub issues, screenshots or public documentation.

---

## 14. Safe Use Guidelines

- Use only your own account details.
- Do not publish private information in listing descriptions.
- Meet other users in safe, appropriate locations and follow campus guidance.
- Report suspicious, abusive or misleading content.
- Administrators should review moderation actions carefully.
- Do not reuse demonstration passwords on a real service.
- Do not commit connection strings, passwords or deployment secrets to source control.

---

## 15. Feature Status

The core features documented above are part of the implemented academic solution. Image uploads, favourites and real-time messaging should be treated as future enhancements unless the team has verified them in the final hosted environment.

For assessment evidence, combine this guide with the final report, screenshots, the GitHub repository, the presentation materials and the final deployment details.
