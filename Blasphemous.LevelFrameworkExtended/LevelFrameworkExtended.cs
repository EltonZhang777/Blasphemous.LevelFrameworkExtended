using Blasphemous.Framework.Levels;
using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers;
using Blasphemous.ModdingAPI;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended;

public class LevelFrameworkExtended : BlasMod
{
    internal LevelFrameworkExtended() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        // Level edits

        // spikes
        provider.RegisterObjectCreator("spikes-jondo-tiny", new ObjectCreator(
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/inverted-bell-spritesheet_56"),
            new SpikeModifier(new Vector2(0.9f, 0.8f))));
        provider.RegisterObjectCreator("spikes-jondo", new ObjectCreator(
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/inverted-bell-spritesheet_23"),
            new SpikeModifier()));
        provider.RegisterObjectCreator("spikes-jondo-long", new ObjectCreator(
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/inverted-bell-spritesheet_25"),
            new SpikeModifier(new Vector2(4f, 0.8f))));
        provider.RegisterObjectCreator("spikes-patio", new ObjectCreator(
            new SceneLoader("D04Z01S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier(new Vector2(2.6f, 0.8f))));
        provider.RegisterObjectCreator("spikes-canvases", new ObjectCreator(
            new SceneLoader("D05Z02S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier(new Vector2(3f, 0.8f))));
        provider.RegisterObjectCreator("spikes-rooftops", new ObjectCreator(
            new SceneLoader("D06Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()));
        provider.RegisterObjectCreator("spikes-brotherhood", new ObjectCreator(
            new SceneLoader("D17BZ02S01_DECO", "MIDDLEGROUND (1)/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()));
        provider.RegisterObjectCreator("spikes-miriam", new ObjectCreator(
            new SceneLoader("D23Z01S05_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()));

        // other traps
        provider.RegisterObjectCreator("bell-face", new ObjectCreator(
            new SceneLoader("D03Z02S06_LOGIC", "TRAPS/TRAP_SHOCK_ENEMY"),
            new NoModifier("Face bell")));

        // ladders
        provider.RegisterObjectCreator("ladder-jondo", new ObjectCreator(
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(0.8f)));
        provider.RegisterObjectCreator("ladder-brotherhood", new ObjectCreator(
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/brotherhood-spritesheet_43"),
            new LadderModifier(1.6f)));

        // droppable platforms
        provider.RegisterObjectCreator("platform-droppable-library", new ObjectCreator(
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/library_spritesheet_34"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))));
        provider.RegisterObjectCreator("platform-droppable-brotherhood", new ObjectCreator(
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_41"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))));

        // solid objects
        provider.RegisterObjectCreator("platform-solid-brotherhood", new ObjectCreator(
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))));

    }
}
