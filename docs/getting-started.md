# Getting started

First-time setup of a fresh clone. The README's "How To Run" section is outdated (it describes Supabase and .NET 8); this page is the current path.

## 1. Prerequisites

- .NET 10 SDK
- Node.js >= 20 and pnpm 10
- Docker Desktop (for the local database)
- Git with Git Bash. The git hooks are `sh` scripts and need `curl` and `dotnet` on the PATH.

## 2. Install the hooks and tools

From the repo root:

```
sh .githooks/setup.sh
```

This is required once per clone. It:

1. Points git at the versioned hooks (`core.hooksPath=.githooks`).
2. Restores the dotnet tools from `.config/dotnet-tools.json`. CSharpier is a fork that isn't on nuget.org, so `.githooks/restore-tools.sh` downloads the pinned package from the fork's GitHub Release into `~/.csharpier-expanded`.
3. Registers that folder as a machine-level NuGet source named `csharpier-expanded`. The Visual Studio CSharpier extension installs the pinned version from NuGet sources, so without this source it shows "CSharpier could not be set up properly".

Restart Visual Studio after the first run so the extension picks the tool up. Details of the formatter and the hooks are in [conventions.md](conventions.md#git-hooks-and-line-endings).

Then install the frontend dependencies:

```
cd Frontend
pnpm install
```

## 3. Database

The local database is a PostGIS container defined in `docker-compose.override.yml`. `docker compose` merges that file with `docker-compose.yml` automatically, so start only the database with:

```
docker compose up -d postgres-db
```

| | |
|---|---|
| Container | `postgres_db` |
| Image | `postgis/postgis:18-3.6` |
| Port | `5432` |
| User / password | `postgres` / `postgres` |
| Data | named volume `pgdata` |

Local development data lives in the `postgres` database of that container. The compose file also sets `POSTGRES_DB=myappdb`, but the app is pointed at `postgres` (see the connection string below).

`docker compose up` with no service name also builds and starts the API image from `Api/Dockerfile`. You don't need that for development, so run the API from the IDE or `dotnet run`.

## 4. Backend configuration

Secrets are stored per developer with .NET user secrets (the project's `UserSecretsId` is set in `Directory.Build.props`). From the repo root:

```
dotnet user-secrets set "ConnectionStrings:TripDbCS" "Host=127.0.0.1;Port=5432;Database=postgres;Username=postgres;Password=postgres" --project Api
dotnet user-secrets set "Cors:AllowedOrigins" "http://localhost:54840" --project Api
dotnet user-secrets set "Storage:Account" "<azure storage account name>" --project Api
dotnet user-secrets set "Storage:Key" "<azure storage account key>" --project Api
dotnet user-secrets set "Seeding:PeaksUrl" "<url of the peaks CSV>" --project Api
dotnet user-secrets set "Seeding:RegionsUrl" "<url of the regions data>" --project Api
dotnet user-secrets set "Seeding:Users:Admin" "{\"Id\":\"<guid>\",\"UserName\":\"admin\",\"FirstName\":\"Admin\",\"LastName\":\"User\",\"Email\":\"admin@example.com\",\"Password\":\"<password>\"}" --project Api
dotnet user-secrets set "Seeding:Users:Demo" "{\"Id\":\"<guid>\",\"UserName\":\"demo\",\"FirstName\":\"Demo\",\"LastName\":\"User\",\"Email\":\"demo@example.com\",\"Password\":\"<password>\"}" --project Api
```

Notes:

- `Cors:AllowedOrigins` is a comma-separated list. It must include the frontend origin (the Vite dev server runs on port `54840`).
- File storage is Azure Blob Storage only; there is no local emulator. The account URL is built as `https://<account>.blob.core.windows.net`, so uploads and avatars need a real account.
- The peaks and regions seed data is read from the URLs above on first start, when the tables are empty. The peaks CSV is hand-curated from Google Maps and incomplete.
- In production the same keys are environment variables with `__` instead of `:` (for example `ConnectionStrings__TripDbCS`).

## 5. Run

API (migrations are applied and data is seeded on startup):

```
dotnet run --project Api --launch-profile http
```

It listens on `http://localhost:5063`. There is also an `https` profile on `https://localhost:7278`; trust the dev certificate once with `dotnet dev-certs https --trust`.

Frontend. Create `Frontend/.env`:

```
VITE_API_ORIGIN=http://localhost:5063
VITE_API_PREFIX=api
```

then:

```
cd Frontend
pnpm dev
```

The app is at `http://localhost:54840`. The seeded demo user can log in with the "Login as Demo User" button.

## 6. Verify

```
dotnet build
dotnet test
```

## Troubleshooting

- **`dotnet ef database update` can't connect.** The design-time factory hardcodes port 54322 outside Production. Use the override from `CLAUDE.md`, or let the app apply migrations on startup. New migrations go through the `ef-migration` skill.
- **Settings or pipeline defaults look stale after a code change.** `AppSettings` rows are seeded once and cached. Run `delete from "AppSettings"` in the database and restart the API.
- **A commit or push is blocked.** The hooks print why. Common causes: a staged `.cs` or frontend file with unstaged changes, or a build, test or lint failure on push.
- **`dotnet csharpier` is not found.** Re-run `sh .githooks/setup.sh`.
