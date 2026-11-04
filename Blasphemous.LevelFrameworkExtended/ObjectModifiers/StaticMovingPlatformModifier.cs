using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Modifiers;
using Gameplay.GameControllers.Environment.MovingPlatforms;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

/// <summary>Leaves a moving platform object in a static state.</summary>
/// <param name="name">The name to assign to the object.</param>
internal sealed class StaticMovingPlatformModifier(string name) : IModifier
{
    /// <summary>Renames the object and disables its moving platform components.</summary>
    /// <param name="obj">The loaded object to modify.</param>
    /// <param name="data">The LevelEdit data for the object.</param>
    public void Apply(GameObject obj, ObjectData data)
    {
        obj.name = name;
        foreach (StraightMovingPlatform platform in obj.GetComponentsInChildren<StraightMovingPlatform>(true))
            platform.enabled = false;
    }
}
