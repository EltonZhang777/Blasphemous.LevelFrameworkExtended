using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;
using System.Collections;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended.Components;

/// <summary>Defines one registered Level Framework object creator.</summary>
/// <param name="id">The registered object type.</param>
/// <param name="loader">Loads the source object.</param>
/// <param name="modifier">Applies changes to the loaded object.</param>
internal class LevelObject(string id, ILoader loader, IModifier modifier)
{
    /// <summary>Gets whether a source object is being loaded from a scene.</summary>
    internal static bool IsLoadingSceneObject { get; private set; }

    public string id = id;
    public ILoader loader = loader;
    public IModifier modifier = modifier;

    /// <summary>Registers this object creator with the mod service provider.</summary>
    /// <param name="provider">The provider that stores object creators.</param>
    public void Register(ModServiceProvider provider)
    {
        provider.RegisterObjectCreator(id, new ObjectCreator(new SceneLoaderScope(loader), modifier));
    }

    /// <summary>Tracks scene loading while delegating to the wrapped loader.</summary>
    /// <param name="inner">The loader to wrap.</param>
    private sealed class SceneLoaderScope(ILoader inner) : ILoader
    {
        /// <summary>Gets the object produced by the wrapped loader.</summary>
        public GameObject Result => inner.Result;

        /// <summary>Runs the wrapped loader and tracks active scene loads.</summary>
        public IEnumerator Apply()
        {
            bool isSceneLoader = inner is SceneLoader;
            if (isSceneLoader)
                IsLoadingSceneObject = true;

            try
            {
                yield return inner.Apply();
            }
            finally
            {
                if (isSceneLoader)
                    IsLoadingSceneObject = false;
            }
        }
    }
}
