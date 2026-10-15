# Pipeline preview bench: implementation plan

Issue: `issue.md` (same folder). Work happens on a feature branch off `master`.

## Conventions every task must follow

- Use Edit/Read/Grep for files; no scripted rewrites.
- `if` always has braces. Big conditions go in a clearly named `const`/local.
- Prefer handler maps (objects/dictionaries) over `switch`/`case`.
- Match the surrounding code's naming, comment density and idiom.
- Do not touch files outside the task's "Files" list; report instead of widening scope.

## Phase 0: contract (done first, then frozen)

Owner: main session. Output: C# DTOs, TS types, one JSON fixture. Backend and frontend both build
against these; any change goes through the main session.

### Request

```
PipelinePreviewRequest
  PipelineConfig Config
  StageId[]?     Stages      // null = all stages

StageId = cleanup | analytics | elevationProfile | peakMatching | route

PipelineConfig                      // every field nullable; null = production default
  CleanupConfig          Cleanup            { IFilterConfig[]? Filters }   // same filter vocabulary as Route; null = production list
  ElevationProfileConfig ElevationProfile   { IFilterConfig[]? Filters }   // PinLocalMaxima + Simplify keep summits; gains computed from the result
  PeakMatchingConfig     PeakMatching       { IFilterConfig[]? Filters, ProximityRadiusM? }   // peak filters: MergeNearbyByDistance(m), MergeNearbyByElevation(m); production order = distance, elevation; radius stays scalar (DB lookup)
  RouteConfig            Route              { IFilterConfig[] Filters }   // existing polymorphic filters + PinLocalMaxima + Simplify
```

### Response

```
PipelinePreviewResponse
  Dictionary<StageId, StageResult> Results     // preview config
  Dictionary<StageId, StageResult> Baseline    // defaults, same stages

StageResult
  StageId Stage
  bool    Ok
  string? Error
  bool    CacheHit
  int     DurationMs
  int     PointCount
  int     PayloadBytes
  object  Data               // shape depends on Stage, see below
```

`Data` per stage:

| Stage | Data |
|---|---|
| cleanup | `{ pointCountIn, pointCountOut }` |
| analytics | `{ route: <existing route analytics DTO>, time: <existing time analytics DTO> or null }` |
| elevationProfile | existing `ElevationProfileDto` (`start`, `gains[]`) |
| peakMatching | `{ candidateCount, matched: [{ peakId, name, distanceM, elevation }] }` |
| route | existing `RoutePath` (`points[]`) |

Dependency map (used by the backend orchestrator and mirrored in the UI):
every stage depends on `cleanup`; no other edges.

New filter configs (added to the `[JsonDerivedType]` list in `IFilter.cs`):
`PinLocalMaxima.Config` (value: bool, true = enabled) and `Simplify.Config` (value: double toleranceM).

### Files

- Backend: `Application/Trips/Analytics/Pipeline/Contracts/*.cs` (new)
- Frontend: `Frontend/src/types/Api/pipelinePreview.types.ts` (new), mirrors the DTOs 1:1
- Shared fixture: `docs/pipeline-preview-bench/fixture.response.json`, a complete sample response
  that the backend contract test deserialises and the frontend mock server serves.

Reuse existing DTO types for analytics, elevation profile and route path; do not redefine them.

### Phase 0 status (done)

- Naming: stage configs are nested in the static class `PipeConfig` (`PipeConfig.Cleanup`, `.Profile`, `.Peaks`,
  `.Route`); the top-level `PipelineConfig` keeps properties `Cleanup`, `ElevationProfile`, `PeakMatching`, `Route`; stage payloads are nested
  in `StageData` (`StageData.Cleanup`, `.Analytics`, `.PeakMatching`, `.MatchedPeak`). The TS types keep flat names
  (`CleanupConfig`, `PeakMatchingConfig`, ...); JSON is identical.
- Contract: `Application/Trips/Analytics/Pipeline/Contracts/{PipelineConfig,StageId,PipelineResult}.cs`,
  `Frontend/src/types/Api/pipelinePreview.types.ts`, `fixture.response.json` (wired into Domain.Tests).
- Response dictionaries are keyed by `StageIds.Key(stage)` (camelCase string), not by the enum.
- `IPipelinePreviewService` is defined in the contract; BE-3b implements it.
- Filter wire format follows the existing dev preview: `{ "FilterType": "SimplifyFilter", "value": { "ToleranceM": 3 } }`.
  New configs: `PinLocalMaximaFilter.Config(bool Enabled)`, `SimplifyFilter.Config(double ToleranceM)`.
- Stubs (throw `NotImplementedException`) exist for `PinLocalMaximaFilter` and `SimplifyFilter`, plus
  `MutableGpxPoint.Pinned`. BE-2 replaces the stubs.
- Tests live in `Domain.Tests` (no Api/Application test project exists; Application has
  `InternalsVisibleTo("Domain.Tests")` now). Written: `Pipeline/RouteFilterTests.cs`,
  `Pipeline/PipelineContractTests.cs`. Currently 11 failing, all BE-2.
- Path corrections: `IFilter.cs` is `Domain/Common/Abstractions/IFilter.cs`; `MutableGpxPoint` is in
  `Domain/Trips/Root/Builders/GpxDataBuilder/GpxDataBuilder.cs`. The domain peak settings type is named
  `PeakMatchingSettings` (the contract's `PeakMatchingConfig` holds nullable overrides).
- Still to write before the matching task starts (they need that task's signatures): BE-1 default-equals-old
  test, BE-3 no-persistence and cache tests, FE-2a staleness tests.

## Phase 1: parallel, after the contract is frozen

### BE-1: config extraction (Haiku-suitable, mechanical, test-guarded)

Files: `Domain/Trips/Root/Builders/Config/GpxDataConfigs.cs`,
`Domain/Trips/Root/Builders/GpxDataBuilder/GpxDataDirector.cs`,
`Application/ReachedPeaks/ReachedPeakService.cs`,
`Domain/Trips/Analytics/ElevationProfiles/Commands/FindLocalMaximas.cs`.

1. Add peak filters implementing `IFilter<GpxPointWithDistance>`: `MergeNearbyByDistanceFilter.Config(double DistanceM)`
   and `MergeNearbyByElevationFilter.Config(double ElevationDeltaM)`, wrapping the existing
   `MergeNearbyPeakByDistance` / `MergeNearbyPeaksByElevation`. The stubs, JSON registration and the
   `IPeakFilterConfig` marker already exist (Phase 0); BE-1 fills in `Apply`.
2. Replace `WithProximityMerge()` and `PeakProximityTreshold` with a default filter list
   `[MergeByDistance 50, MergeByElevation 50]` and radius 100, passed in so defaults equal the old values.
   Misplaced filters are rejected at deserialisation by the typed lists (`IPointFilterConfig` vs
   `IPeakFilterConfig`), so the orchestrator needs no validation for this.
3. Make `GpxDataDirector.AnalyticData(points, config)` use `config` (defaults = `GpxDataConfigs.GpxFile`).
4. Add a `PipelineConfig` defaults helper (resolves null fields to defaults).

Done when: `dotnet test` passes unchanged; a new test asserts default output equals the
pre-change output on the fixtures in `Domain.Tests/data`.

### BE-2: route filters (Sonnet-level; algorithmic)

Files: `Domain/Common/Geography/ValueObjects` (`MutableGpxPoint`),
`Domain/Trips/Analytics/Shared/Filters/` (two new files),
`Application/Commons/Abstractions/IFilter.cs`,
`Application/Trips/Analytics/RouteVisualizations/RouteVisualizationService.cs`,
`Domain.Tests/`.

1. Add `bool Pinned` to `MutableGpxPoint` (default false).
2. `PinLocalMaximaFilter`: sets `Pinned` on the points `ToLocalMaximaWithDistance()` finds, plus
   first and last point.
3. `SimplifyFilter(toleranceM)`: Ramer-Douglas-Peucker on local-plane coordinates
   (equirectangular projection) with elevation as a third axis; never removes a pinned point;
   returns the shorter list.
4. Register both in `GpxFilterFactory.CreateFilter` (handler map, not switch). JSON registration and the
   `IPointFilterConfig` marker are already done in Phase 0.
5. `CreateVisualization`: `points = filter.Apply(points)`.

Tests: plateau summit survives; straight track collapses to endpoints; `Pinned` survives later
filters; a point-removing filter shortens `RoutePath`; JSON round-trip of both configs.

### FE-1: types, mock, API hook (Haiku-suitable)

Files: `Frontend/src/types/Api/pipelinePreview.types.ts`,
`Frontend/src/components/Trip/Details/PipelineBench/api/*` (new).

1. Types from the contract (already written in Phase 0).
2. `usePipelinePreview(tripId)` mutation (follow `useRouteVisualizationMutations.ts`).
3. A mock layer that returns `fixture.response.json`, switchable by an env flag, so UI work does
   not wait for the backend.

### BE-1b: unify smoothing filters (after BE-1, Haiku)

Filter classes become the single implementation of clamp/median/EMA/round; builder methods delegate. Golden
hashes of current `GpxDataDirector` output must stay unchanged. EMA filter is elevation-only.

### BE-4: coordinate smoothing filter (after BE-1b, Haiku)

New opt-in `CoordinateSmoothingFilter.Config(double Alpha)` (EMA on Lat/Lon, off by default, not in any
default list because production does not smooth coordinates). Register on `IFilterConfig` and
`IPointFilterConfig`, add to the factory handler map and the TS `PointFilterConfig` union. Test: smooths a
noisy track; absent from `PipelineDefaults`.

## Phase 2: parallel, after Phase 1

### BE-3: orchestrator and endpoint (Sonnet-level; design-heavy)

Files: `Application/Trips/Analytics/Pipeline/*` (new),
`Api/Controllers/Trips/AnalyticsController.cs`, DI registration, `Api.Tests`/`Application.Tests`.

1. Stage functions `(upstream, config) -> output`, one per `StageId`, reusing the existing
   factories (`RouteAnalyticFactory`, `TimeAnalyticFactory`, elevation profile, peak lookup up to
   the DB match).
2. Orchestrator with the dependency map and an `IMemoryCache` keyed by
   `hash(stage config + upstream keys)` plus user id and file id; short TTL, size cap.
3. Endpoint `POST trips/{id}/analytics/pipeline/preview`, `[Authorize(Roles = "Admin")]`, loads
   the raw file as `DevAnalyticPreview` does today, computes requested stages and baseline.
4. Peak stage must not call `trip.AddReachedPeaks` or any repository write.

Tests: contract test deserialises `fixture.response.json`; no-persistence test (change tracker
clean, no `SaveChanges`); cache test (second run with only a route-filter change reports
`CacheHit = true` for cleanup and analytics); non-admin gets 403.

### FE-2: block UI (Sonnet or Haiku with the contract in hand)

Files: `Frontend/src/components/Trip/Details/PipelineBench/*` (new), admin-only tab entry in
`TripDetailsTabs`/`tabOrder`.

1. One block component per stage: config form (reuse `RenderInputs` and `VisualisationFilterMenu`),
   state (idle, stale, running, done, error).
2. Dependency map: editing a block marks it and dependents stale; request only stale stages;
   merge results into state per stage.
3. Debounced auto-run for pure stages; manual run button for peak matching.
4. Result views: stats table (baseline / preview / delta), elevation graph with baseline overlay
   (reuse `GenerateChartDataWithPreview`), route map (reuse `DevRouteVisualisation`), peaks list.
5. Warning when `Simplify` precedes `PinLocalMaxima`.

### FE-3: stats table and delta formatting (Haiku-suitable, small)

Pure presentational component from `StageResult` baseline/preview pairs; no state.

## Phase 3: integration

Run the real backend against the frontend, remove the mock flag, verify against 3 real trips and
one noisy edge-case file. Then: delete `DevPreview`, `PreviewGraphDevOnly`, `DevRouteVisualisation`
and the two old preview endpoints; set the production `RouteVisualizationSetting` default
tolerance (about 2-3 m, chosen with the bench).

## Execution notes

- Run each agent in its own worktree; the frozen contract files are read-only for them.
- All tasks run on Haiku (decision: values get tuned afterwards via the bench). To compensate for
  the judgement-heavy ones (BE-2, BE-3, FE-2):
  - the main session writes the failing tests first (simplifier cases, no-persistence, cache hit,
    stale-dependency logic) in Phase 0, and the agent's job is "make these pass without editing them";
  - BE-3 and FE-2 are run as two sub-tasks each (BE-3a stage functions, BE-3b orchestrator, cache
    and endpoint; FE-2a block state and dependency logic, FE-2b result views);
  - the main session reviews every diff against this plan before merging.
- Each task prompt = this task's section + the Conventions block + the Phase 0 contract.
