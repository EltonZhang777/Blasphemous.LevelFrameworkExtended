using System.Collections;
using System.Collections.Generic;
using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;
using Gameplay.GameControllers.Bosses.BossFight;
using Gameplay.GameControllers.Bosses.ElderBrother;
using Gameplay.GameControllers.Penitent;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace Blasphemous.LevelFrameworkExtended.Components;

/// <summary>
/// A boss encounter as a level object. Placing <c>boss-elder-brother</c> spawns the
/// Warden of the Silent Sorrow with a working fight trigger on any level.
/// </summary>
/// <remarks>
/// The Warden cannot simply be cloned from another level: its AI is a NodeCanvas
/// behaviour tree asset that belongs to its own room, so a clone only carries a
/// reference that dies when that room unloads and the boss then just stands still.
/// The loader therefore keeps the source room loaded for the session and parks it,
/// which keeps the graph alive, and the framework then instantiates a fresh boss
/// from that template on every level load, the way the real game rebuilds it.
/// </remarks>
internal static class ElderBrotherObjects
{
    /// <summary>Registered object type.</summary>
    internal const string ObjectId = "boss-elder-brother";

    /// <summary>Logic scene of the Warden's own boss room.</summary>
    internal const string SourceScene = "D17Z01S11_LOGIC";

    /// <summary>Boss track that the fight manager plays.</summary>
    internal const string BossTrack = "event:/Boss Music/Elder Brother Music";

    /// <summary>Builds the level object so it can join the creator list.</summary>
    internal static IEnumerable<LevelObject> CreateAll()
    {
        return new LevelObject[]
        {
            new LevelObject(ObjectId, new ElderBrotherLoader(), new ElderBrotherModifier()),
        };
    }
}

/// <summary>
/// Extracts the Warden from its own room and turns it into the spawn template.
/// The source room stays resident because its behaviour tree asset must stay loaded.
/// </summary>
internal class ElderBrotherLoader : ILoader
{
    /// <summary>
    /// Gets the intro jump point that survives from the source room. The behaviour
    /// keeps a reference to this sibling object, which is not part of the boss itself.
    /// </summary>
    internal static Transform JumpPoint { get; private set; }

    /// <inheritdoc/>
    public GameObject Result { get; private set; }

    /// <inheritdoc/>
    public IEnumerator Apply()
    {
        Scene scene = SceneManager.GetSceneByName(ElderBrotherObjects.SourceScene);
        if (!scene.IsValid())
        {
            AsyncOperation load = SceneManager.LoadSceneAsync(ElderBrotherObjects.SourceScene, LoadSceneMode.Additive);
            while (!load.isDone)
            {
                yield return null;
            }

            scene = SceneManager.GetSceneByName(ElderBrotherObjects.SourceScene);
        }

        GameObject[] roots = scene.GetRootGameObjects();
        GameObject boss = FindBoss(roots);
        if (boss == null)
        {
            ModLog.Error($"No Warden found in {ElderBrotherObjects.SourceScene}");
            yield break;
        }

        JumpPoint = FindJumpPoint(boss.transform);
        ParkRoom(roots);
        ParkSiblings(boss.transform);

        Result = boss;
        ModLog.Info("Warden template is ready");
    }

    private static GameObject FindBoss(GameObject[] roots)
    {
        for (int i = 0; i < roots.Length; i++)
        {
            ElderBrother[] found = roots[i].GetComponentsInChildren<ElderBrother>(true);
            if (found.Length > 0)
            {
                return found[0].gameObject;
            }
        }

        return null;
    }

    private static Transform FindJumpPoint(Transform boss)
    {
        Transform enemies = boss.parent;
        if (enemies == null)
        {
            return null;
        }

        for (int i = 0; i < enemies.childCount; i++)
        {
            Transform child = enemies.GetChild(i);
            if (child.name == "IntroJumpPoint")
            {
                return child;
            }
        }

        return null;
    }

    /// <summary>
    /// Keeps only the root that holds the boss. Leaving the source room's camera,
    /// logic or effects roots alive makes it compete with the current level, which
    /// floods the log and breaks level transitions.
    /// </summary>
    private static void ParkRoom(GameObject[] roots)
    {
        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].activeSelf && roots[i].name != "CHARACTERS")
            {
                roots[i].SetActive(false);
            }
        }
    }

    private static void ParkSiblings(Transform boss)
    {
        Transform enemies = boss.parent;
        if (enemies == null)
        {
            return;
        }

        for (int i = 0; i < enemies.childCount; i++)
        {
            Transform child = enemies.GetChild(i);
            if (child == boss || child.name == "IntroJumpPoint")
            {
                continue;
            }

            if (child.gameObject.activeSelf)
            {
                child.gameObject.SetActive(false);
            }
        }
    }
}

/// <summary>
/// Places a spawned Warden and gives it a fight trigger. Each level load runs this
/// again, so the boss always starts fresh with full health.
/// </summary>
internal class ElderBrotherModifier : IModifier
{
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = string.IsNullOrEmpty(data.id) ? "ElderBrother" : data.id;

        ElderBrother boss = obj.GetComponent<ElderBrother>();
        if (boss == null)
        {
            obj.SetActive(false);
            return;
        }

        if (boss.Behaviour != null && ElderBrotherLoader.JumpPoint != null)
        {
            ElderBrotherLoader.JumpPoint.position = obj.transform.position;
            boss.Behaviour.introJumpPoint = ElderBrotherLoader.JumpPoint;
        }

        CreateTrigger(obj.transform.position, boss);
    }

    /// <summary>
    /// Builds the fight node by hand instead of copying the original room's state
    /// machine, whose actions would also grant the original achievement and write
    /// that room's flags.
    /// </summary>
    private static void CreateTrigger(Vector3 position, ElderBrother boss)
    {
        GameObject trigger = new GameObject(boss.gameObject.name + "Fight");
        trigger.transform.position = position;

        // Audio and Metrics must exist first: the manager's Awake resolves them.
        trigger.AddComponent<BossFightAudio>().BossTrackId = ElderBrotherObjects.BossTrack;
        trigger.AddComponent<BossFightMetrics>();

        BossFightManager manager = trigger.AddComponent<BossFightManager>();
        manager.Boss = boss;
        manager.ShowLifeBar = true;
        manager.BossCountsTowardsAC43 = false;
        manager.onStartBossFight = new UnityEvent();
        manager.onFinishBossFight = new UnityEvent();

        BoxCollider2D collider = trigger.AddComponent<BoxCollider2D>();
        collider.isTrigger = true;
        collider.size = new Vector2(4f, 10f);

        trigger.AddComponent<BossFightTrigger>().Boss = boss;
    }
}

/// <summary>
/// Starts the encounter when the penitent walks in: the intro jump is what also
/// starts the boss behaviour tree, then the fight manager takes over.
/// </summary>
internal class BossFightTrigger : MonoBehaviour
{
    /// <summary>Gets or sets the boss this trigger starts.</summary>
    internal ElderBrother Boss { get; set; }

    private BossFightManager _manager;

    private bool _started;

    private void Awake()
    {
        _manager = GetComponent<BossFightManager>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryStart(other);
    }

    /// <summary>Also fires when the penitent spawns already inside the trigger.</summary>
    private void OnTriggerStay2D(Collider2D other)
    {
        TryStart(other);
    }

    private void TryStart(Collider2D other)
    {
        if (_started || _manager == null || Boss == null)
        {
            return;
        }

        if (other.GetComponentInParent<Penitent>() == null)
        {
            return;
        }

        _started = true;
        StartCoroutine(StartFight());
    }

    private IEnumerator StartFight()
    {
        if (Boss.Behaviour != null)
        {
            Boss.Behaviour.IntroJump();
        }

        yield return new WaitForSeconds(2f);
        _manager.StartBossFight();
    }
}
