# StarterApp

## Project Overview

StarterApp is a .NET MAUI rental marketplace application built for the SET09102 coursework. The application allows users to authenticate, create item listings, browse available rental items, request rentals, manage rental workflows, search for nearby items using GPS/radius search, and leave reviews after completed rentals.

The project extends the original StarterApp foundation with backend API integration, item management, rental workflow management, location-based discovery, reviews, automated testing, CI/CD, code coverage reporting, and Doxygen-generated documentation.

### Key Implemented Features

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

Restore dependencies:

```bash
dotnet restore StarterApp.sln
```

Restore MAUI workloads:

```bash
dotnet workload restore StarterApp/StarterApp.csproj
```

---

## Docker / Database Setup

A PostgreSQL/PostGIS database can be started with:

```bash
docker run --name starterapp-postgis \
  -e POSTGRES_USER=test_user \
  -e POSTGRES_PASSWORD=test_password \
  -e POSTGRES_DB=test_db \
  -p 5432:5432 \
  -d postgis/postgis:16-3.4
```

Alternatively, when using Visual Studio Code, the PostgreSQL extension can be used to connect to the database using the same host, username, password, database name, and port.

The GitHub Actions workflow also starts a PostGIS service container automatically during CI/CD.

Example local database configuration:

```json
{
  "ConnectionStrings": {
    "DevelopmentConnection": "Host=localhost;Port=5432;Database=test_db;Username=test_user;Password=test_password"
  }
}
```

---

## How to Run the Application

Build the MAUI app:

```bash
dotnet build StarterApp/StarterApp.csproj
```

Run the application using Visual Studio with an Android emulator or connected Android device.

Main application project:

```text
StarterApp/StarterApp.csproj
```

---

## How to Run Tests

The xUnit test project is:

```text
StarterApp.Tests/StarterApp.Tests.csproj
```

Run tests:

```bash
dotnet test StarterApp.Tests/StarterApp.Tests.csproj
```

Run tests with coverage:

```bash
dotnet test StarterApp.Tests/StarterApp.Tests.csproj --collect:"XPlat Code Coverage"
```

Coverage files are generated under:

```text
StarterApp.Tests/TestResults/
```

Coverage is also generated automatically through the GitHub Actions CI/CD workflow.

---

## API Endpoint Documentation

The app uses the following backend API endpoints:

```text
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
```

Backend API base URL:

```text
https://set09102-api.b-davison.workers.dev/
```

Main API integration files:

```text
StarterApp/Services/ApiService.cs
StarterApp/Services/ApiAuthenticationService.cs
StarterApp/Services/ApiItemRepository.cs
StarterApp/Services/ApiRentalRepository.cs
```

---

## Architecture Overview

The application uses a layered MVVM architecture:

```text
Views → ViewModels → Services → Repositories/API → Backend/Database
```

### Views

Views are XAML pages responsible for displaying the user interface.

Example folders/files:

```text
StarterApp/Views
```

### ViewModels

ViewModels contain bindable properties and commands. They handle UI logic without placing business logic directly inside XAML code-behind files.

Example folder:

```text
StarterApp/ViewModels
```

### Services

Services contain business logic such as rental workflow validation, review validation, API communication, authentication, navigation, and location access.

Example folder:

```text
StarterApp/Services
```

### Repositories

Repositories abstract data access. The app uses both local database repositories and API-backed repositories.

Example folders:

```text
StarterApp.Database/Data/Repositories
StarterApp/Services/ApiItemRepository.cs
StarterApp/Services/ApiRentalRepository.cs
```

### Models

Shared models are stored in:

```text
StarterApp.Database/Models
```

Important models include:

```text
Item.cs
Rental.cs
RentalStatus.cs
Review.cs
User.cs
Role.cs
UserRole.cs
```

### Rental Workflow

Rental workflow states:

```text
Requested → Approved → Out for Rent → Returned → Completed
Requested → Rejected
```

Location-based discovery uses `LocationService` to retrieve GPS coordinates and calls the API nearby endpoint using latitude, longitude, and radius.

---

## Testing and CI/CD

The test project is structured as:

```text
StarterApp.Tests/
├── ViewModels/
├── Services/
├── Repositories/
└── Fixtures/
```

The GitHub Actions workflow is located at:

```text
.github/workflows/build.yml
```

The workflow:

- Restores dependencies
- Builds the database project
- Starts a PostGIS service container
- Runs xUnit tests
- Displays xUnit test results
- Generates code coverage
- Uploads coverage to Codecov
- Generates an HTML coverage report
- Generates Doxygen HTML documentation
- Uploads workflow artifacts

Workflow artifacts include:

```text
test-results
coverage-html-report
doxygen-docs
```

---

## Code Documentation

Public classes, services, repositories, models, and interfaces include XML documentation comments.

Doxygen is run in the GitHub Actions workflow to generate HTML documentation. The generated documentation is uploaded as the `doxygen-docs` artifact.

After downloading the artifact, open:

```text
index.html
```

---