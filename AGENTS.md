## local instructions

See [AGENTS.local.md](AGENTS.local.md)

## Project

`Blasphemous.LevelFrameworkExtended` — a Blasphemous 1 mod built on the ModdingAPI framework that **extends** the _Level Framework_.

## Stack & Dependencies

- Source of `Blasphemous.Framework.Levels` is vendored read-only at `.references/Blasphemous.Framework.Levels/` — read it before touching framework-dependent code.
- **LevelEdit JSON** — one file per scene at `Modding/levels/<ModName>/<Scene>.json`:
  `{ "additions": ObjectData[], "modifications": ObjectData[], "deletions": ObjectData[] }`.
- **ObjectData** fields: `scene` (`decoration`/`layout`/`logic`), `path`, `type` (registered id), `id`, `position`/`rotation`/`scale`, `condition`, `properties[]`.
- **condition** gates an edit: `flag:<id>` · `penitence:<id>` · `gamemode:<id>`; empty string = always apply.
- Scene types map to real scenes: `<level>_DECO` / `<level>_LAYOUT` / `<level>_LOGIC`.
- An `ObjectCreator` = `ILoader` (coroutine `Apply()` → `Result`) + `IModifier` (`Apply(GameObject, ObjectData)`); registered by `ModServiceProvider.RegisterObjectCreator(type, creator)` (see `LevelRegister`).
- Scene paths support `{n}` child-index syntax in `LevelExtensions.FindObject`, `n` being 0-based.

## Build & Deploy

- `ModInfo` (MOD_ID / MOD_NAME / MOD_AUTHOR / MOD_VERSION) is generated at build time from the csproj — do not hand-edit it.


## Agent skills

### Issue tracker

Issues are tracked in GitHub Issues. See `docs/agents/issue-tracker.md`.

### Triage labels

Use the default five canonical triage labels. See `docs/agents/triage-labels.md`.

### Domain docs

Single-context layout with root `GLOSSARY.md` and `docs/adr/`. See `docs/agents/domain.md`.
