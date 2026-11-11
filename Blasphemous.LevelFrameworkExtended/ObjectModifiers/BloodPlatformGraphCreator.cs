using Blasphemous.Framework.Levels;
using Blasphemous.ModdingAPI;

namespace Blasphemous.LevelFrameworkExtended.ObjectModifiers;

/// <summary>
/// Registers the graph type with the ordinary platform's asset and a graph-only modifier.
/// </summary>
public static class BloodPlatformGraphCreator
{
    /// <summary>Registers platform-blood-graph.</summary>
    public static void Register(ModServiceProvider provider)
    {
        provider.RegisterObjectCreator("platform-blood-graph",
            new ObjectCreator(new BloodPlatformGraphTemplate(), new BloodPlatformGraphModifier()));
    }
}
