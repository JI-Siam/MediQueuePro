# MediQueuePro

MediQueuePro is a lightweight, production-oriented clinic queue and appointment management system built with ASP.NET Core. It provides role-based appointment booking, real-time queue tracking with token generation, emergency prioritization, and admin analytics to optimize patient flow.

## Key Features

- Appointment booking: patients can create, view, and manage appointments with doctors.
- Live queue & tokens: generates tokens and displays real-time queue positions.
- Emergency prioritization: urgent cases are automatically re-prioritized in the queue.
- Role-based access control: Admin, Doctor, and Patient roles enforced via authorization filters.
- Doctor dashboard: manage appointments, update statuses, and review patient details.
- Admin analytics: clinic metrics, appointment summaries, and basic reporting.
- Layered architecture: clear separation between Data Access (DAL), Business Logic (BLL), and Presentation (MVC).
- Repository + Service patterns: maintainable data access and business services.
- DTOs & validations: transfer objects and server-side validation for robust inputs.
- AutoMapper: entity ↔ DTO mapping configured in `BLL/MapperConfig.cs`.
- Responsive UI: Razor views and static assets located under `MvcAppLayer/wwwroot`.

## Technologies

- .NET 10 and C#
- ASP.NET Core MVC
- Entity Framework Core (DbContext + code-first entities)
- Repository Pattern & Service Layer
- AutoMapper
- Razor Views, HTML/CSS/JavaScript (client assets in `wwwroot`)
- Dependency Injection (built-in ASP.NET Core DI)
- Configuration via `appsettings.json` and environment-specific settings
- Layered solution layout with `MediQueueProSolution.slnx`

## Project Structure (high-level)

- `DAL/` — EF Core DbContext, entity models, and repositories
- `BLL/` — services, DTOs, validations, AutoMapper configuration
- `MvcAppLayer/` — MVC app: controllers, Razor views, static assets, and app configuration

## Quick Start

1. Restore NuGet packages and build the solution targeting .NET 10.
2. Configure your database connection in `MvcAppLayer/appsettings.json`.
3. Run the MVC app from Visual Studio or `dotnet run` inside `MvcAppLayer/`.

For details on deployment, database initialization, or contribution guidelines, see the project files and solution structure.