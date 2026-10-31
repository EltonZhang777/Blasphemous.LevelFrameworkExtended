using System.Collections;
using System.Collections.Generic;
using Blasphemous.Framework.Levels;
using Blasphemous.ModdingAPI;
using Framework.Managers;
using Gameplay.GameControllers.Effects.Entity.BlobShadow;
using Gameplay.GameControllers.Enemies.Framework.IA;
using Gameplay.GameControllers.Enemies.MeltedLady;
using Gameplay.GameControllers.Enemies.MeltedLady.Attack;
using Gameplay.GameControllers.Enemies.ReekLeader;
using Gameplay.GameControllers.Enemies.ReekLeader.Attack;
using Gameplay.GameControllers.Entities;
using HutongGames.PlayMaker;
using Tools.Level.Layout;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MonsterEditor.Components;

/// <summary>
/// The in-game editor. Press <c>F7</c> to open the panel, pick a monster, type coordinates
/// and spawn. Templates are built lazily: the first time a monster is used its spawn point
/// is cloned out of the original room, which is never reloaded or unloaded if that room
/// happens to be loaded already.
/// </summary>
internal class MonsterEditorTool : MonoBehaviour
{
    private const string InputBlocker = "MONSTER_EDITOR";

    /// <summary>The real level's initializer, captured on every level load.</summary>
    private static LevelInitializer _levelConfig;

    /// <summary>False while a level is loading; spawning is refused until it flips back.</summary>
    private static bool _levelReady;

    private readonly Dictionary<int, GameObject> _templates = new Dictionary<int, GameObject>();

    private readonly List<GameObject> _spawnRoots = new List<GameObject>();

    private readonly List<ReekSpawnPoint> _reekPoints = new List<ReekSpawnPoint>();

    private bool _open;

    private bool _busy;

    private int _selected;

    private Vector2 _scroll = Vector2.zero;

    private string _xText = "0";

    private string _yText = "0";

    private string _status = "F7 opens the panel";

    private GUIStyle _text;

    private GUIStyle _head;

    /// <summary>Gets the entry the panel is currently pointing at.</summary>
    private MonsterEntry Current => EntryAt(_selected);

    private void Start()
    {
        FillFromPenitent();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7))
        {
            Toggle();
        }

        if (Input.GetKeyDown(KeyCode.F9))
        {
            ClearSpawned();
        }
    }

    private void OnGUI()
    {
        if (_text == null)
        {
            _text = new GUIStyle(GUI.skin.label) { fontSize = 14 };
            _head = new GUIStyle(GUI.skin.label) { fontSize = 17, fontStyle = FontStyle.Bold };
        }

        if (!_open)
        {
            GUI.Label(new Rect(20f, 20f, 420f, 26f), "F7  monster editor", _text);
            return;
        }

        GUILayout.BeginArea(new Rect(32f, 32f, 470f, 610f), GUI.skin.box);
        GUILayout.Label("MONSTER EDITOR", _head);
        GUILayout.Label($"{Current.Id}  {Current.Name}", _text);

        _scroll = GUILayout.BeginScrollView(_scroll, GUILayout.Height(300f));
        for (int i = 0; i < MonsterEditorObjects.Catalog.Length; i++)
        {
            MonsterEntry entry = MonsterEditorObjects.Catalog[i];
            if (GUILayout.Button($"{entry.Id}  {entry.Name}"))
            {
                _selected = i;
                FillFromPenitent();
            }
        }

        GUILayout.EndScrollView();

        for (int i = 0; i < MonsterEditorObjects.Specials.Length; i++)
        {
            MonsterEntry special = MonsterEditorObjects.Specials[i];
            if (GUILayout.Button($"{special.Id}   {special.Name}"))
            {
                _selected = MonsterEditorObjects.Catalog.Length + i;
                FillFromPenitent();
            }
        }

        GUILayout.BeginHorizontal();
        GUILayout.Label("X", GUILayout.Width(18f));
        _xText = GUILayout.TextField(_xText, GUILayout.Width(96f));
        GUILayout.Label("Y", GUILayout.Width(18f));
        _yText = GUILayout.TextField(_yText, GUILayout.Width(96f));
        if (GUILayout.Button("from player"))
        {
            FillFromPenitent();
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Spawn"))
        {
            RequestSpawn();
        }

        if (GUILayout.Button("Clear"))
        {
            ClearSpawned();
        }

        if (GUILayout.Button("Close"))
        {
            Toggle();
        }

        GUILayout.EndHorizontal();

        GUILayout.Label(_status, _text);
        GUILayout.Label("Markers first for Reek and MeltedLady.", _text);
        GUILayout.EndArea();
    }

    /// <summary>Remembers the current level's initializer so it can be restored.</summary>
    internal static void CaptureLevelConfig()
    {
        LevelInitializer found = Object.FindObjectOfType<LevelInitializer>();
        if (found != null)
        {
            _levelConfig = found;
        }
    }

    /// <summary>Opens or closes the door while a level transition is in progress.</summary>
    internal static void SetLevelReady(bool ready)
    {
        _levelReady = ready;
    }

    /// <summary>Drops the captured initializer, which dies with the level that owned it.</summary>
    internal static void InvalidateLevelConfig()
    {
        _levelConfig = null;
    }

    /// <summary>
    /// Building a template loads a foreign room, whose LevelInitializer steals
    /// <c>Core.Logic.CurrentLevelConfig</c>; once that room unloads the config is a destroyed
    /// object and every later spawn throws on it. Put the real one back before every spawn.
    /// </summary>
    private static void EnsureLevelConfig()
    {
        if (_levelConfig != null)
        {
            Core.Logic.SetCurrentLevelConfig(_levelConfig);
        }
    }

    /// <summary>The catalogue comes first; its marker entries follow it.</summary>
    private static MonsterEntry EntryAt(int index)
    {
        return index < MonsterEditorObjects.Catalog.Length
            ? MonsterEditorObjects.Catalog[index]
            : MonsterEditorObjects.Specials[index - MonsterEditorObjects.Catalog.Length];
    }

    private void Toggle()
    {
        _open = !_open;

        if (_open)
        {
            FillFromPenitent();
            Core.Input.SetBlocker(InputBlocker, true);
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Core.Input.SetBlocker(InputBlocker, false);
        }

        ModLog.Info("MonsterEditor: panel " + (_open ? "opened" : "closed"));
    }

    private void FillFromPenitent()
    {
        if (Core.Logic.Penitent == null)
        {
            return;
        }

        Vector3 position = Core.Logic.Penitent.transform.position;
        _xText = (position.x + 2f).ToString("0.##");
        _yText = (position.y + Current.Lift).ToString("0.##");
    }

    private void RequestSpawn()
    {
        if (!_levelReady)
        {
            _status = "level is still loading - try again";
            return;
        }

        if (_busy)
        {
            return;
        }

        float x;
        float y;
        if (!float.TryParse(_xText, out x) || !float.TryParse(_yText, out y))
        {
            _status = "bad coordinates";
            return;
        }

        if (Current.Kind != MonsterKind.Enemy)
        {
            SpawnAt(_selected, x, y);
            return;
        }

        if (_templates.ContainsKey(_selected))
        {
            SpawnAt(_selected, x, y);
            return;
        }

        StartCoroutine(BuildThenSpawn(_selected, x, y));
    }

    private IEnumerator BuildThenSpawn(int index, float x, float y)
    {
        try
        {
            yield return BuildTemplate(index);
            if (_templates.ContainsKey(index))
            {
                SpawnAt(index, x, y);
            }
        }
        finally
        {
            _busy = false;
        }
    }

    /// <summary>
    /// Clones one spawn point out of its source scene. A scene that is already loaded is used
    /// in place and left alone, because unloading it would tear down the running level.
    /// </summary>
    private IEnumerator BuildTemplate(int index)
    {
        if (!_levelReady)
        {
            _status = "level is still loading - template not built";
            yield break;
        }

        _busy = true;
        MonsterEntry entry = EntryAt(index);
        _status = "building template: " + entry.Name;

        LevelInitializer savedConfig = Core.Logic.CurrentLevelConfig;

        Scene scene = SceneManager.GetSceneByName(entry.Scene);
        bool loadedHere = !scene.IsValid();
        ThreadPriority previousPriority = Application.backgroundLoadingPriority;
        if (loadedHere)
        {
            // The game lowers this while playing, which makes an additive scene load crawl.
            Application.backgroundLoadingPriority = ThreadPriority.High;
            AsyncOperation load = SceneManager.LoadSceneAsync(entry.Scene, LoadSceneMode.Additive);
            while (!load.isDone)
            {
                yield return null;
            }

            Application.backgroundLoadingPriority = previousPriority;
            scene = SceneManager.GetSceneByName(entry.Scene);
            Core.Logic.SetCurrentLevelConfig(savedConfig);
        }

        GameObject source = scene.IsValid() ? scene.FindObject(entry.Path, false) : null;
        if (source == null)
        {
            ModLog.Warn($"MonsterEditor: no spawn point at {entry.Scene}/{entry.Path}");
            _status = "template missing: " + entry.Name;
        }
        else
        {
            EnemySpawnPoint probe = source.GetComponent<EnemySpawnPoint>();
            if (probe != null && probe.SelectedEnemy == null)
            {
                ModLog.Warn($"MonsterEditor: template {entry.Id} carries no enemy prefab");
            }

            if (loadedHere)
            {
                PauseForeignScene(scene, source);
            }

            GameObject template = Instantiate(source);
            template.name = "template-" + entry.Id;
            template.SetActive(false);
            DontDestroyOnLoad(template);
            _templates[index] = template;
            ModLog.Info($"MonsterEditor: template ready {entry.Id} {entry.Name}");
        }

        if (loadedHere)
        {
            AsyncOperation unload = SceneManager.UnloadSceneAsync(scene);
            while (!unload.isDone)
            {
                yield return null;
            }

            Core.Logic.SetCurrentLevelConfig(savedConfig);
        }
    }

    /// <summary>
    /// Stops the loaded room from doing anything on its own. Only the root that holds the
    /// spawn point stays active, so the room's camera pan, level initializer and enemies are
    /// frozen and cannot reach into the level that is actually running.
    /// </summary>
    private static void PauseForeignScene(Scene scene, GameObject keep)
    {
        Transform keepRoot = keep.transform.root;
        GameObject[] roots = scene.GetRootGameObjects();
        for (int i = 0; i < roots.Length; i++)
        {
            if (roots[i].transform != keepRoot)
            {
                roots[i].SetActive(false);
            }
        }
    }

    private void SpawnAt(int index, float x, float y)
    {
        MonsterEntry entry = EntryAt(index);

        if (entry.Kind != MonsterKind.Enemy)
        {
            try
            {
                SpawnMarker(entry, x, y);
            }
            catch (System.Exception exception)
            {
                _status = "error: " + entry.Name;
                ModLog.Error($"MonsterEditor: marker {entry.Id} failed with {exception.GetType().Name}: {exception.Message}\n{exception.StackTrace}");
            }

            return;
        }

        GameObject template;
        if (!_templates.TryGetValue(index, out template) || template == null)
        {
            _status = "no template for " + entry.Name;
            return;
        }

        try
        {
            SpawnCore(entry, template, x, y);
        }
        catch (System.Exception exception)
        {
            _status = "error: " + entry.Name;
            ModLog.Error($"MonsterEditor: spawn {entry.Id} failed with {exception.GetType().Name}: {exception.Message}\n{exception.StackTrace}");
        }
    }

    private void SpawnCore(MonsterEntry entry, GameObject template, float x, float y)
    {
        EnsureLevelConfig();

        Vector3 target = new Vector3(x, y, 0f);
        GameObject root = Instantiate(template, target, Quaternion.identity);
        root.name = "editor-spawn-" + entry.Id;
        root.SetActive(true);
        _spawnRoots.Add(root);

        EnemySpawnPoint point = root.GetComponent<EnemySpawnPoint>();
        if (point == null)
        {
            _status = "template lost its spawn point";
            return;
        }

        Transform child = root.transform.Find("Spawn Point");
        if (child != null)
        {
            child.localPosition = Vector3.zero;
        }

        EnsureMeltedLadyPoints(point.SelectedEnemy, target);

        // EntityName must not be empty: CreateEnemy names the clone through it and
        // dereferences its Length, so a null name throws inside the game's own spawner.
        point.SpawnEnabledEnemy = false;
        point.EntityName = entry.Name;
        point.EnableBehaviourOnLoad = true;
        point.EnablePersistence = false;
        point.Consumed = false;
        point.CreateEnemy();

        Enemy enemy = point.SpawnedEnemy;
        if (enemy == null)
        {
            _status = "spawn failed: " + entry.Name;
            ModLog.Warn($"MonsterEditor: spawn {entry.Id} produced no enemy (selectedEnemy={(point.SelectedEnemy != null)})");
            return;
        }

        // Shadows belong to the source room: EntityShadow and BlobShadow both fail outside
        // it (the latter logs "Blob shadow needs an entity to works" and leaves an
        // ownerless sprite behind). Strip them while the clone is still inactive.
        EntityShadow[] shadows = enemy.GetComponentsInChildren<EntityShadow>(true);
        for (int i = 0; i < shadows.Length; i++)
        {
            shadows[i].enabled = false;
        }

        BlobShadow[] blobs = enemy.GetComponentsInChildren<BlobShadow>(true);
        for (int i = 0; i < blobs.Length; i++)
        {
            SpriteRenderer renderer = blobs[i].GetComponent<SpriteRenderer>();
            if (renderer != null)
            {
                renderer.enabled = false;
            }

            blobs[i].enabled = false;
        }

        enemy.transform.position = target;
        enemy.gameObject.SetActive(true);

        WakeLateSpawn(enemy);
        ReportEnemy(entry.Id, enemy);
        _status = $"spawned {entry.Name} at ({x}, {y})";
    }

    /// <summary>
    /// MeltedLady reads every teleport marker once, in her OnStart, and switches herself off
    /// when she finds none, which looks exactly like a failed spawn. The marker array has a
    /// private setter, so a point added afterwards can never reach her: the point has to
    /// exist before she starts. Place one at her own position so a plain spawn still works.
    /// </summary>
    private void EnsureMeltedLadyPoints(GameObject prefab, Vector3 target)
    {
        if (prefab == null || prefab.GetComponentInChildren<MeltedLady>(true) == null)
        {
            return;
        }

        if (Object.FindObjectsOfType<MeltedLadyTeleportPoint>().Length > 0)
        {
            return;
        }

        CreateMarker<MeltedLadyTeleportPoint>("editor-melted-lady-point", target);
        ModLog.Info($"MonsterEditor: MeltedLady had no teleport point here, placed one at ({target.x:0.#},{target.y:0.#})");
    }

    /// <summary>Places a bare marker. These have no scene template, so none is loaded.</summary>
    private void SpawnMarker(MonsterEntry entry, float x, float y)
    {
        Vector3 target = new Vector3(x, y, 0f);

        if (entry.Kind == MonsterKind.MeltedLadyPoint)
        {
            CreateMarker<MeltedLadyTeleportPoint>("editor-melted-lady-point", target);
            _status = $"melted lady point at ({x}, {y})";
            ModLog.Info($"MonsterEditor: MeltedLadyTeleportPoint at ({x}, {y})");
            return;
        }

        GameObject marker = CreateMarker<ReekSpawnPoint>("editor-reek-point", target);
        ReekSpawnPoint point = marker.GetComponent<ReekSpawnPoint>();
        point.SpawnedEntityId = 0;

        ReekLeader leader = Object.FindObjectOfType<ReekLeader>();
        ReekSpawner spawner = leader != null && leader.Behaviour != null ? leader.Behaviour.ReekSpawner : null;
        if (spawner == null)
        {
            _status = "point placed, but no ReekLeader here - spawn EN18 first";
            ModLog.Warn("MonsterEditor: no ReekLeader in this level, so this ReekSpawnPoint will never be used");
            return;
        }

        if (!spawner.ReekSpawnPoints.Contains(point))
        {
            spawner.ReekSpawnPoints.Add(point);
        }

        if (!_reekPoints.Contains(point))
        {
            _reekPoints.Add(point);
        }

        _status = $"reek point registered ({spawner.ReekSpawnPoints.Count} points on {leader.name})";
        ModLog.Info($"MonsterEditor: ReekSpawnPoint at ({x}, {y}) registered to {leader.name}");
    }

    private GameObject CreateMarker<T>(string name, Vector3 position) where T : Component
    {
        GameObject marker = new GameObject(name);
        marker.transform.position = position;
        marker.AddComponent<T>();
        _spawnRoots.Add(marker);
        return marker;
    }

    /// <summary>
    /// Many enemies run <c>Behaviour.enabled = false</c> in OnAwake and only re-enable it
    /// from <c>SpawnManager.OnPlayerSpawn</c>, which fired long before this editor spawns
    /// anything. ChainedAngel, BellGhost, LanceAngel, Legionary, WalkingTomb, JarThrower,
    /// CowardTrapper, Roller, Runner, ShieldMaiden, FloatingLady, ShooterProcessioner,
    /// RangedBoomerang, BellCarrier, PietyMonster and BejeweledSaintHead all follow that
    /// pattern, so replay the two steps their handlers perform on this enemy only.
    /// </summary>
    private static void WakeLateSpawn(Enemy enemy)
    {
        if (enemy.Target == null && Core.Logic.Penitent != null)
        {
            enemy.Target = Core.Logic.Penitent.gameObject;
        }

        EnemyBehaviour[] behaviours = enemy.GetComponentsInChildren<EnemyBehaviour>(true);
        int woken = 0;
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (!behaviours[i].enabled)
            {
                behaviours[i].enabled = true;
                woken++;
            }
        }

        ModLog.Info($"MonsterEditor: {enemy.name} woke {woken} behaviour(s), target={(enemy.Target != null)}");
    }

    private static void ReportEnemy(string id, Enemy enemy)
    {
        EnemyBehaviour[] behaviours = enemy.GetComponentsInChildren<EnemyBehaviour>(true);
        int sleeping = 0;
        for (int i = 0; i < behaviours.Length; i++)
        {
            if (!behaviours[i].enabled)
            {
                sleeping++;
            }
        }

        int stateMachines = enemy.GetComponentsInChildren<PlayMakerFSM>(true).Length;
        Vector3 position = enemy.transform.position;

        ModLog.Info(
            $"MonsterEditor: {id} active={enemy.gameObject.activeInHierarchy}"
            + $" behaviours={behaviours.Length} sleeping={sleeping} pmfsm={stateMachines}"
            + $" pos=({position.x:0.#},{position.y:0.#})");
    }

    private void ClearSpawned()
    {
        ReekLeader[] leaders = Object.FindObjectsOfType<ReekLeader>();
        for (int i = 0; i < _reekPoints.Count; i++)
        {
            if (_reekPoints[i] == null)
            {
                continue;
            }

            for (int j = 0; j < leaders.Length; j++)
            {
                ReekSpawner spawner = leaders[j] != null && leaders[j].Behaviour != null
                    ? leaders[j].Behaviour.ReekSpawner
                    : null;
                if (spawner != null)
                {
                    // Leaving a destroyed marker in this list makes the leader throw later.
                    spawner.ReekSpawnPoints.Remove(_reekPoints[i]);
                }
            }
        }

        _reekPoints.Clear();

        // Kill the monsters first: a live MeltedLady keeps a snapshot of the teleport
        // markers and would throw on her next teleport if they vanished underneath her.
        for (int i = 0; i < _spawnRoots.Count; i++)
        {
            if (_spawnRoots[i] == null)
            {
                continue;
            }

            EnemySpawnPoint point = _spawnRoots[i].GetComponent<EnemySpawnPoint>();
            if (point != null && point.SpawnedEnemy != null)
            {
                Destroy(point.SpawnedEnemy.gameObject);
            }
        }

        for (int i = 0; i < _spawnRoots.Count; i++)
        {
            if (_spawnRoots[i] != null)
            {
                Destroy(_spawnRoots[i]);
            }
        }

        _spawnRoots.Clear();
        _status = "cleared";
        ModLog.Info("MonsterEditor: cleared every monster this session placed");
    }
}