# AGENTS.md

## Project

`Blasphemous.LevelFrameworkExtended` — a Blasphemous 1 mod built on the ModdingAPI framework that **extends the Level Framework**.

## Stack & Dependencies

- C# / `net35`, Unity 2017.4.40f1, BepInEx plugin, Harmony patching (inside the framework).
- NuGet: `Blasphemous.ModdingAPI` 3.0.1 · `Blasphemous.Framework.Levels` 0.2.0 · `Blasphemous.GameLibs` 4.0.67 · `Blasphemous.NewbieEltonLibs` 0.1.0.
- The framework's source is vendored read-only at `.references/Blasphemous.Framework.Levels/` — read it before touching framework-dependent code.

## Build & Deploy

- `dotnet build` (Debug default). The csproj `Development` target copies the plugin DLL + `resources/` into the game's `Modding/` folder, exports to `publish/`, and zips the mod (`publish/LevelFrameworkExtended.zip`).
- CI (`.github/workflows/build.yml`, on `main` + tags): `dotnet restore && dotnet build -c Release --no-restore`, then attaches `publish/<TargetName>.zip` to a release and pushes the `.nupkg` to NuGet.
- `ModInfo` (MOD_ID / MOD_NAME / MOD_AUTHOR / MOD_VERSION) is generated at build time from the csproj — do not hand-edit it.

## Project layout

- `Blasphemous.LevelFrameworkExtended/` - Main mod source
- `resources/levels/Level Framework Extended/` — JSON level-edit files, shipped to `Modding/levels/Level Framework Extended/`.
- `publish/` — build artifacts + zip.
- `.references/Blasphemous.Framework.Levels/` — vendored framework source.

## Domain model (from Blasphemous.Framework.Levels)

- **LevelEdit JSON** — one file per scene at `Modding/levels/<ModName>/<Scene>.json`:
  `{ "additions": ObjectData[], "modifications": ObjectData[], "deletions": ObjectData[] }`.
- **ObjectData** fields: `scene` (`decoration`/`layout`/`logic`), `path`, `type` (registered id), `id`, `position`/`rotation`/`scale`, `condition`, `properties[]`.
- **condition** gates an edit: `flag:<id>` · `penitence:<id>` · `gamemode:<id>`; empty string = always apply.
- Scene types map to real scenes: `<level>_DECO` / `<level>_LAYOUT` / `<level>_LOGIC`.
- An `ObjectCreator` = `ILoader` (coroutine `Apply()` → `Result`) + `IModifier` (`Apply(GameObject, ObjectData)`); registered by `ModServiceProvider.RegisterObjectCreator(type, creator)` (see `LevelRegister`).
- Scene paths support `{n}` child-index syntax in `LevelExtensions.FindObject`.

## Conventions

- Follow ModdingAPI conventions: mod class extends `BlasMod`, services registered in `OnRegisterServices`.
- Issues are tracked in GitHub Issues — see `.docs/agents/issue-tracker.md`; domain docs via `.docs/agents/domain.md`.
- Git: commit messages must be in English; get user approval before any commit / PR.
