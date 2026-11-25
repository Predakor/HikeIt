# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

HikeIt is a hiking trip planner/analyzer: ASP.NET Core (.NET 10) API + React SPA, PostgreSQL via EF Core. Users upload GPX files; the backend runs them through a processing pipeline to produce trip analytics (distance, elevation, time, terrain, pace, reached peaks).

## Commands

Backend (solution `HikeIt.sln`, run from the repo root):

```
dotnet build
dotnet test                                                    # Core.Tests + Domain.Tests
dotnet test Domain.Tests --filter "FullyQualifiedName~PeakFilterTests"   # single class / method
dotnet run --project Api
```

Frontend (`Frontend/`, pnpm, Node >= 22.22):

```
pnpm install
pnpm dev        # Vite dev server
pnpm build      # tsc --b && vite build
pnpm lint
```

Frontend env: `.env` with `VITE_API_ORIGIN=http://localhost:5063` and `VITE_API_PREFIX=api`; the Vite dev server runs on port 54840. Backend secrets (connection string, CORS, storage, seeding) go in user secrets / env vars. First-time setup of a clone, including the hooks and the local database container: `docs/getting-started.md`.

Formatting: C# is formatted with CSharpier (`dotnet csharpier format <path>`), the frontend with prettier. The pre-commit hook does both on staged files, and CSharpier is a pinned fork (see `docs/conventions.md`), so don't swap in a stock version or run another C# formatter.

Migrations: use the `ef-migration` skill; never call `dotnet ef migrations add` directly or hand-write migrations.

## Architecture

Layered solution with one-way references: `Api` -> `Application` -> `Domain`; `Infrastructure` implements Domain/Application interfaces; `Core` is referenced by every project (via `Directory.Build.props`).

- **Core**: cross-cutting primitives. `Result`/`ResultError` for error flow (no exceptions for expected failures), validators, extensions, and `Serialization` (the single place for JSON options and converters). `Core.Results/Validators/Extentions/Abstractions` are global usings in every project except Core.
- **Domain**: aggregates and pure logic, no I/O. `Trips` holds the `Trip` aggregate and `Analytics` (elevation profiles, peaks, route, time, visualization). GPX parsing and the pipeline filters are unit-tested here (`Domain.Tests/Pipeline`).
- **Application**: use cases grouped by vertical slice (`Trips`, `Users`, `Locations`, `ReachedPeaks`, `FileReferences`, `AppSettings`), each with its own commands, queries, DTOs and services. `Trips/Analytics/Pipeline` is the GPX processing pipeline: staged filters whose parameters resolve from DB-backed `AppSettings`, falling back to code constants.
- **Infrastructure**: EF Core `TripDbContext`, per-aggregate configurations, repositories, migrations (`Infrastructure/Migrations`), blob storage.
- **Api**: controllers in `Controllers/<Area>` plus minimal-API groups in `Endpoints` (peaks, regions, admin pipeline preview). Migrations are applied and data seeded on startup (`DbHelpers.TryMigrationAndSeeding`).
- **Frontend**: Vite + React 19, Chakra UI v3, TanStack Query, React Router 7, MapLibre. Feature code lives in `src/modules` (e.g. `PipelineBench`, the admin pipeline preview UI) and `src/pages`.

### Linked resources

Some endpoints return a resource URL (`Uri?`) instead of an id or the embedded data, so the client fetches the detail on demand. Example: `TripAnalyticsDto.Linked` carries `trips/{id}/analytics/peaks|elevation|visualisation` URLs, built in the DTO with `ToResoutceUrl`. A null link means that resource doesn't exist for the trip. New heavy or optional sub-resources should follow this pattern rather than inlining the payload.

### Frontend data flow

- Server state is TanStack Query. For a returned resource URL use `useResourceLink<T>(url)` (`hooks/Api`); it normalizes the leading `/api` and keys the query as `["resource", url]`. `usePrefetch` prefetches the same keys.
- Don't hand-roll loading/error branches: pass the query result to `FetchWrapper` (`components/Utils/Fetching`), which handles loading, error and empty states and gives the data to `children(data)` or `Component`.
- Routing is React Router 7 driven by a custom config: routes are declared in `src/data/routes/routes.ts` (single pages or groups) and rendered by `RenderRoutes`. Add pages there, not as inline `<Route>`s.

Pipeline config: `GET/POST api/trips/{id}/analytics/pipeline/{defaults,preview}` (Admin role) lets the frontend bench run the full pipeline with config overrides without persisting.

## Gotchas

- Local DB is the `postgres_db` container (database `postgres`, port 5432), but `DesignTimeDbContextFactory` hardcodes port 54322 outside Production, so `dotnet ef database update` needs the connection string overridden:
  `ConnectionStrings__TripDbCS="Host=127.0.0.1;Port=5432;Database=postgres;Username=postgres;Password=postgres" ASPNETCORE_ENVIRONMENT=Production dotnet ef database update --project Infrastructure --startup-project Infrastructure`
- `AppSettings` rows are seeded once from code defaults and cached. After changing the shape of a filter list, `delete from "AppSettings"` and restart.
- The Peaks CSV is hand-copied from Google Maps (Wikipedia coordinates are wrong) and is incomplete; do not "fix" it from Wikipedia.
- Analyzers run at `AnalysisMode=recommended` plus SonarAnalyzer; CA1707 applies to naming (see conventions).
- `.claude/vs-*.ps1` hooks and `.mcp.json` are Visual Studio integration; the hooks no-op if the script is missing.
- Production runs on a self-hosted Dokploy on a custom VPS (API image from `Api/Dockerfile`, `docker-compose.prod.yml`).

## Conventions

Full rules and rationale: `docs/conventions.md`. Architecture detail (pipeline, adding a filter): `docs/architecture.md`.

- **Errors are `Result<T>`, not exceptions.** Throw only for states that should never happen (programmer errors); anything a user, file or missing row can cause returns an `Errors.*` result.
- Validate with rules (`IRule`, `AbstractValidator`) where possible; put them in a decorator that implements the service interface plus the `IValidatable<TService>` marker (`ICacheable<TService>` for caches); `DecorateMarked` applies them automatically. Don't use the `ICommand` pattern for new code, it's considered the wrong abstraction.
- Services register through a marker on the interface: `IService` (scoped) or `ISingletonService`. An interface without the marker is not registered; decorators are applied via `IValidatable` / `ICacheable`, not registered.
- Run `sh .githooks/setup.sh` once per clone to install the hooks and restore tools: pre-commit formats (CSharpier for C#, prettier for the frontend), pre-push builds, tests and lints.
- Vertical slices: put new features in their own folder per layer, not into shared buckets.
- All JSON work goes through `Core.Serialization`; add missing helpers there, no local converters or naming policies.
- Explicit access modifiers everywhere. Primary constructor parameters stay camelCase; `_` prefix only on declared private fields.
- Generic type parameters are `T` + name (`TFilter`, `TStage`), never bare `F`/`S`/`K`.
- Prefer handler maps (object/dictionary) over `switch` when dispatching on a key to behavior; a plain `switch` is fine for simple cases. Name complex `if` conditions in a clearly named `const`/local; `if` always has braces.
- Comments: one short line max, only for a non-obvious why. No class-level doc paragraphs. Public members get a one-line `/// <summary>Short sentence.</summary>`.
- Frontend: no `for-of`, no `delete`; use `<Show>` / `&&` for conditional rendering; reuse existing helpers before writing new ones.
- Tests cover core business logic (filter outputs, analytics), not casing, DI wiring or config plumbing.
