using Blasphemous.Framework.Levels.Loaders;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

/// <summary>
/// Loads the ordinary platform asset once for graph nodes.
/// </summary>
public class BloodPlatformGraphTemplate : ILoader
{
    internal static GameObject Current { get; private set; }

    private readonly SceneLoader _loader = new("D17Z01S10_LOGIC", "LOGIC/INTERACTABLES/{0}");

    /// <inheritdoc/>
    public GameObject Result { get; private set; }

    /// <inheritdoc/>
    public IEnumerator Apply()
    {
        yield return _loader.Apply();
        Result = _loader.Result;
        Current = Result;
    }
}
