# CampusConnect
## INSY7315 Task 3 Final Report

**Team:** Campus Innovators (Group 4)  
**Client:** Emeris Sandton  
**Lecturer:** Tebogo Rakgogo  
**Module:** Information Systems 3E – Work Integrated Learning (INSY7315)  
**Submission:** Task 3 Final Project, Report and User Guide  
**Date:** October 2026

> **Submission note:** The live application is available at https://campusconnect-and4cuhjevaud8ed.uaenorth-01.azurewebsites.net. Add the final GitHub repository URL and presentation link before submitting. Keep this report in the repository so the marker can access the supporting evidence.

---

## Table of Contents

1. Executive Summary  
2. Project Overview and Client Need  
3. Requirements and Scope  
4. Final System and Architecture  
5. Implemented Functionality  
6. Security and Data Protection  
7. Testing and Quality Assurance  
8. Hosting, Deployment and CI/CD  
9. Feedback, Refinement and Improvements  
10. Challenges, Limitations and Future Work  
11. Teamwork and Individual Contribution  
12. Conclusion  
13. Submission Evidence Checklist  

---

## 1. Executive Summary

CampusConnect is a campus-exclusive web application for Emeris Sandton students. It brings together a student marketplace, accommodation listings, a services directory, student profiles, reporting, and administration in one moderated platform.

The solution addresses the problem of students relying on fragmented and unmoderated channels such as messaging groups and general marketplace platforms. These channels make it difficult to verify users, find relevant campus opportunities, and report inappropriate content. CampusConnect provides a structured alternative with authentication, student and administrator roles, searchable content, moderation tools, and a REST API for marketplace data.

The final implementation uses ASP.NET Core 8 MVC, C#, Entity Framework Core, Microsoft SQL Server, ASP.NET Core Identity, Bootstrap 5, Razor Views, Swagger, xUnit, and GitHub Actions. The system is organised using MVC, repository, service-layer, and dependency-injection patterns.

The project includes the core functionality agreed during Task 1 and Task 2. Remaining enhancements are documented honestly in this report rather than being presented as completed features.

---

## 2. Project Overview and Client Need

### 2.1 Client

The client for the project is Emeris Sandton. The target users are students who need a trusted campus environment for buying and selling goods, finding accommodation, and discovering student-provided services. Administrators require oversight of users, listings, services, accommodations, and reports.

### 2.2 Problem Statement

Students often use several unrelated platforms to find textbooks, furniture, accommodation, tutoring, repairs, and other services. General platforms do not provide a campus-specific identity, consistent moderation, or a central place for student-focused information. This creates unnecessary searching, increases exposure to scams or unsuitable content, and makes it difficult for the institution to support a safe digital community.

### 2.3 Proposed Solution

CampusConnect provides:

- Student and administrator authentication.
- University-style email validation during registration.
- A marketplace for student goods.
- Accommodation listings near campus.
- A student services directory.
- Search and filtering.
- Profiles and user-generated content.
- Reporting and administrator moderation.
- A documented read-only marketplace API.
- Automated build and test checks through GitHub Actions.

### 2.4 Project Objectives

1. Provide a clear and responsive student-facing interface.
2. Store application data in a relational database.
3. Apply authentication and role-based authorisation.
4. Allow administrators to moderate content.
5. Provide reliable data flow between views, controllers, services, repositories, and the database.
6. Demonstrate testing and an automated development workflow.
7. Produce documentation that allows another user to set up and operate the application.

---

## 3. Requirements and Scope

### 3.1 User Roles

| Role | Purpose | Main permissions |
|---|---|---|
| Student | Uses the campus platform as a buyer, seller, accommodation seeker, or service provider. | Register, log in, browse, create content, report content, manage personal activity. |
| Service provider | A student who advertises a skill or service. | All student permissions plus service creation and receiving reviews. |
| Administrator | Moderates the platform and manages users. | View dashboard, manage listings, manage accommodation and services, manage users, resolve reports. |

### 3.2 Functional Requirements and Final Status

| Requirement | Final status | Evidence in the solution |
|---|---|---|
| Register and log in | Implemented | Account controller, Identity configuration, login and registration views. |
| Restrict registration to university-style domains | Implemented | Server-side `.edu` and `.ac.za` validation. |
| Browse marketplace listings | Implemented | Marketplace index and details views. |
| Search, filter and sort marketplace listings | Implemented | Marketplace service and query parameters for search, category, condition and sort order. |
| Create marketplace listings | Implemented | Authenticated create form and server-side seller assignment. |
| Report marketplace content | Implemented | Report action creates a pending report for administrator review. |
| Mark a marketplace item as sold | Implemented | Authenticated marketplace action. |
| Browse accommodation | Implemented | Accommodation index and details views. |
| Create accommodation listings | Implemented | Authenticated accommodation creation flow. |
| Browse and create services | Implemented | Service index, details and create views. |
| Review and rate services | Implemented | Review model and average-rating logic. |
| View and edit profile information | Implemented | Profile views and controller actions. |
| Administrator dashboard | Implemented | Dashboard counts for users, listings, accommodation, services and reports. |
| Administrator content moderation | Implemented | Approval, removal, report resolution and dismissal actions. |
| Administrator user management | Implemented | Verification toggle and administrator promotion actions. |
| REST API | Implemented | Read-only marketplace endpoints documented using Swagger in development. |
| Image uploads | Partial / future enhancement | Image-related model/package support exists, but a complete upload and production storage flow should be verified before claiming completion. |
| Favourites and real-time messaging | Future enhancement | Not included in the final core implementation. |

### 3.3 Non-functional Requirements

- **Usability:** clear navigation, grouped features, validation messages, success messages and empty states.
- **Accessibility:** semantic forms, visible focus states, labels, readable contrast, and reduced-motion support in the interface styling.
- **Security:** Identity authentication, password rules, account lockout, role checks, HTTPS redirection in production, anti-forgery protection on forms, validation, and parameterised database access through Entity Framework Core.
- **Maintainability:** MVC separation, repository interfaces, a business-logic service, dependency injection, and a documented folder structure.
- **Responsiveness:** Bootstrap layout patterns and responsive CSS for desktop, tablet and mobile screens.
- **Performance:** server-side filtering, read-focused repository queries, limited client-side scripting, and a small focused API surface.

---

## 4. Final System and Architecture

### 4.1 Technology Stack

| Area | Technology |
|---|---|
| Application framework | ASP.NET Core 8 MVC |
| Language | C# |
| Presentation | Razor Views, Bootstrap 5, custom CSS and Bootstrap Icons |
| Authentication | ASP.NET Core Identity with Identity roles |
| Database | Microsoft SQL Server |
| Data access | Entity Framework Core 8 |
| API documentation | Swashbuckle / Swagger |
| Automated tests | xUnit |
| Source control and pipeline | GitHub and GitHub Actions |
| Hosting target | Azure App Service with hosted SQL Server configuration |

### 4.2 Layered Data Flow

The main request flow is:

1. A user selects a page or submits a form in the browser.
2. ASP.NET routing sends the request to a controller action.
3. The controller checks authentication, authorisation, and model validation where applicable.
4. The controller calls a repository or the marketplace service.
5. The repository uses Entity Framework Core and the application database context.
6. The database returns entities and related data.
7. The controller returns a Razor View or a JSON API response.
8. The browser displays the result and any success, validation, or error feedback.

### 4.3 Main Application Layers

- **Views:** HTML presentation and forms for landing, account, student, marketplace, accommodation, services, profile, and admin pages.
- **Controllers:** coordinate requests and responses for accounts, home/dashboard, marketplace, accommodation, services, profiles, administration, and the marketplace API.
- **Services:** contain reusable business logic, including marketplace search and listing operations.
- **Repositories:** provide generic and feature-specific data access through interfaces.
- **Models:** represent users, listings, listing images, accommodation, services, reviews and reports.
- **Data context:** maps the application entities and relationships to SQL Server.

### 4.4 Database Model

The database extends the standard Identity tables with application entities:

- `ApplicationUser` stores identity information together with full name, faculty, year of study, verification status and joining date.
- `Listing` stores marketplace items, seller relationship, price, category, condition, status and posting date.
- `ListingImage` stores image records associated with listings.
- `Accommodation` stores property type, rent, location, bedrooms, bathrooms, furnishing and advertiser relationship.
- `Service` stores service title, category, price, description and provider relationship.
- `Review` stores ratings and review relationships between students and services.
- `Report` stores reported content, reporting user, reason, date and moderation status.

Important relationship decisions include restricted deletion for user-owned marketplace, accommodation, service and report records, and cascading deletion from a service to its reviews.

### 4.5 API

The marketplace API is read-only and returns JSON. It supports:

- `GET /api/marketplaceapi`
- `GET /api/marketplaceapi/{id}`

The collection endpoint accepts optional search, category, condition and sort parameters. The API uses data-transfer output objects so that the response shape is controlled instead of exposing the entire database entity.

---

## 5. Implemented Functionality

### 5.1 Authentication and Registration

Users can register with their name, email, password, faculty and year of study. The server checks required fields, password length, email format, and university-style domains. New accounts are assigned the Student role. Existing users can log in with email and password, select a remember-me option, and log out.

### 5.2 Marketplace

Students can view public marketplace content, search by text, filter by category and condition, and sort by newest or price. Authenticated students can create listings, mark items as sold, and report unsuitable content. Administrators can search listings, filter by status, approve pending content, and remove listings.

### 5.3 Accommodation

Students can browse accommodation listings and view details such as type, rent, location, bedrooms, bathrooms, furnishing, availability, description and contact information. Authenticated users can create accommodation listings. Administrators can review and remove accommodation entries.

### 5.4 Services and Reviews

Students can browse services such as tutoring and academic support, view provider details, and create service listings. Services contain descriptions and prices. Reviews store ratings and comments, and average ratings are calculated from the review collection.

### 5.5 Profiles

The profile area allows a signed-in student to view their account details and edit profile information. Profile data includes the user's name, faculty and year of study, together with their campus activity.

### 5.6 Administration

The administrator dashboard summarises the platform. The admin area provides pages for marketplace listings, accommodation, services, reports and users. Admin actions include approving or removing marketplace listings, removing accommodation or service records, resolving or dismissing reports, toggling verification, and promoting a user to the Admin role.

---

## 6. Security and Data Protection

### 6.1 Authentication Controls

ASP.NET Core Identity manages account creation, password hashing, sign-in cookies, sign-out, unique email addresses and token providers. Passwords are not stored as plain text. The configured password policy requires at least eight characters and a digit. Account lockout is enabled after five failed attempts for five minutes.

### 6.2 Authorisation

The system uses Student and Admin roles. New users receive the Student role. The administrator controller requires the Admin role. Student-only actions require an authenticated user, while public browsing and read-only marketplace API actions are explicitly allowed anonymously.

### 6.3 Input and Request Protection

Forms use model validation attributes and controllers check `ModelState` before processing submitted data. POST forms use ASP.NET Core anti-forgery protection. Redirect handling after login only allows local URLs, reducing open-redirect risk.

### 6.4 Data Access Protection

Entity Framework Core generates parameterised queries for the repository and service operations. The application does not construct database queries by concatenating user input. Ownership-related identifiers for new listings and reports are taken from the signed-in user on the server instead of being trusted from the form.

### 6.5 Transport and Cookies

Production requests use HTTPS redirection and HSTS. The authentication cookie is HttpOnly, uses SameSite=Lax, and has a configured lifetime. Production exception handling avoids displaying development stack traces to end users.

### 6.6 Security Limitations to Address in Future

Before production use beyond an academic demonstration, the team should verify that every destructive POST form explicitly renders an anti-forgery token, replace seeded demonstration passwords, add rate limiting and audit logging, and complete a production storage strategy for uploaded images. The public API should also be rate-limited if exposed to external consumers.

---

## 7. Testing and Quality Assurance

### 7.1 Automated Tests

The xUnit test project covers:

- Default marketplace listing status.
- Marketplace listing posting date initialisation.
- Listing image collection initialisation.
- Marketplace property assignment using multiple data sets.
- Service average rating when there are no reviews.
- Service average rating when reviews exist.
- Accommodation defaults and property assignment.
- Report default status and property assignment.

The theory tests demonstrate that the same behaviour is checked against multiple input combinations.

### 7.2 Manual Acceptance Testing

The following user journeys should be executed and recorded as evidence before submission:

| Test | Expected result |
|---|---|
| Register with an invalid non-university domain | Registration is rejected with a validation message. |
| Register with valid details | A Student account is created and the user reaches the student dashboard. |
| Login with valid credentials | The user is signed in and redirected appropriately. |
| Five failed login attempts | Account lockout is triggered according to the configured policy. |
| Student opens Admin page | Access is denied because the user lacks the Admin role. |
| Create marketplace listing | The listing is saved with the signed-in user as seller. |
| Search and filter listings | Matching records are returned using the selected criteria. |
| Submit a report | A pending report is stored for admin review. |
| Admin approves a listing | Listing status changes to available. |
| Admin resolves a report | Report status changes to resolved. |
| API listing request | A JSON response is returned with the expected listing fields. |
| Refresh after creating content | The record remains available, confirming database persistence. |

### 7.3 Quality Evidence

The repository should include screenshots or links showing:

- A successful GitHub Actions build and test run.
- The test project and test output.
- The live application or hosting dashboard.
- The API response or Swagger page.
- Branches, pull requests and commit history.

---

## 8. Hosting, Deployment and CI/CD

### 8.1 Hosting Plan

The intended production arrangement is Azure App Service for the ASP.NET Core application and a hosted SQL Server database. The database connection is supplied through hosting configuration rather than committed into source code.

Before final submission, record the actual live URL and confirm the application can:

1. Load over HTTPS.
2. Connect to the hosted database.
3. Register and authenticate users.
4. Create and retrieve persisted records.
5. Serve the marketplace API.

### 8.2 GitHub Actions Workflow

The workflow runs on pushes and pull requests targeting `main` and `develop`. It:

1. Checks out the repository.
2. Installs .NET 8.
3. Restores solution dependencies.
4. Builds the solution in Release mode.
5. Runs the automated tests.
6. Publishes the web project for deployment on successful branch builds.

The workflow contains staging and production deployment sections. These sections must contain the selected hosting provider's real deployment action and repository secrets before claiming that automated deployment is live. Placeholder deployment commands should be replaced before the final submission.

### 8.3 Branching Workflow

The documented strategy is:

- `main`: production-ready code.
- `develop`: integration and staging branch.
- `feature/*`: individual feature work.
- `bugfix/*`: focused corrections.

Team members should merge through pull requests and retain the successful CI checks as evidence.

---

## 9. Feedback, Refinement and Improvements

The final implementation incorporates the following refinement themes from the earlier project stages:

- Consistent branding, colours, typography, cards, badges and layouts.
- Responsive layout support for desktop, tablet and mobile screens.
- Clear navigation for student and administrator roles.
- Search and status filtering in the administrator marketplace view.
- Server-side validation and user-facing feedback for forms.
- Clear separation between public browsing and authenticated actions.
- Documented repository, service and controller responsibilities.
- Automated tests and a repeatable GitHub workflow.

The final report should be updated with any direct lecturer or client feedback received after the last demonstration. Where no feedback was provided, state that clearly rather than inventing a response.

---

## 10. Challenges, Limitations and Future Work

### 10.1 Challenges Resolved

- **Role separation:** Student and administrator actions were separated with Identity roles and controller authorisation.
- **Search and filtering:** Marketplace and admin filtering were implemented through query parameters and service/repository logic.
- **Relational data:** User-owned content, reviews and reports were mapped with foreign keys and deletion rules.
- **Deployment configuration:** The application was structured so database configuration can be supplied by the hosting environment.
- **Maintainability:** Repositories, a marketplace service and dependency injection separate responsibilities and make later changes easier.

### 10.2 Current Limitations

- The React/Vite files in the repository are a separate front-end prototype; the implemented database-backed solution is the ASP.NET Core MVC application under the `CampusConnect` folder. The final demonstration should use the actual chosen deliverable consistently.
- The GitHub Actions deployment sections contain provider-specific placeholders until the hosting provider and secrets are configured.
- Image uploads, favourites and real-time messaging should not be described as fully operational unless they are tested in the final hosted environment.
- Swagger is configured for Development, so the production API documentation exposure should be decided deliberately.

### 10.3 Future Enhancements

1. Complete secure image upload and hosted object storage.
2. Add favourites and saved searches.
3. Add in-app messaging with moderation and abuse controls.
4. Add password reset and email verification workflows.
5. Add controller and integration tests against a test database.
6. Add audit logs for administrator actions.
7. Add API rate limiting and pagination.
8. Improve deployment automation with a real Azure or selected-provider action.

---

## 11. Teamwork and Individual Contribution

The recorded team members are:

| Member | Student number | Contribution evidence to attach |
|---|---|---|
| Thabang Kobe | ST10440553 | Add actual branch, commits, pull requests and delivered features. |
| Wade Rowe | ST10439060 | Add actual branch, commits, pull requests and delivered features. |
| Shuaib Mohamed | ST10437501 | Add actual branch, commits, pull requests and delivered features. |
| Darian Nair | ST10445414 | Add actual branch, commits, pull requests and delivered features. |
| Muhammed Suliman | ST10433999 | Add actual branch, commits, pull requests and delivered features. |

Each member should personalise their individual submission with their actual contribution. GitHub evidence should support the description, but contribution should not be claimed based only on commit count because design, testing, documentation and coordination may also be valid contributions.

---

## 12. Conclusion

CampusConnect delivers a structured campus platform for student commerce, accommodation and services. The solution demonstrates a complete web application architecture, relational data modelling, authentication, role-based authorisation, moderation, API access, automated testing and a documented development workflow.

The project is suitable for final demonstration once the live hosting URL, repository evidence and actual CI/CD deployment status have been confirmed. The limitations listed in this report provide a transparent basis for future development and prevent unsupported claims about features that still require production verification.

---

## 13. Submission Evidence Checklist

- [ ] Cover page completed.
- [ ] Table of contents updated after exporting to PDF.
- [ ] GitHub repository URL added.
- [x] Live application URL added: https://campusconnect-and4cuhjevaud8ed.uaenorth-01.azurewebsites.net
- [ ] Team names and student numbers checked.
- [ ] Actual individual contribution added.
- [ ] Task 1 and Task 2 feedback addressed.
- [ ] Final report included in the repository.
- [ ] User guide included in the repository.
- [ ] Authenticity declaration attached.
- [ ] Peer evaluations attached for each team member.
- [ ] Presentation slides or recording link attached.
- [ ] Screenshots of the working application included.
- [ ] GitHub branches, commits, pull requests and Actions evidence included.
- [ ] Hosting and database connection tested.
- [ ] Outstanding features and limitations stated honestly.
