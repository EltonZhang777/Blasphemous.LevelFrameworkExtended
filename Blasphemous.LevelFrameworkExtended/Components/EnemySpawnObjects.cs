using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using System.Collections;
using System.Collections.Generic;
using Framework.Managers;
using Gameplay.GameControllers.Enemies.Framework.IA;
using Gameplay.GameControllers.Entities;
using Tools.Level.Layout;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.Components;

/// <summary>
/// Level objects that spawn one enemy per JSON entry, for example:
/// <code>{ "type": "enemy-EV27", "id": "angel-1", "position": { "x": 370, "y": 133 } }</code>
/// The spawn point is cloned from the enemy's vanilla scene, so the enemy prefab
/// reference is already serialized on it. <see cref="EnemySpawnActivator"/> places
/// and enables the enemy one frame later, after the level's colliders exist.
/// </summary>
internal static class EnemySpawnObjects
{
    /// <summary>One level object per enemy that has a reusable EnemySpawnPoint template.</summary>
    internal static IEnumerable<LevelObject> CreateAll()
    {
        return new LevelObject[]
        {
        new LevelObject("enemy-EN01", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("NewFlagellant", 0.10f)),
        new LevelObject("enemy-EN02", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (10)"), new EnemySpawnModifier("Acolyte", 1.78f)),
        new LevelObject("enemy-EN03", new SceneLoader("D03Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("Bell Carrier", 1.64f)),
        new LevelObject("enemy-EN04", new SceneLoader("D02Z01S05_LOGIC", "CHARACTERS/ENEMIES/SPAWN (8)"), new EnemySpawnModifier("BellGhost", 0.10f)),
        new LevelObject("enemy-EN05", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (9)"), new EnemySpawnModifier("Stoners", 1.21f)),
        new LevelObject("enemy-EN07", new SceneLoader("D03Z03S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("GhostKnight", 0.10f)),
        new LevelObject("enemy-EN08", new SceneLoader("D01Z05S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)"), new EnemySpawnModifier("ReekLeader", 0.14f)), // target summoned by ReekLeader
        new LevelObject("enemy-EN09", new SceneLoader("D05Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("HeadThrower", 0.34f)),
        new LevelObject("enemy-EN10", new SceneLoader("D05Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("HeadThrower", 0.34f)), // target summoned by HeadThrower
        new LevelObject("enemy-EN11", new SceneLoader("D03Z03S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("RangedWallEnemy", 0.70f)),
        new LevelObject("enemy-EN12", new SceneLoader("D02Z03S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("Nun", 0.15f)),
        new LevelObject("enemy-EN13", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (19)"), new EnemySpawnModifier("CrossCrawler", 0.10f)),
        new LevelObject("enemy-EN14", new SceneLoader("D01Z04S06_LOGIC", "CHARACTERS/ENEMIES/SPAWN (9)"), new EnemySpawnModifier("Bishop", 2.35f)),
        new LevelObject("enemy-EN15", new SceneLoader("D01Z03S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (9)"), new EnemySpawnModifier("Menina", 0.15f)),
        new LevelObject("enemy-EN16", new SceneLoader("D04Z01S04_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("ShooterProcessioner", 0.10f)),
        new LevelObject("enemy-EN17", new SceneLoader("D03Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)"), new EnemySpawnModifier("RangedBoomerang", 0.17f)),
        new LevelObject("enemy-EN18", new SceneLoader("D01Z05S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)"), new EnemySpawnModifier("ReekLeader", 0.14f)),
        new LevelObject("enemy-EN20", new SceneLoader("D03Z01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)"), new EnemySpawnModifier("Roller", 0.10f)),
        new LevelObject("enemy-EN201", new SceneLoader("D20Z02S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("Axe Roller", 0.10f)),
        new LevelObject("enemy-EN202", new SceneLoader("D20Z02S04_LOGIC", "CHARACTERS/ENEMIES/SPAWN (5)"), new EnemySpawnModifier("HomingTurret", 0.10f)),
        new LevelObject("enemy-EN203", new SceneLoader("D20Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (7)"), new EnemySpawnModifier("Drowned Corpse", 0.10f)),
        new LevelObject("enemy-EN21", new SceneLoader("D02Z03S05_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)"), new EnemySpawnModifier("Firethrower", 0.20f)),
        new LevelObject("enemy-EN22", new SceneLoader("D02Z03S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("Jumper", 0.10f)),
        new LevelObject("enemy-EN23", new SceneLoader("D05Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)"), new EnemySpawnModifier("FlyingPortrait", 0.10f)),
        new LevelObject("enemy-EN24", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)"), new EnemySpawnModifier("ShieldMaiden", 0.10f)),
        new LevelObject("enemy-EN26", new SceneLoader("D04Z02S05_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("ViciousDasher", 0.10f)),
        new LevelObject("enemy-EN27", new SceneLoader("D02Z03S06_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)"), new EnemySpawnModifier("MeltedLady", 0.18f)),
        new LevelObject("enemy-EN28", new SceneLoader("D02Z01S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)"), new EnemySpawnModifier("UndergroundCaveSwimmer", 0.10f)),
        new LevelObject("enemy-EN29", new SceneLoader("D04Z01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("CowardTrapper", 0.10f)),
        new LevelObject("enemy-EN31", new SceneLoader("D06Z01S06_LOGIC", "CHARACTERS/ENEMIES/SPAWN (6)"), new EnemySpawnModifier("LanceAngel", 0.94f)),
        new LevelObject("enemy-EN32", new SceneLoader("D02Z01S03_LOGIC", "SPAWN"), new EnemySpawnModifier("JarThrower", 0.17f)),
        new LevelObject("enemy-EN33", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (3)"), new EnemySpawnModifier("Runner", 0.10f)),
        new LevelObject("enemy-EV01", new SceneLoader("D02Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("FlagellantVariant_B", 1.83f)),
        new LevelObject("enemy-EV02", new SceneLoader("D04Z02S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)"), new EnemySpawnModifier("AcolyteVariant_B", 1.78f)),
        new LevelObject("enemy-EV03", new SceneLoader("D01BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (17)"), new EnemySpawnModifier("BellGhostVariant_B", 0.10f)),
        new LevelObject("enemy-EV05", new SceneLoader("D05Z01S06", "CHARACTERS/ENEMIES/SPAWN (7)"), new EnemySpawnModifier("HeadThrower", 0.34f)), // target summoned by HeadThrower
        new LevelObject("enemy-EV08", new SceneLoader("D01Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (8)"), new EnemySpawnModifier("MudCrawler", 0.10f)),
        new LevelObject("enemy-EV10", new SceneLoader("D01Z01S01_LOGIC", "ENEMIES/SPAWN (15)"), new EnemySpawnModifier("WheelCarrier", 0.11f)),
        new LevelObject("enemy-EV11", new SceneLoader("D01Z01S01_LOGIC", "ENEMIES/SPAWN"), new EnemySpawnModifier("Fool", 0.12f)),
        new LevelObject("enemy-EV12", new SceneLoader("D01Z05S03_LOGIC", "CHARACTERS/ENEMIES/SPAWN (5)"), new EnemySpawnModifier("SewerCrawler", 0.10f)),
        new LevelObject("enemy-EV13", new SceneLoader("D03Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)"), new EnemySpawnModifier("Brass Ringer", 0.10f)),
        new LevelObject("enemy-EV14", new SceneLoader("D02Z03S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("Chime Ringer", 0.10f)),
        new LevelObject("enemy-EV15", new SceneLoader("D02Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("WallEnemySnow", 0.70f)),
        new LevelObject("enemy-EV17", new SceneLoader("D03Z01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN_PATROLLING (3)"), new EnemySpawnModifier("PatrollingFlyingEnemy", 0.10f)),
        new LevelObject("enemy-EV18", new SceneLoader("D05Z01S07_LOGIC", "CHARACTERS/ENEMIES/SPAWN (4)"), new EnemySpawnModifier("InkLady", 0.18f)),
        new LevelObject("enemy-EV19", new SceneLoader("D09BZ01S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)"), new EnemySpawnModifier("LionHead", 0.15f)),
        new LevelObject("enemy-EV20", new SceneLoader("D05Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN"), new EnemySpawnModifier("GoldenCorpse", 0.12f)),
        new LevelObject("enemy-EV21", new SceneLoader("D02Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (1)"), new EnemySpawnModifier("WalkingTomb_King", 0.16f)),
        new LevelObject("enemy-EV22", new SceneLoader("D05Z01S02_LOGIC", "CHARACTERS/ENEMIES/SPAWN (5)"), new EnemySpawnModifier("RangedBoomerang_Var", 0.17f)),
        new LevelObject("enemy-EV23", new SceneLoader("D06Z01S13_LOGIC", "CHARACTERS/ENEMIES/SPAWN (4)"), new EnemySpawnModifier("ShooterProcessioner_AngelVariation", 0.10f)),
        new LevelObject("enemy-EV24", new SceneLoader("D04Z02S09_LOGIC", "CHARACTERS/ENEMIES/SPAWN (7)"), new EnemySpawnModifier("JumperVariant", 0.10f)),
        new LevelObject("enemy-EV26", new SceneLoader("D05Z02S01_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("Isabel", 0.10f)),
        new LevelObject("enemy-EV27", new SceneLoader("D06Z01S07_LOGIC", "CHARACTERS/ENEMIES/SPAWN (2)"), new EnemySpawnModifier("ChainedAngel", 0.10f)),
        new LevelObject("enemy-EV29", new SceneLoader("D06Z01S04_LOGIC", "CHARACTERS/ENEMIES/SPAWN_PATROLLING"), new EnemySpawnModifier("PatrollingFlyingEnemyCannon", 0.10f)),
        };
    }
}

/// <summary>Configures a cloned spawn point: enemy name, lift and deferred activation.</summary>
internal sealed class EnemySpawnModifier : IModifier
{
    private readonly string _entityName;
    private readonly float _lift;

    /// <param name="entityName">Name used by the game for the spawned enemy.</param>
    /// <param name="lift">Vertical offset applied when the enemy is placed, computed from its collider/probes.</param>
    internal EnemySpawnModifier(string entityName, float lift)
    {
        _entityName = entityName;
        _lift = lift;
    }

    /// <inheritdoc/>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = string.IsNullOrEmpty(data.id) ? "EnemySpawn" : data.id;

        EnemySpawnPoint point = obj.GetComponent<EnemySpawnPoint>();
        if (point == null)
            return;

        // Keep the enemy inactive; EnemySpawnActivator activates it one frame later.
        point.SpawnEnabledEnemy = false;
        point.SpawnOnArena = false;
        point.EnableBehaviourOnLoad = true;
        point.EnablePersistence = false;
        point.EnemySpawnDisabled = false;
        point.EntityName = _entityName;

        EnemySpawnMarker marker = obj.AddComponent<EnemySpawnMarker>();
        marker.EnemyId = data.id;
        marker.Lift = _lift;

        foreach (SpriteRenderer renderer in obj.GetComponentsInChildren<SpriteRenderer>(true))
            renderer.enabled = false;
    }
}

/// <summary>Marks a cloned spawn point and stores where its enemy should be placed.</summary>
internal sealed class EnemySpawnMarker : MonoBehaviour
{
    /// <summary>Gets or sets the level edit id, used only for diagnostics.</summary>
    internal string EnemyId { get; set; }

    /// <summary>Gets or sets the vertical offset applied when placing the enemy.</summary>
    internal float Lift { get; set; } = 0.1f;
}

/// <summary>
/// Places and enables every marked enemy one frame after a level loads, once the
/// physics scene exists. Ground enemies spawned at floor height would otherwise
/// write float.MinValue into Y when their ground probes start inside the floor.
/// </summary>
internal sealed class EnemySpawnActivator : MonoBehaviour
{
    /// <summary>Creates the activator for the current level if it does not exist yet.</summary>
    internal static void Ensure()
    {
        if (Object.FindObjectOfType<EnemySpawnActivator>() == null)
            new GameObject("EnemySpawnActivator").AddComponent<EnemySpawnActivator>();
    }

    private IEnumerator Start()
    {
        yield return null;

        EnemySpawnMarker[] markers = Object.FindObjectsOfType<EnemySpawnMarker>();
        for (int i = 0; i < markers.Length; i++)
        {
            EnemySpawnPoint point = markers[i].GetComponent<EnemySpawnPoint>();
            if (point == null || point.SpawnedEnemy == null)
                continue;

            float lift = markers[i].Lift > 0f ? markers[i].Lift : 0.1f;
            point.SpawnedEnemy.transform.position = point.transform.position + new Vector3(0f, lift, 0f);
            point.SpawnedEnemy.gameObject.SetActive(true);
            WakeLateSpawn(point.SpawnedEnemy);
        }

        Destroy(gameObject);
    }

    /// <summary>
    /// Many enemies disable their behaviour in OnAwake and only re-enable it from
    /// <c>SpawnManager.OnPlayerSpawn</c>, which has already fired by the time a level edit
    /// spawns anything. ChainedAngel, BellGhost, LanceAngel, Legionary, WalkingTomb,
    /// JarThrower, CowardTrapper, Roller, Runner, ShieldMaiden, FloatingLady,
    /// ShooterProcessioner, RangedBoomerang, BellCarrier, PietyMonster and
    /// BejeweledSaintHead all follow that pattern and would otherwise stand still.
    /// Replay the two steps those handlers perform, on this enemy only.
    /// </summary>
    private static void WakeLateSpawn(Enemy enemy)
    {
        if (enemy.Target == null && Core.Logic.Penitent != null)
            enemy.Target = Core.Logic.Penitent.gameObject;

        EnemyBehaviour[] behaviours = enemy.GetComponentsInChildren<EnemyBehaviour>(true);
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (!behaviours[i].enabled)
                behaviours[i].enabled = true;
        }
    }
}
