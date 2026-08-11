# Project Management App

Simple personal task and project management system built with .NET, Blazor, ASP.NET Core, and Aspire.

The application currently contains a Blazor web frontend, an API service, shared Aspire service defaults, and an Aspire app host that wires the services together.

## Tech Stack

- .NET 10
- ASP.NET Core
- Blazor interactive server components
- .NET Aspire app hosting and service discovery
- Bootstrap

## Solution Structure

```text
AppHost.sln
+-- AppHost.AppHost          Aspire orchestration project
+-- AppHost.ApiService       ASP.NET Core API service
+-- AppHost.Web              Blazor web frontend
+-- AppHost.ServiceDefaults  Shared Aspire defaults
+-- Shared                   Shared class library
```

## Features

- Blazor web UI with Home, Counter, Weather, and Login pages.
- API service with sample weather endpoint.
- Login API endpoint that returns a temporary JWT-shaped token.
- Browser `localStorage` token persistence.
- Custom Blazor authentication state provider.
- Blazor route guard for pages marked with `[RequireAuthentication]`.
- Aspire service discovery between the web app and API service.

## Prerequisites

- .NET 10 SDK
- An IDE such as Visual Studio, Rider, or VS Code
- Trusted ASP.NET Core development certificate if you run HTTPS endpoints directly

To trust the local development certificate:

```powershell
dotnet dev-certs https --trust
```

## Run the App

The recommended way is to run the Aspire app host:

```powershell
dotnet run --project AppHost.AppHost/AppHost.AppHost.csproj
```

## Local configuration

Create your local environment file before starting the application:

```bash
cp .env.example .env
```

Open `.env` and replace the sample database connection values with your local
SQL Server credentials. The API loads this file only in the Development
environment. Environment variable names use double underscores for nested .NET
configuration keys; for example, `ConnectionStrings__DefaultConnection` maps to
`ConnectionStrings:DefaultConnection`.

The `.env` file is ignored by Git. Keep `.env.example` free of real passwords
and update it whenever a new required configuration value is introduced.

Alternatively, developers can use .NET User Secrets:

```bash
dotnet user-secrets set "ConnectionStrings:DefaultConnection" \
  "Server=localhost,1433;Database=ProjectManagementApp;User Id=project_app;Password=replace-me;TrustServerCertificate=True;" \
  --project AppHost.ApiService
```

The Aspire dashboard will show the running `webfrontend` and `apiservice` projects, including their available URLs.

You can also run projects individually:

```powershell
dotnet run --project AppHost.ApiService/AppHost.ApiService.csproj
dotnet run --project AppHost.Web/AppHost.Web.csproj
```

Default launch profile URLs:

- API HTTP: `http://localhost:5333`
- API HTTPS: `https://localhost:7349`
- Web HTTP: `http://localhost:5281`
- Web HTTPS: `https://localhost:7024`

## API Endpoints

### Health

```http
GET /health
```

### Weather

```http
GET /weatherforecast
```

Returns sample weather forecast data.

### Login

```http
POST /api/auth
Content-Type: application/json

{
  "username": "demo",
  "password": "demo"
}
```

Returns a temporary token:

```json
{
  "token": "..."
}
```

The login logic is currently a placeholder and should be replaced with real user validation before production use.

## Web Routes

- `/` - Home page
- `/login` - Login page
- `/counter` - Protected counter page
- `/weather` - Weather page

Protected pages use the custom `[RequireAuthentication]` attribute and are checked by `GuardedRouteView`.

## Authentication Notes

The web app stores the login token in browser `localStorage`. Because server middleware cannot read browser `localStorage` on page refresh, protected Blazor pages are guarded in the Blazor route layer instead of ASP.NET Core endpoint authorization middleware.

Important files:

- `AppHost.Web/Services/AuthService.cs`
- `AppHost.Web/Services/TokenService.cs`
- `AppHost.Web/Services/LocalStorageService.cs`
- `AppHost.Web/Authentication/CustomAuthenticationStateProvider.cs`
- `AppHost.Web/Authentication/RequireAuthenticationAttribute.cs`
- `AppHost.Web/Components/GuardedRouteView.razor`
- `AppHost.Web/Components/Routes.razor`
- `AppHost.ApiService/Controllers/AuthController.cs`

In development, the web app uses `http://apiservice` for the login API client to avoid local HTTPS certificate trust issues between services. Production uses `https+http://apiservice`.

## Build

Build the full solution:

```powershell
dotnet build
```

Build individual projects:

```powershell
dotnet build AppHost.ApiService/AppHost.ApiService.csproj
dotnet build AppHost.Web/AppHost.Web.csproj
```

## Troubleshooting

### `The SSL connection could not be established`

If the error mentions `UntrustedRoot`, trust the local development certificate:

```powershell
dotnet dev-certs https --trust
```

The web login client is configured to use HTTP service discovery in Development to avoid this issue.

### `IAuthenticationService` is missing

Do not use ASP.NET Core `[Authorize]` middleware for localStorage-only Blazor route protection. Use `[RequireAuthentication]` on the page and let `GuardedRouteView` check the Blazor authentication state.

### JavaScript interop during prerender

`localStorage` requires JavaScript interop, which is not available during static prerender. `Routes.razor` disables prerendering:

```razor
@rendermode @(new InteractiveServerRenderMode(prerender: false))
```

### `dotnet watch` exits with `-532462766`

This usually means the app threw an unhandled startup exception. Ensure all service registrations, such as `AddControllers()`, happen before `builder.Build()`.

### `/api/user` does not work

The active login endpoint is:

```http
POST /api/auth
```

Use `/api/auth` for login requests unless a separate user controller is added.

## Development Notes

- Current authentication is for development only.
- The API currently returns a fake JWT-shaped token.
- Replace placeholder login logic with a real user store, password validation, and signed JWTs before production.
- Add tests once the authentication and project/task domain model are finalized.
