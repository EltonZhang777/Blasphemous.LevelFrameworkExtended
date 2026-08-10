using Blasphemous.Framework.Levels;
using Blasphemous.ModdingAPI;
using HarmonyLib;
using System;
using System.Collections.Generic;

namespace Blasphemous.LevelFrameworkExtended.Patches;

/// <summary>
/// Intercepts the level framework's edit loading so that .jsonc files are supported.
/// Registered automatically by PatchAll when the mod is constructed.
/// </summary>
[HarmonyPatch(typeof(LevelFramework), "LoadModEdits")]
internal static class LevelEditLoaderPatch
{
    private static bool Prefix(string folder, ref object __result)
    {
        try
        {
            __result = LevelEditLoader.LoadLevelEdits(folder, ModLog.Error, ModLog.Warn);
        }
        catch (Exception e)
        {
            ModLog.Error($"Failed to load level edits from '{folder}': {e}");

            Type levelEditType = typeof(LevelFramework).Assembly.GetType(LevelEditLoader.LevelEditTypeName);
            __result = Activator.CreateInstance(typeof(Dictionary<,>).MakeGenericType(typeof(string), levelEditType));
        }
        return false;
    }
}
