using Blasphemous.LevelFrameworkExtended.Components;
using Gameplay.GameControllers.Camera;
using HarmonyLib;

namespace Blasphemous.LevelFrameworkExtended.Patches;

/// <summary>Guards camera cleanup while a source scene object is loading.</summary>
[HarmonyPatch(typeof(CameraAssistedPan), "OnDestroy")]
internal static class CameraAssistedPan_OnDestroy_Patch
{
    // The temporary source-scene pan has no valid camera context during SceneLoader cleanup.
    /// <summary>Runs the original cleanup only outside a scene object load.</summary>
    [HarmonyPrefix]
    private static bool Prefix() => !LevelObject.IsLoadingSceneObject;
}
