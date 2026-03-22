# Tuna League Management API

A course-management style ASP.NET Core project for managing teams, players, coaches, matches, and player statistics using layered architecture, DTOs, validation, JWT authentication, and role-based authorization.

## How to Create and Run the Project

1. Install prerequisites:
- .NET SDK 9.0+
- MySQL Server


2. Create a similar project from scratch:
```bash
dotnet new mvc -n Tuna-SoccerLeague
cd Tuna-SoccerLeague
```

3. Add required NuGet packages:
```bash
dotnet add package Pomelo.EntityFrameworkCore.MySql
dotnet add package Microsoft.EntityFrameworkCore.Design
dotnet add package Microsoft.AspNetCore.Authentication.JwtBearer
dotnet add package Swashbuckle.AspNetCore
dotnet add package Swashbuckle.AspNetCore.Annotations
dotnet add package DotNetEnv
```

4. Configure application settings in appsettings.json:
- Add `ConnectionStrings:DefaultConnection`
- Add `Jwt` settings (`Key`, `Issuer`, `Audience`, `ExpiryMinutes`)

5. Set database password environment variable:
```bash
set DB_PASSWORD=your_password_here
```

6. Create and apply EF Core migrations:
```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

7. Run the app:
```bash
dotnet run
```

8. Open Swagger UI:
- `http://localhost:<port>/swagger`

## Technologies Used

- ASP.NET Core MVC + Web API: Framework used to build both MVC pages and REST-style endpoints.
- C# (.NET 9): Main programming language and runtime.
- Entity Framework Core: ORM used for modeling entities and database access.
- Pomelo.EntityFrameworkCore.MySql: EF Core provider for MySQL.
- MySQL: Relational database for persistent storage.
- MySqlConnector: MySQL connection and command execution library.
- Dependency Injection (built-in): Used to register and inject services and DbContext.
- Service Layer Pattern: Separates business logic from controllers for cleaner architecture.
- DTOs (Create/Update/Read): Keeps API contracts clean and prevents exposing entity models directly.
- Data Annotations Validation: Validates incoming request payloads before database operations.
- JWT Bearer Authentication: Issues and validates access tokens for authenticated API access.
- Role-Based Authorization: Restricts endpoints by roles such as `Admin` and `User`.
- LINQ Projections (`Select`): Returns only required fields to reduce query and payload size.
- `AsNoTracking()`: Improves performance for read-only EF Core queries.
- Swagger / OpenAPI (Swashbuckle): Auto-generates interactive API documentation and testing UI.
- DotNetEnv: Loads environment variables from `.env` for local development.

## Authentication Notes

The project currently authenticates with JWT access tokens in the `Authorization` header.

## Why HTTP-only Cookies Are an Industry Standard for Auth Security

HTTP-only cookies are commonly used because they reduce token theft risk in browser-based applications.

- They are not readable by JavaScript: this helps mitigate token theft during XSS attacks.
- They can be marked `Secure`: cookie is sent only over HTTPS.
- They can use `SameSite`: helps reduce CSRF attack surface.
- Browsers handle sending cookies automatically: less client-side token handling code.

Important note:
- HTTP-only cookies are not a complete security solution by themselves.
- Production systems usually combine them with HTTPS, CSRF protection, short token lifetimes, and token rotation.

## Current Project Security Behavior

- Login endpoint returns JWT access token.
- Clients send token via `Authorization: Bearer <token>`.
- GET endpoints are role-restricted for `User`.
- POST/PUT endpoints are role-restricted for `Admin`.

If needed, this project can be extended to store tokens in HTTP-only cookies and add refresh token rotation for stronger session security.
