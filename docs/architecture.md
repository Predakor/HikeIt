# Architecture

## Layers

```
Api -> Application -> Domain
Infrastructure implements Application/Domain interfaces
Core is referenced by everything
```

| Project | Owns |
|---|---|
| `Core` | `Result`/`ResultError`, validators and rules, extensions, `Serialization`, timing. No domain knowledge. |
| `Domain` | Aggregates (`Trip`, `User`, ...), value objects, domain events, repository interfaces, pure analytics and filters. No I/O. |
| `Application` | Use cases per vertical slice: commands, queries, DTOs, services, event handlers, the pipeline orchestration. |
| `Infrastructure` | `TripDbContext`, entity configurations, repositories, migrations, blob storage. |
| `Api` | Controllers (`Controllers/<Area>`), minimal-API groups (`Endpoints`), DI setup (`Commons/DI`), HTTP mapping of `Result`. |
| `Frontend` | Vite + React SPA. |

Every project except `Core` gets `Core.Results`, `Core.Validators`, `Core.Extentions` and `Core.Abstractions` as global usings (`Directory.Build.props`).

## Request flow

Controller or endpoint -> Application service/query -> Domain + repository -> `Result<T>` -> `ToApiResult`/`ToActionResult` -> HTTP. Errors never travel as exceptions; see [conventions](conventions.md#error-handling-backend).

Aggregates raise domain events (`Domain/Trips/Root/Events`); handlers in `Application` react to them (for example uploading an attached GPX file to storage, updating user stats when analytics are created).

## Trip analytics pipeline

Uploading a GPX file produces `TripAnalytic` data by running staged filter chains (`Application/Trips/Analytics/Pipeline`):

1. `CleanupStage`: point filters (spike removal, smoothing, rounding) over the raw points.
2. `ElevationProfileStage`, `RouteStage`, `TerrainStage`: route metrics, elevation profile, terrain segments.
3. `PaceStage`: idle/grade/speed segments over time data.
4. `PeakMatchingStage`: local maxima, merging and matching against the Peaks table.

Pieces:

- **Filters** (`Domain/Trips/Analytics/Shared/Filters`, plus `Route`/`Time` filters) are small `IFilter<TState>` steps. A family shares one state type, and a chain applies them in order (`FilterChain`).
- **Configs**: each filter has a nested `Config` that creates it (`IFilterConfig<TState>.CreateFilter`). Configs are polymorphic JSON with a `filterType` discriminator, grouped by family interface (`IPointFilterConfig`, `IPeakFilterConfig`, `IPaceFilterConfig`, `ITerrainFilterConfig`). A new filter must be registered in those `JsonDerivedType` lists.
- **Settings**: a stage resolves its config from DB-backed `AppSettings` (editable by admins), and falls back to code constants in `PipelineDefaults` when none is stored.
- **Preview bench**: `api/trips/{id}/analytics/pipeline/defaults|preview` (Admin) runs the full pipeline with config overrides and returns per-stage output and timings without saving. The frontend module is `Frontend/src/modules/PipelineBench`.

Adding a filter: implement `IFilter<TState>` (or `SyncFilter<TState>`) with a nested `Config`, add it to the right family interface and to the `JsonDerivedType` lists, add it to the stage's defaults, and add a test in `Domain.Tests/Pipeline`.

## Linked resources

Heavy or optional sub-resources are returned as URLs rather than inlined or referenced by id. The trip analytics DTO has `Linked`, `Basic` and `Full` shapes, and `Linked` carries URLs like `trips/{id}/analytics/peaks`. The client fetches them lazily with `useResourceLink`.

## Reads and writes

- **Writes** go through aggregates. Repository interfaces live in Domain (`ITripRepository`, ...) and are implemented in Infrastructure.
- **Reads** skip the aggregates and return flat DTOs. The query interface and its DTOs live in Application (`IPeaksQueryService`), and the implementation lives in Infrastructure (`PeaksQueryService`), because it uses `TripDbContext` directly. Application never references EF.
- A query service may also implement a Domain port, for example `PeaksQueryService` implements `IPeakSource`, which the pipeline's peak matching depends on. Services are registered under every interface they implement, so such ports need no extra registration.

## Persistence

EF Core with PostgreSQL. Migrations are applied and seed data inserted at startup (`DbHelpers.TryMigrationAndSeeding`). Create migrations with the `ef-migration` skill. Locally the DB is the `postgres_db` container; see the gotchas in `CLAUDE.md`.

Reference data (regions, peaks) is seeded from CSV. The Peaks CSV is hand-curated from Google Maps and incomplete on purpose-built coordinates; don't replace it with Wikipedia data.

## Frontend

- Routing: React Router 7, driven by a route config in `src/data/routes` and rendered by `RenderRoutes`. Groups share a path prefix.
- Data: TanStack Query with a persisted client (localStorage). `src/Utils/Api` holds the `ApiClient`, request helpers and stale times; `useResourceLink`/`usePrefetch` in `hooks/Api` handle linked resources.
- `FetchWrapper` (`components/Utils/Fetching`) takes a query result and renders loading, error, empty, or the children with data.
- UI: Chakra UI v3, MapLibre for maps, Recharts / Chakra charts for graphs.
- Layout: `src/Layout`, with a separate `AdminLayout` for admin pages.
- Feature modules (`src/modules`) own bigger self-contained features such as app settings and the pipeline bench; pages in `src/pages` compose components.

## Deployment

Production runs on a custom VPS with a self-hosted Dokploy. The API builds from `Api/Dockerfile` (`docker-compose.prod.yml`, port 8080 exposed behind Dokploy's proxy, config from `.env`).