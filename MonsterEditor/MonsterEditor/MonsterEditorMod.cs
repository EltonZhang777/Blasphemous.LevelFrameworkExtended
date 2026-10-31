using System.Collections;
using Blasphemous.ModdingAPI;
using MonsterEditor.Components;
using UnityEngine;

namespace MonsterEditor;

/// <summary>
/// Registers the monster catalogue as level objects and starts the in-game editor.
/// </summary>
public class MonsterEditorMod : BlasMod
{
    private static bool _toolCreated;

    internal MonsterEditorMod()
        : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION)
    {
    }

    /// <inheritdoc/>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        foreach (LevelObjectEntry entry in MonsterEditorObjects.CreateAll())
        {
            entry.Register(provider);
        }
    }

    /// <inheritdoc/>
    protected override void OnLevelPreloaded(string oldLevel, string newLevel)
    {
        // Spawning while a level is transitioning corrupts the camera manager and the level
        // config, so the editor closes its door for the whole transition.
        MonsterEditorTool.SetLevelReady(false);
    }

    /// <inheritdoc/>
    protected override void OnLevelLoaded(string oldLevel, string newLevel)
    {
        MonsterEditorTool.CaptureLevelConfig();
        MonsterEditorTool.SetLevelReady(true);

        if (_toolCreated)
        {
            return;
        }

        _toolCreated = true;
        GameObject tool = new GameObject("MonsterEditorTool");
        Object.DontDestroyOnLoad(tool);
        tool.AddComponent<MonsterEditorTool>();
        ModLog.Info("MonsterEditor: tool created");
    }

    /// <inheritdoc/>
    protected override void OnLevelUnloaded(string oldLevel, string newLevel)
    {
        MonsterEditorTool.SetLevelReady(false);
        MonsterEditorTool.InvalidateLevelConfig();
    }
}