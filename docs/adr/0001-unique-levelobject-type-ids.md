# ADR 0001: Give every LevelObject a unique type ID

- Status: Accepted
- Date: 2026-11-06

## Context

The Level Framework registers object creators by LevelObject ID, which is the ObjectData.type used by level edits. Duplicate IDs cause later registrations to be ignored. This makes distinct constructor configurations unreachable by their own test entries. Three declarations also repeat an identical ID, scene loader, and modifier configuration.

## Decision

- Remove later declarations whose complete LevelObject configuration is identical, ignoring only formatting and comments.
- Keep the existing ID for the first distinct configuration in each duplicate-ID group. Give each later distinct configuration a fixed numeric suffix (-1, -2, and so on).
- Add one test object for every remaining declaration, grouped by source region, with no more than 45 additions per case. Keep suffixes as stable source literals; do not recalculate them when declarations move.

## Consequences

Existing references to unsuffixed IDs continue to resolve to the same first configuration. Previously shadowed configurations become addressable and are covered individually by the new fixtures. Type IDs are public level-edit identifiers, so changing a suffix later requires reviewing saved level edits.

The framework registry can be shared with other mods. This repository and the vendored framework were checked for collisions with the new IDs; external mods were not available for inspection, so collisions outside this repository remain possible.
