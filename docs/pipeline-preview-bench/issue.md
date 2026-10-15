# Admin pipeline preview bench: run the full processing pipeline with config overrides

## Problem

We can preview the elevation profile and the route path separately (`elevations/preview`,
`visualizations/preview`), but neither shows what a setting does to **trip stats**, and neither
runs the real pipeline. There is no way to answer "how does the smoothing filter change ascent,
distance and duration?" or "what does a 3 m simplification tolerance cost in shape?".

Related problems found while analysing:

- `GpxDataDirector.AnalyticData(points, config)` ignores its `config` parameter, so a preview path
  (`FromConfig`) and the production path can drift apart.
- Peak matching uses hardcoded values (`PeakProximityTreshold = 100f`, two `minDistance = 50`).
- Route visualisation sends **every** point to the client. The only filters are elevation filters,
  and `RouteVisualizationService.CreateVisualization` ignores the list a filter returns, so a
  filter that removes points would silently do nothing.

## Proposal

An admin-only bench: pick a stored trip file, edit per-stage config, run the pipeline, and compare
every stage against a baseline (production defaults). Nothing is persisted or mutated.

Stages (a dependency graph, not a chain; everything depends on cleanup):

| Stage | Config |
|---|---|
| Cleanup | spike clamp, median window, EMA alpha, rounding |
| Analytics (route + time stats) | derived |
| Elevation profile | downsample factor |
| Peak matching (read-only) | merge distance, elevation delta, proximity radius |
| Route visualisation | ordered filter list incl. new `PinLocalMaxima` and `Simplify(toleranceM)` |

UI: one block per stage with its own config. Editing a block marks it and its dependents stale and
re-runs only those. Server caches stage results keyed by `hash(stage config + upstream keys)`.

## Decisions

- Stored files stay **raw** (blob storage). Downsampling happens only in preview/visualisation.
- Route filter order is user-controlled; the UI warns when `Simplify` precedes `PinLocalMaxima`.
- Local maxima are pinned and never dropped by `Simplify`.
- No backend resampling; the client interpolates (animation, gradients).
- Peak stage stops before first-time flags and `trip.AddReachedPeaks`.

## Acceptance criteria

- [ ] Default `PipelineConfig` reproduces current production output exactly.
- [ ] `GpxDataDirector.AnalyticData(points, config)` honours `config`.
- [ ] Peak thresholds come from `PeakMatchingConfig`, not constants.
- [ ] `Simplify` keeps pinned points; a summit on a flat plateau survives; a straight track
      collapses to its endpoints.
- [ ] `CreateVisualization` uses the list returned by each filter (test: a point-removing filter
      shortens the route).
- [ ] `POST trips/{id}/analytics/pipeline/preview` is admin-only, returns full data per requested
      stage plus baseline, `cacheHit`, duration, point count and payload bytes.
- [ ] A test proves the endpoint persists and mutates nothing.
- [ ] Admin UI renders stats table (baseline / preview / delta), elevation graph, route map, peaks.
- [ ] Changing one stage's config recomputes only that stage and its dependents.
- [ ] Production `RouteVisualizationSetting` supports the new filters.

## Out of scope

Time-synchronized simplification and stop pins, photo-to-route matching, pinned simplification for
the elevation profile, zoom-dependent detail, compact point format, minima pins.

Plan: `docs/pipeline-preview-bench/plan.md`
