using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;

namespace Blasphemous.LevelFrameworkExtended.Components;

internal class LevelObject
{
    public string id;
    public ILoader loader;
    public IModifier modifier;

    public LevelObject(string id, ILoader loader, IModifier modifier)
    {
        this.id = id;
        this.loader = loader;
        this.modifier = modifier;
    }

    public void Register(ModServiceProvider provider)
    {
        provider.RegisterObjectCreator(id, new ObjectCreator(loader, modifier));
    }
}