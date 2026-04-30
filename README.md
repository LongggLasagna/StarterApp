---
title: "StarterApp readme"
parent: StarterApp
grand_parent: C# practice
nav_order: 5
mermaid: true
---

# StarterApp

## Project Overview

StarterApp is a .NET MAUI rental marketplace application built for the SET09102 coursework. The application allows users to authenticate, create item listings, browse available rental items, request rentals, manage rental workflows, search for nearby items using GPS/radius search, and leave reviews after completed rentals.

The project extends the original StarterApp foundation with backend API integration, item management, rental workflow management, location-based discovery, reviews, automated testing, CI/CD, code coverage reporting, and Doxygen-generated documentation.

Key implemented features include:

- User login and registration through the backend API
- JWT-authenticated API requests
- Item creation, browsing, details, and editing
- Incoming and outgoing rental request management
- Rental workflow states: Requested, Approved, Rejected, Out for Rent, Returned, Completed
- Review submission after completed rentals
- Location-based item discovery using GPS coordinates and radius search
- MVVM architecture
- Repository pattern
- Service layer
- xUnit testing
- GitHub Actions CI/CD
- Codecov coverage reporting
- Doxygen HTML documentation

---

## Setup Instructions

### Prerequisites

Install the following before running the project:

- .NET SDK 10.0 preview
- .NET MAUI workload
- Visual Studio 2022 or Visual Studio Code
- Android emulator or Android device for running the MAUI app
- Docker
- PostgreSQL/PostGIS Docker image, if running database features locally

Restore Dependencies:

dotnet restore StarterApp.sln

Restore MAUI workloads:

dotnet workload restore StarterApp/StarterApp.csproj

Docker/Database Setup

A postgreSQL/PostGIS database can be started with:

docker run --name starterapp-postgis \
   -e POSTGRES_USER=test_user \
   -e POSTGRES_PASSWORD=test_password \
   -e POSTGRES_DB=test_db \
   -p 5432:5432 \
   -d postgis/postgis:16-3.4

Alternatively if accessed through visual studio code, with postgresSQL extension user can connect with the above as their data and connect that way.

The GitHub Actions workflow ALSO starts a PostGIS service container automatically

Example of local database config:
{
  "ConnectionStrings": {
    "DevelopmentConnection": "Host=localhost;Port=5432;Database=test_db;Username=test_user;Password=test_password"
  }
}

How to run the application

Build the MAUI app:

dotnet build StarterApp/StarterApp.csproj

Run the application using visual studio with an android emulator or android device.

Main application project:

StarterApp/StarterApp.csproj

How to run tests

The xUnit test project is:

StarterApp.Tests/StarterApp.Tests.csproj

Run tests:

dotnet test StarterApp.Tests/StarterApp.Tests.csproj

Run tests with coverage:

dotnet test StarterApp.Tests/StarterApp.Tests.csproj --collect:"XPlat Code Coverage"

Coverasge files are under: 

StarterApp.Tests/TestResults/ (also generated through CI/CD Workflow)

API Endpoint Documentation

The app uses the following backend API endpoints:

POST /auth/token
POST /auth/register
GET /users/me

GET /items
GET /items/{id}
POST /items
PUT /items/{id}
GET /items/nearby

POST /rentals
GET /rentals/incoming
GET /rentals/outgoing
PATCH /rentals/{id}/status

POST /reviews
GET /items/{id}/reviews

Backend API base URL:

https://set09102-api.b-davison.workers.dev/

Main API integration files:

StarterApp/Services/ApiService.cs
StarterApp/Services/ApiAuthenticationService.cs
StarterApp/Services/ApiItemRepository.cs
StarterApp/Services/ApiRentalRepository.cs

Architecture Overview

The application uses a layered MVVM architecture:

Views → ViewModels → Services → Repositories/API → Backend/Database

Views are XAML pages responsible for UI display.

ViewModels contain bindable properties and commands.

Services contain business logic such as rental workflow validation, review validation, API communication, authentication, navigation, and location access.

Repositories abstract data access. The app uses both local database repositories and API-backed repositories.

Important folders:

StarterApp/Views
StarterApp/ViewModels
StarterApp/Services
StarterApp.Database/Models
StarterApp.Database/Data/Repositories
StarterApp.Tests

Rental workflow states:

Requested → Approved → Out for Rent → Returned → Completed
Requested → Rejected

Location-based discovery uses LocationService to retrieve GPS coordinates and calls the API nearby endpoint using latitude, longitude, and radius.

Testing and CI/CD

The test project is structured as:

StarterApp.Tests/
├── ViewModels/
├── Services/
├── Repositories/
└── Fixtures/

The GitHub Actions workflow is located at:

.github/workflows/build.yml

The workflow:

Restores dependencies
Builds the database project
Starts a PostGIS service container
Runs xUnit tests
Generates code coverage
Uploads coverage to Codecov
Generates an HTML coverage report
Generates Doxygen HTML documentation
Uploads workflow artifacts

Workflow artifacts include:

test-results
coverage-html-report
doxygen-docs
Code Documentation

Public classes, services, repositories, models, and interfaces include XML documentation comments.

Doxygen is run in the GitHub Actions workflow to generate HTML documentation. The generated documentation is uploaded as the doxygen-docs artifact. After downloading the artifact, open:

index.html
References
.NET MAUI Documentation: https://learn.microsoft.com/dotnet/maui/
Entity Framework Core Documentation: https://learn.microsoft.com/ef/core/
PostgreSQL Documentation: https://www.postgresql.org/docs/
PostGIS Documentation: https://postgis.net/documentation/
xUnit Documentation: https://xunit.net/
GitHub Actions Documentation: https://docs.github.com/actions
Codecov Documentation: https://docs.codecov.com/
Doxygen Documentation: https://www.doxygen.nl/
CommunityToolkit.Mvvm Documentation: https://learn.microsoft.com/dotnet/communitytoolkit/mvvm/
SET09102 Tutorial Materials: https://edinburgh-napier.github.io/SET09102/tutorials/csharp/