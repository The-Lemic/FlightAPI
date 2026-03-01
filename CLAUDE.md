# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

FlightAPI is an ASP.NET Core Web API (.NET 10.0) that provides flight and location data management. The API uses Entity Framework Core with PostgreSQL for data persistence and is containerized for deployment to Azure Container Apps.

## Architecture

### Domain Model

The application centers around two main entities with a relationship:

- **Location**: Represents airports/locations with IATA codes, city, country, and geographic coordinates (latitude/longitude)
- **Flight**: Represents flights with a flight number, departure/arrival locations (foreign keys to Location), departure/arrival times, and distance

The Flight entity has two navigation properties (`Departure` and `Arrival`) that reference the Location entity via `DepartureId` and `ArrivalId` foreign keys.

### Project Structure

```
FlightDataProvider/
├── Controllers/       - API controllers (FlightsController, LocationsController)
├── Data/             - EF Core DbContext (FlightDataContext)
├── DTOs/             - Data Transfer Objects for API requests (Create/Update DTOs)
├── Models/           - Domain entities (Flight, Location)
├── Migrations/       - EF Core database migrations
└── Program.cs        - Application entry point and service configuration
```

### Key Architectural Decisions

1. **Primary Constructor Injection**: Controllers and DbContext use C# primary constructors (e.g., `FlightsController(FlightDataContext context)`)

2. **DTO Pattern**: Separate DTOs for Create and Update operations that exclude the `Id` property to prevent client-side ID manipulation

3. **JSON Serialization**: Configured with `ReferenceHandler.IgnoreCycles` in Program.cs:8-12 to handle the circular references between Flight and Location entities

4. **Eager Loading**: Controllers use `.Include()` to eagerly load related Location entities when querying Flights to avoid N+1 queries

5. **Automatic Migrations**: Database migrations run automatically on startup (Program.cs:22-26) using `db.Database.Migrate()`

## Development Commands

### Build and Run

```bash
# Run the application locally
dotnet run --project FlightDataProvider/FlightDataProvider.csproj

# Build the project
dotnet build FlightDataProvider/FlightDataProvider.csproj

# Build for release
dotnet build FlightDataProvider/FlightDataProvider.csproj -c Release
```

### Database Migrations

```bash
# Add a new migration
dotnet ef migrations add <MigrationName> --project FlightDataProvider

# Remove the last migration (if not applied)
dotnet ef migrations remove --project FlightDataProvider

# Update database manually (not needed on app startup)
dotnet ef database update --project FlightDataProvider
```

### Docker

```bash
# Build Docker image (run from repository root)
docker build -f FlightDataProvider/Dockerfile -t flightdataprovider .

# Run container
docker run -p 8080:8080 -p 8081:8081 flightdataprovider
```

Note: The Dockerfile expects to be run from the repository root, not from the FlightDataProvider directory.

## Configuration

### Database Connection

The application uses PostgreSQL. Connection strings are configured via:
- `appsettings.json` or `appsettings.Development.json` with key `ConnectionStrings:DefaultConnection`
- User Secrets (ID: `09adb675-d2d8-4f22-8116-26f4d6f35932`) for local development
- Azure Container Apps environment variables for production

### API Endpoints

All controllers use the route pattern `api/[controller]`:
- `/api/flights` - Flight operations (GET, GET/{id}, POST, PATCH/{id}, DELETE/{id})
- `/api/locations` - Location operations (GET, GET/{id}, POST, PATCH/{id}, DELETE/{id})

OpenAPI/Swagger is available in development mode at `/openapi/v1.json`.

## Deployment

The application auto-deploys to Azure Container Apps on push to the `master` branch via GitHub Actions workflow. The workflow:
1. Builds a Docker image using `FlightDataProvider/Dockerfile`
2. Pushes to Azure Container Registry (`flightdataacr.azurecr.io`)
3. Deploys to Container App `flightdata-api` in resource group `flightdata-rg`
