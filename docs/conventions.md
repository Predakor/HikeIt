# Conventions

Rules that the analyzers don't enforce. `.editorconfig` and the analyzers (`AnalysisMode=recommended`, SonarAnalyzer) cover formatting and most naming.

## Error handling (backend)

Expected failures are values, not exceptions. Everything that can fail for a reason a user or caller can cause returns `Result<T>` (`Core.Results`).

- Return `Errors.*` (`NotFound`, `BadRequest`, `NotUnique`, `RuleViolation`, `ParsingError`, `File`, `EmptyCollection`, `NotAuthorized`, ...) or the value; both convert implicitly to `Result<T>`. Add a new `ResultError` case in `Core/Results/ResultError.cs` instead of reusing `Unknown`.
- Compose with `Map`, `Bind`, `Tap`, `Match`, `EnsureNotEmpty` (`ResultExtentions`) rather than `if (!result.IsSuccess)` ladders.
- The API boundary converts to HTTP once: `ToApiResult` / `ToApiResultAsync` for minimal APIs, `ToActionResult` for controllers. Don't build `BadRequest`/`NotFound` responses by hand in an action.
- Business rules go through `IRule` / `Errors.RuleViolation(rule)`.

### Validation

Validate with rules (`IRule` / `IRuleAsync`, `AbstractValidator<T>`, `Core/Validators/Rules`) whenever a rule fits, instead of inline `if` checks that build errors by hand. A failed rule becomes `Errors.RuleViolation(rule)`.

Validators are optional decorators over the service they guard, so the service stays free of input checks. To add one, write a class that implements the service interface and the `IValidatable<TService>` marker, wrapping the inner service:

```csharp
internal sealed class TripServiceValidator(ITripService inner) : ITripService, IValidatable<ITripService>
```

`DependencyInjection.DecorateMarked` finds every class carrying the marker and applies it with Scrutor's `Decorate`, so there is no registration code per validator.

`ICacheable<TService>` works the same way for caching decorators. Caches are applied first and validators last, so validation runs outermost, before a cache lookup. `IValidatable` and `ICacheable` both extend `IDecorator<TService>`. Older decorators (`CachedAppSettingsServiceDecorator`, `CachedFileStorageDecorator`, the logged event dispatcher) are still registered by hand; move them to the markers when they're touched.

### Service registration

A service opts in to DI through a marker on its interface (`Core.Abstractions`, imported everywhere): `IService` for scoped, `ISingletonService` for singleton, for example `public interface ITripService : IService`. `InjectServices` (Api) registers every implementation of a marked interface under that interface. Decorators are skipped by the scan (they carry `IDecorator<>`) and applied by `Decorate` instead. Query interfaces extend `IQueryService`, which already extends `IService`. Class names no longer matter. Repositories (`IRepository<,>`), pipeline stages and domain event handlers keep their own scans.

### Command pattern

`ICommand<T>` / `IAsyncCommand<T>` (`Domain/Common/Abstractions/ICommand.cs`, used by `DecodeGainsDataCommand`, `CreatePeakAnalyticsCommand`, `CreateReachedPeaksCommand`, `CreateElevationProfileCommand`) is the wrong abstraction here and shouldn't be used for new code. Write a service method or a pipeline stage instead. Existing commands stay until they're touched; when you edit one, prefer moving its logic to the service rather than extending the command.

Throw only when the state is a programmer error that should never happen: a stage output that is missing after an earlier stage guaranteed it, an unregistered setting type, an invalid filter config built from constants, an impossible enum value. A thrown exception means "bug", so don't catch it to turn it into a `Result`.

Decision test: could a user, a bad file, or a missing row cause this? Then it is a `Result`. If only a code bug could, throw.

Existing code has some `throw`s on inputs that may be reachable from a user upload (for example the GPX helpers). Treat those as debt to convert, not a pattern to copy.

## Linking between resources

Endpoints return resource URLs (`Uri?`) instead of ids or embedded payloads for heavy or optional sub-resources. See `TripAnalyticsDto.Linked` and `ToResoutceUrl`. Null means the resource doesn't exist. The frontend resolves them with `useResourceLink`.

## Backend style

- Vertical slices: a feature gets its own folder in each layer, not a shared bucket.
- All JSON through `Core.Serialization`. Missing helper? Add it there; no local converters or naming policies.
- Explicit access modifiers. Primary constructor parameters stay camelCase; `_` only on declared private fields (CA1707).
- Type parameters are `T` + name: `TFilter`, `TStage`, `TState`. Never bare `F`/`S`/`K`.
- Dispatching on a key to behavior: use a handler map. A plain `switch` is fine for simple cases.
- A non-trivial `if` condition goes into a clearly named local `const`/variable. `if` always has braces.
- Comments: one short line, only for a non-obvious why. No class-level doc paragraphs.

## Frontend style

- Server state is TanStack Query. Wrap request results in `FetchWrapper` instead of writing loading/error branches.
- Conditional rendering with `<Show>` or `&&`.
- No `for-of`, no `delete`. Use array methods and object spreads.
- Reuse the helpers in `src/Utils` and `src/hooks` before adding new ones.
- Pages are registered in `src/data/routes`, not as inline `<Route>`s.

## Git hooks and line endings

Run `sh .githooks/setup.sh` once per clone. It sets `core.hooksPath=.githooks`.

- **pre-commit**: formats the staged files (`dotnet format` for `.cs`, prettier + eslint `--fix` for `Frontend/src`), re-stages them, and converts any CRLF in staged text files to LF (working tree and index). It refuses to run when a staged `.cs` or frontend file also has unstaged changes, so formatting can't sweep unrelated edits into the commit.
- **pre-push**: when C# changed, `dotnet build` (analyzers run) and `dotnet test`; when `Frontend/` changed, `tsc --b` and `pnpm lint`. It also fails if the pushed changes add CRLF lines, which catches commits made with `--no-verify`.

Line endings: LF everywhere, in the repo and the working tree, enforced by `* text=auto eol=lf` in `.gitattributes` (plus `end_of_line = lf` in `.editorconfig`), so no local `autocrlf` setting is needed. Only `.bat`/`.cmd` are CRLF. Editors and AI tools sometimes still write CRLF, which is why `pre-commit` normalizes staged files and `pre-push` checks the diff.

The solution has existing format and analyzer debt (brace style, final newlines, many Sonar warnings), so the hooks only touch staged files and don't gate on warnings. A touched legacy file gets reformatted whole. Clean the baseline in its own commit with `dotnet format HikeIt.sln` before relying on the gate.

## Tests

Test business logic: filter outputs, analytics, pipeline results (`Domain.Tests`). Don't test casing, DI wiring, serialization naming or config plumbing. Pipeline filters get a test with a small hand-built input and an exact expected output.
