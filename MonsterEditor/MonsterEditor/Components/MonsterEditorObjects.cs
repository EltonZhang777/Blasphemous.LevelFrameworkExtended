using System.Collections.Generic;
using Blasphemous.Framework.Levels.Loaders;

namespace MonsterEditor.Components;

/// <summary>
/// The monster catalogue. Every entry can be placed through a level edit
/// (<c>monster-en04</c> and so on); the in-game editor reuses the same catalogue and
/// clones a template on demand.
/// </summary>
internal static class MonsterEditorObjects
{
    /// <summary>Prefix shared by every registered object type.</summary>
    internal const string IdPrefix = "monster-";

    /// <summary>
    /// Reek cannot be placed directly: it has no <c>EnemySpawnPoint</c> of its own, and a
    /// <c>ReekSpawnPoint</c> is just an empty marker. The editor places the marker and
    /// registers it with a ReekLeader, which does the summoning.
    /// </summary>
    internal static readonly MonsterEntry ReekPoint =
        new MonsterEntry("REEK", "ReekSpawnPoint (host: ReekLeader)", string.Empty, string.Empty, 0f, MonsterKind.ReekPoint);

    /// <summary>
    /// MeltedLady refuses to live without at least one teleport marker and deactivates
    /// herself otherwise, so the editor can place markers for her.
    /// </summary>
    internal static readonly MonsterEntry MeltedLadyPoint =
        new MonsterEntry("MELTED", "MeltedLadyTeleportPoint", string.Empty, string.Empty, 0f, MonsterKind.MeltedLadyPoint);

    /// <summary>Entries that spawn a bare marker instead of a cloned enemy.</summary>
    internal static readonly MonsterEntry[] Specials =
    {
        ReekPoint,
        MeltedLadyPoint,
    };

    /// <summary>
    /// The 51 normal enemies with a reusable <c>EnemySpawnPoint</c>. The three that need a
    /// summoner host are handled elsewhere, and WaxCrawler has no template at all.
    /// </summary>
    internal static readonly MonsterEntry[] Catalog =
    {
        new MonsterEntry("EN01", "NewFlagellant", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.10f),
        new MonsterEntry("EN02", "Acolyte", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (10)", 1.78f),
        new MonsterEntry("EN03", "Bell Carrier", "D03Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 1.64f),
        new MonsterEntry("EN04", "BellGhost", "D02Z01S05_LOGIC", "CHARACTERS/ENEMIES/SPAWN (8)", 0.10f),
        new MonsterEntry("EN05", "Stoners", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (9)", 1.21f),
        new MonsterEntry("EN07", "GhostKnight", "D03Z03S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.10f),
        new MonsterEntry("EN09", "HeadThrower", "D05Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.34f),
        new MonsterEntry("EN11", "RangedWallEnemy", "D03Z03S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.70f),
        new MonsterEntry("EN12", "Nun", "D02Z03S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.15f),
        new MonsterEntry("EN13", "CrossCrawler", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (19)", 0.10f),
        new MonsterEntry("EN14", "Bishop", "D01Z04S06_LOGIC", "CHARACTERS/ENEMIES/SPAWN (9)", 2.35f),
        new MonsterEntry("EN15", "Menina", "D01Z03S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (9)", 0.15f),
        new MonsterEntry("EN16", "ShooterProcessioner", "D04Z01S04_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.10f),
        new MonsterEntry("EN17", "RangedBoomerang", "D03Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)", 0.17f),
        new MonsterEntry("EN18", "ReekLeader", "D01Z05S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)", 0.14f),
        new MonsterEntry("EN20", "Roller", "D03Z01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)", 0.10f),
        new MonsterEntry("EN21", "Firethrower", "D02Z03S05_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)", 0.20f),
        new MonsterEntry("EN22", "Jumper", "D02Z03S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.10f),
        new MonsterEntry("EN23", "FlyingPortrait", "D05Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)", 0.10f),
        new MonsterEntry("EN24", "ShieldMaiden", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)", 0.10f),
        new MonsterEntry("EN26", "ViciousDasher", "D04Z02S05_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.10f),
        new MonsterEntry("EN27", "MeltedLady", "D02Z03S06_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)", 0.18f),
        new MonsterEntry("EN28", "UndergroundCaveSwimmer", "D02Z01S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)", 0.10f),
        new MonsterEntry("EN29", "CowardTrapper", "D04Z01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.10f),
        new MonsterEntry("EN31", "LanceAngel", "D06Z01S06_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)", 0.94f),
        new MonsterEntry("EN32", "JarThrower", "D02Z01S03_LOGIC", "SPAWN", 0.17f),
        new MonsterEntry("EN33", "Runner", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)", 0.10f),
        new MonsterEntry("EN201", "Axe Roller", "D20Z02S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.10f),
        new MonsterEntry("EN202", "HomingTurret", "D20Z02S04_LOGIC", "CHARACTERS/ENEMIES/SPAWN (5)", 0.10f),
        new MonsterEntry("EN203", "Drowned Corpse", "D20Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (7)", 0.10f),
        new MonsterEntry("EV01", "FlagellantVariant_B", "D02Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 1.83f),
        new MonsterEntry("EV02", "AcolyteVariant_B", "D04Z02S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)", 1.78f),
        new MonsterEntry("EV03", "BellGhostVariant_B", "D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (17)", 0.10f),
        new MonsterEntry("EV08", "MudCrawler", "D01Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (8)", 0.10f),
        new MonsterEntry("EV10", "WheelCarrier", "D01Z01S01_LOGIC", "ENEMIES/SPAWN (15)", 0.11f),
        new MonsterEntry("EV11", "Fool", "D01Z01S01_LOGIC", "ENEMIES/SPAWN", 0.12f),
        new MonsterEntry("EV12", "SewerCrawler", "D01Z05S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN (5)", 0.10f),
        new MonsterEntry("EV13", "Brass Ringer", "D03Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)", 0.10f),
        new MonsterEntry("EV14", "Chime Ringer", "D02Z03S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.10f),
        new MonsterEntry("EV15", "WallEnemySnow", "D02Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.70f),
        new MonsterEntry("EV17", "PatrollingFlyingEnemy", "D03Z01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN_PATROLLING (3)", 0.10f),
        new MonsterEntry("EV18", "InkLady", "D05Z01S07_LOGIC", "CHARACTERS/ENEMIES/SPAWN (4)", 0.18f),
        new MonsterEntry("EV19", "LionHead", "D09BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)", 0.15f),
        new MonsterEntry("EV20", "GoldenCorpse", "D05Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN", 0.12f),
        new MonsterEntry("EV21", "WalkingTomb_King", "D02Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)", 0.16f),
        new MonsterEntry("EV22", "RangedBoomerang_Var", "D05Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (5)", 0.17f),
        new MonsterEntry("EV23", "ShooterProcessioner_AngelVariation", "D06Z01S13_LOGIC", "CHARACTERS/ENEMIES/SPAWN (4)", 0.10f),
        new MonsterEntry("EV24", "JumperVariant", "D04Z02S09_LOGIC", "CHARACTERS/ENEMIES/SPAWN (7)", 0.10f),
        new MonsterEntry("EV26", "Isabel", "D05Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.10f),
        new MonsterEntry("EV27", "ChainedAngel", "D06Z01S07_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)", 0.10f),
        new MonsterEntry("EV29", "PatrollingFlyingEnemyCannon", "D06Z01S04_LOGIC", "CHARACTERS/ENEMIES/SPAWN_PATROLLING", 0.10f),
    };

    /// <summary>Builds the registerable level objects from the catalogue.</summary>
    internal static IEnumerable<LevelObjectEntry> CreateAll()
    {
        for (int i = 0; i < Catalog.Length; i++)
        {
            MonsterEntry entry = Catalog[i];
            yield return new LevelObjectEntry(
                IdPrefix + entry.Id.ToLowerInvariant(),
                new SceneLoader(entry.Scene, entry.Path),
                new MonsterPlacementModifier());
        }
    }
}