# TaskService Microservice

## Solution
- Solution file: `TaskService.slnx` (XML-based `.slnx` format, NOT `.sln`)
- Do NOT create additional `.slnx` or `.sln` files

## Projects
- `TaskService/` — ASP.NET Core Web API (.NET 10)
- `TaskService.Tests/` — xUnit test project with NSubstitute for mocking

## Architecture
- **Repository pattern:** Controllers depend on `ITaskRepository` (in `Interfaces/`), implemented by `TaskRepository` (in `Repositories/`)
- **DI registration:** Done in `Program.cs` via `builder.Services.AddScoped<ITaskRepository, TaskRepository>()`
- **Database:** Entity Framework Core with SQLite (`Data/AppDbContext.cs`), uses EF Core migrations (`Migrations/`)

## Commands
- Build: `dotnet build TaskService.slnx`
- Test: `dotnet test TaskService.slnx`
- Run: `dotnet run --project TaskService`
- API endpoints:
  - `POST /api/tasks` with JSON body `{ "title": "...", "status": "..." }`
  - `GET /api/tasks/{id}` — returns a task by ID
