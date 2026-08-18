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

#region LevelObject Creators
    private static readonly List<LevelObject> _creators =
    [
        #region Spikes
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
        new LevelObject("spikes-wasteland",
            new SceneLoader("D01Z03S01_DECO", "MIDDLEGUROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()),
        #endregion Spikes

        # region Traps
        // other traps
        new LevelObject("bell-face",
            new SceneLoader("D03Z02S06_LOGIC", "TRAPS/TRAP_SHOCK_ENEMY"),
            new NoModifier("Face bell")),
        #endregion Traps

        #region Ladders
        // ladders
        new LevelObject("ladder-jondo",
            new SceneLoader("D03Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(0.8f)),
        new LevelObject("ladder-brotherhood",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/brotherhood-spritesheet_43"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Aerbeiluos1",
            new SceneLoader("D01Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/village-inside-house-spritesheet_19"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Aenbeieluos2",
            new SceneLoader("D01Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/village-inside-house-spritesheet_20(2)"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Aerbeiluos3",
            new SceneLoader("D01Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/village-inside-house-spritesheet_21(1)"),
            new LadderModifier(1.6f)),
        
        #endregion Ladders

        #region Droppable Platforms
        // droppable platforms
        new LevelObject("platform-droppable-library",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/library_spritesheet_34"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-brotherhood",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_41"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        #endregion Droppable Platforms

        #region Solid Objects
        // solid objects
        new LevelObject("platform-solid-brotherhood",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Church1",
            new SceneLoader("D01Z04S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/chaple-spritesheet_13"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Church2",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/chaple-spritesheet_9"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Church3",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/chaple-spritesheet_12"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-SewerpuzzleReward",
            new SceneLoader("D01BZ05S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Church3-withcarpet",
            new SceneLoader("D01BZ05S01_LOGIC", "Interactables/{0}/chaple-spritesheet_61(1)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-ossuary",
            new SceneLoader("D01BZ06S01_DECO", "MIDDLEGROUND/{1}/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Santos",
            new SceneLoader("D01BZ07S01_DECO", "MIDDLEGROUND/{2}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
//喇叭哥
        new LevelObject("platform-solid-Boss Ossary//死歌",
            new SceneLoader("D01BZ08S01_DECO", "MIDDLEGROUND/Frontwalls(1)/FLOOR_Dark"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-TheHolyLand",
            new SceneLoader("D01Z01S01_DECO", "Afteplayer/Floor/forest-spritesheet_0(4)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-TheHolyLand2",
            new SceneLoader("D01Z01S01_DECO", "Afteplayer/Floor/forest-spritesheet_2(3)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-TheHolyLand3",
            new SceneLoader("D01Z01S01_DECO", "Afteplayer/Floor/forest-spritesheet_1"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-TheHolyLand4",
            new SceneLoader("D01Z01S02_DECO", "MIDDLEGROUND/{0}/Ruins/forest-spritsheet_51(1)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Mud of TheHolyLand",
            new SceneLoader("D01Z01S02_DECO", "MIDDLEGROUND/{0}/Mud/forest-spritsheet_34(8)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Village",
            new SceneLoader("D01Z02S01_DECO", "MIDDLEGROUND/{0}/Floor/village-spritsheet_31"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-aerbeiluo",
            new SceneLoader("D01Z02S01_DECO", "MIDDLEGROUND/{0}/Floor/village-inside-house-spritsheet_1(5)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-village-woodenfloor",
            new SceneLoader("D01Z02S03_DECO", "MIDDLEGROUND/{0}/Floor/village-sprite-sheet_27"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-village-cavestone",
            new SceneLoader("D01Z02S04  _DECO", "MIDDLEGROUND/{0}/Floor/cave-room-spritesheet_5"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-floor-in-teleport",
            new SceneLoader("D01Z02S06_DECO", "MIDDLEGROUND/{0}/Floor/teleport-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),











        #endregion Solid Objects
        //Dalin's work
        //Wip:ShelfModifer
        #region Shelf
        new LevelObject("shelf01",
            new SceneLoader("D01BZ02S01_DECO","MIDDLEGROUND/AfterPlayer/Props/Shelf01"),
            new NoModifier("shelf01")),
        new LevelObject("shelf02",
            new SceneLoader("D01BZ02S01_DECO","MIDDLEGROUND/AfterPlayer/Props/Shelf02"),
            new NoModifier("shelf02")),
        new LevelObject("shelf03",
            new SceneLoader("D01BZ02S01_DECO","MIDDLEGROUND/AfterPlayer/Props/Shelf03"),
            new NoModifier("shelf03"))
        #endregion Shelf

        
    ];
#endregion LevelObject Creators

    internal LevelFrameworkExtended() : base(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_AUTHOR, ModInfo.MOD_VERSION) { }

    ///<inheritdoc/>
    protected override void OnRegisterServices(ModServiceProvider provider)
    {
        foreach (LevelObject entry in _creators)
            entry.Register(provider);
    }
}
