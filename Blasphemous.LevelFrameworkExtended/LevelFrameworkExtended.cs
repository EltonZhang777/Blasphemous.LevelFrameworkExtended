using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LevelFrameworkExtended.Components;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers;
using Blasphemous.ModdingAPI;
using System.Collections.Generic;
using UnityEngine;

namespace Blasphemous.LevelFrameworkExtended;

///<inheritdoc/>
public class LevelFrameworkExtended : BlasMod
{
    private static readonly List<LevelObject> _creators =
    [
        // spikes
        new LevelObject("spikes-jondo-tiny",
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/inverted-bell-spritesheet_56"),
            new SpikeModifier(new Vector2(0.9f, 0.8f))),
        new LevelObject("spikes-jondo",
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/inverted-bell-spritesheet_23"),
            new SpikeModifier()),
        new LevelObject("spikes-jondo-long",
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/inverted-bell-spritesheet_25"),
            new SpikeModifier(new Vector2(4f, 0.8f))),
        new LevelObject("spikes-patio",
            new SceneLoader("D04Z01S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier(new Vector2(2.6f, 0.8f))),
        new LevelObject("spikes-canvases",
            new SceneLoader("D05Z02S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier(new Vector2(3f, 0.8f))),
        new LevelObject("spikes-rooftops",
            new SceneLoader("D06Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-brotherhood",
            new SceneLoader("D17BZ02S01_DECO", "MIDDLEGROUND (1)/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-miriam",
            new SceneLoader("D23Z01S05_DECO", "MIDDLEGROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()),

        // other traps
        new LevelObject("bell-face",
            new SceneLoader("D03Z02S06_LOGIC", "TRAPS/TRAP_SHOCK_ENEMY"),
            new NoModifier("Face bell")),

        // ladders
        new LevelObject("ladder-jondo",
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(0.8f)),
        new LevelObject("ladder-brotherhood",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/brotherhood-spritesheet_43"),
            new LadderModifier(1.6f)),

        // droppable platforms
        new LevelObject("platform-droppable-library",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/library_spritesheet_34"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-brotherhood",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_41"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),

        // solid objects
        new LevelObject("platform-solid-brotherhood",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
    ];

    internal LevelFrameworkExtended() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    ///<inheritdoc/>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        foreach (LevelObject entry in _creators)
            entry.Register(provider);
    }
}
