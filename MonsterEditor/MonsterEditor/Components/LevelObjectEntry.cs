using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;

namespace MonsterEditor.Components;

/// <summary>
/// One registerable level object: the id a level edit refers to, plus the loader that
/// produces the template and the modifier that places it.
/// </summary>
internal sealed class LevelObjectEntry
{
    internal LevelObjectEntry(string id, ILoader loader, IModifier modifier)
    {
        Id = id;
        Loader = loader;
        Modifier = modifier;
    }

    /// <summary>Gets the registered object type, for example <c>monster-en04</c>.</summary>
    internal string Id { get; }

    /// <summary>Gets the loader that clones the template out of its source scene.</summary>
    internal ILoader Loader { get; }

    /// <summary>Gets the modifier applied to every placed instance.</summary>
    internal IModifier Modifier { get; }

    /// <summary>Registers this object type with the level framework.</summary>
    internal void Register(ModServiceProvider provider)
    {
        provider.RegisterObjectCreator(Id, new ObjectCreator(Loader, Modifier));
    }
}