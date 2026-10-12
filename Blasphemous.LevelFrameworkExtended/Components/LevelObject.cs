using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.ModdingAPI;

namespace Blasphemous.LevelFrameworkExtended.Components;

internal class LevelObject(string id, ILoader loader, IModifier modifier)
{
    public string id = id;
    public ILoader loader = loader;
    public IModifier modifier = modifier;

    public void Register(ModServiceProvider provider)
    {
        provider.RegisterObjectCreator(id, new ObjectCreator(loader, modifier));
    }
}