using Blasphemous.LevelFrameworkExtended.ObjectModifiers;
using HarmonyLib;
using Tools.Level.Actionables;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.Patches;

/// <summary>
/// Supplies graph-specific fields for the template's existing Use() link setup.
/// The framework's ordinary BloodModifier prefix is inactive during graph setup, so this
/// prefix is the only one that writes the graph's per-platform link fields.
/// </summary>
[HarmonyPatch(typeof(FaithPlatform), "Use")]
internal static class FaithPlatform_UseGraphLink_Patch
{
    private static bool Prefix(ref bool ___firstPlatform, ref GameObject[] ___target)
    {
        BloodPlatformGraphLink link = BloodPlatformGraphLink.Current;
        if (link == null)
            return true;

        ___firstPlatform = link.IsInitial;
        ___target = link.Targets;
        return false;
    }
}
