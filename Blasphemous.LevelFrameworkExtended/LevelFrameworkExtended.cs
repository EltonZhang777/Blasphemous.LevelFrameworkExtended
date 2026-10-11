using Blasphemous.Framework.Levels.Loaders;
using Blasphemous.Framework.Levels.Modifiers;
using Blasphemous.LevelFrameworkExtended.Components;
using Blasphemous.LevelFrameworkExtended.ObjectModifiers;
using Blasphemous.ModdingAPI;
using Gameplay.GameControllers.Environment.AreaEffects;
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
        new LevelObject("spikes-wasteland-inside-house",
            new SceneLoader("D01Z03S02_DECO", "MIDDLEGUROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-Mountain",
            new SceneLoader("D02Z01S05_DECO", "MIDDLEGUROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-jondo",
            new SceneLoader("D03Z02S03_DECO", "MIDDLEGUROUND/AfterPlayer/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-Garden",
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGUROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-Cemetery",
            new SceneLoader("D05Z02S01_DECO", "MIDDLEGUROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-Tower",
            new SceneLoader("D06Z01S04_DECO", "MIDDLEGUROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()),
        new LevelObject("spikes-Tower",
            new SceneLoader("D19Z01S02_DECO", "MIDDLEGUROUND/AfterPlayer/Gameplay/Spikes/{0}"),
            new SpikeModifier()),
        #endregion Spikes

        # region Traps
        // other traps
        new LevelObject("bell-face",
            new SceneLoader("D03Z02S06_LOGIC", "TRAPS/TRAP_SHOCK_ENEMY"),
            new NoModifier("Face bell")),
        new LevelObject("bell-face",
            new SceneLoader("D03Z02S06_LOGIC", "TRAPS/TRAP_SHOCK_CHAIN_REACTION"),
            new NoModifier("Face bell")),
        new LevelObject("Trunk in the Wasteland",
            new SceneLoader("D01Z03S01_LOGIC", "LOGIC/{0}/{0}/{0}"),
            new NoModifier("Trunk")),
        new LevelObject("Bell That can be broken and open some other doors or traps",
            new SceneLoader("D01Z04S01_LOGIC", "LOGIC/{0}/{2}/{0}"),
            new NoModifier("Shock Receiver")),
        new LevelObject("Traps Spike",
            new SceneLoader("D01Z04S05_LOGIC", "LOGIC/Traps/Trap_Spikes/{0}"),
            new NoModifier("Trap Spike")),//If you wanna blood spritesheet,in:D01Z04S05_DECO,Middleground/AfterPlayer/Props/Blood/cathedral-sprite-sheet_173
        new LevelObject("Turret",
            new SceneLoader("D01Z04S06_LOGIC", "LOGIC/Traps/{0}/{0}"),
            new NoModifier("Trap Turret")),
        new LevelObject("Trap Sewage",
            new SceneLoader("D01Z05S01_LOGIC", "Traps/Trap_Sewage/{0}"),//如果你需要其他的管道 它们在：D01Z05S01_DECO/Middleground/AfterPlayer/Props/Sewage_63和_64和_64(1)
            new NoModifier("Trap Sewage")),//另外：地上的毒液（提示上面有该陷阱用）：D01Z05S01_DECO/{0}/{0}
        new LevelObject("Traps Poison Mist",
            new SceneLoader("D01Z05S08_LOGIC", "TRAPS/PoisonAreaEffect/{0}"),
            new NoModifier("Poison Mist")),
        new LevelObject("Traps 开关-golden",
            new SceneLoader("D02Z03S02_LOGIC", "LOGIC/Interactables/{1}/{2}"),
            new NoModifier("开关-golden")),
        new LevelObject("Traps OilPot",
            new SceneLoader("D02Z03S06_LOGIC", "Traps/{0}/{0},{1}"),
            new NoModifier("Oilpot")),
        new LevelObject("Traps 滑索电梯",
            new SceneLoader("D09Z01S06_LOGIC", "LOGIC/{1}/{1}/{0}/{0}"),
            new NoModifier("Elevator--")),
        new LevelObject("Traps 滑索电梯（黑白）",
            new SceneLoader("D02Z03S10_LOGIC", "LOGIC/{2}/{1}/{0}/{0}"),
            new NoModifier("Elevator--")),
        new LevelObject("Traps-香炉",
            new SceneLoader("D04Z02S01_LOGIC", "Traps/{0}"),
            new NoModifier("Oilpot")),
        new LevelObject("Traps-超级无敌巨大香炉",
            new SceneLoader("D04Z02S16_LOGIC", "Traps/{0}/{0}"),
            new NoModifier("Oilpot")),
        new LevelObject("Traps-Movable Platform",
            new SceneLoader("D05Z01S04_LOGIC", "LOGIC/GEO_BlockOWD_Moving (128x32) (1)"),//这个单位很特殊，请自己查看一下
            new NoModifier("Movable Platform")),
        new LevelObject("Traps-Ghost",
            new SceneLoader("D05Z01S04_LOGIC", "TRAPS/GhostTrap"),//{0}：GhostTrapManager，May be useful.
            new NoModifier("Ghost")),
        new LevelObject("Traps-book",
            new SceneLoader("D05Z01S09_DECO", "MIDDLEGROUND/Afterplayer/Props/Riddlebooks"),
            new NoModifier("book")),
        new LevelObject("Traps-Guillotine",
            new SceneLoader("D05Z02S11_LOGIC", "Traps/{0}/{0}"),
            new NoModifier("book")),
        new LevelObject("Traps-Light in maze",
            new SceneLoader("D20Z01S04_LOGIC", "LOGIC/INTERACTABLES/{0}"),
            new NoModifier("LIGHT")),
        new LevelObject("Traps-ElectBall",
            new SceneLoader("D20Z01S04_LOGIC", "TRAP/{0}/{0}"),
            new NoModifier("ELECTRIC BALL")),
        new LevelObject("Traps-ladder OPENER",
            new SceneLoader("D20Z01S04_LOGIC", "LOGIC/INTERACTABLES/{2}"),
            new NoModifier("Opener")),
        new LevelObject("Traps-ladder",
            new SceneLoader("D20Z01S04_LOGIC", "LOGIC/INTERACTABLES/{1}"),//有巨大概率需要和上一条合并
            new NoModifier("Ladder Unfold")),
        new LevelObject("Traps-Ship",
            new SceneLoader("D20Z02S09_LOGIC", "LOGIC/INTERACTABLES/{3}/{2}"),
            new NoModifier("Ship")),
        new LevelObject("Traps-翻转王座",
            new SceneLoader("D24Z01S01_LOGIC", "LOGIC/INTERACTABLES/{0}/Act_Platform/Body"),
            new NoModifier("fzwz")),




        
        
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
        new LevelObject("ladder-WasteLand-inside-house",
            new SceneLoader("D01Z03S02_DECO", "MIDDLEGROUND/AfterPlayer/Ladders/tower-spritesheet_23(2)"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-MercyDreams1",
            new SceneLoader("D01Z04S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-MercyDreams2",
            new SceneLoader("D01Z04S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{1}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Cistern",
            new SceneLoader("D01Z05S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{2}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Verdical-Graveryard",
            new SceneLoader("D02Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),//{1}，{2}，{3}均为梯子。可以考虑合并
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Convent",
            new SceneLoader("D02Z03S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0},{1}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Border",
            new SceneLoader("D03Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Border",
            new SceneLoader("D03Z01S03_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Garden",
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),//这里给出所有的梯子位置，图源不一致。请合理注册其中需要的
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Library",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladder/{0}-{5}"),//这里给出所有的梯子位置，图源不一致。请合理注册其中需要的
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Library",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladder/{0}-{5}"),//这里给出所有的梯子位置，图源不一致。请合理注册其中需要的
            new LadderModifier(1.6f)),
        new LevelObject("ladder-Cemetery",
            new SceneLoader("D05Z02S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0},{1},{2}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-prison",
            new SceneLoader("D09Z01S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-prisonwall",
            new SceneLoader("D09Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/{0}"),
            new LadderModifier(1.6f)),
        new LevelObject("ladder-maze",
            new SceneLoader("D20Z01S02_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/naval-cave-spritesheet_45"),//464748
            new LadderModifier(1.6f)),
        new LevelObject("ladder-sea-cave",
            new SceneLoader("D20Z02S03_DECO", "MIDDLEGROUND/AfterPlayer/Gameplay/Ladders/naval-cave-spritesheet_52"),//5150
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
        new LevelObject("platform-droppable-Verdical-Graveryard",
            new SceneLoader("D02Z02S04_LOGIC", "TRAPS/DECO_SNOW_EFFECTS/{2}"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-Theborder",
            new SceneLoader("D03Z01S01_LOGIC", "LOGIC/{0}/{3}/GEO_ElusiveBlock_TheBorder (64x32) (3)"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-Theborder2",
            new SceneLoader("D03Z02S02_LOGIC", "LOGIC/{0}/GEO_BlockOWD_Moving4/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-Garden",
            new SceneLoader("D04Z01S01_LOGIC", "LOGIC/{1}/{1}/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-Garden2",
            new SceneLoader("D04Z01S01_LOGIC", "TRAPS/{3}/{2}"),//{0}不知道是什么，请查看一下
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-droppable-Garden",
            new SceneLoader("D03Z03S05_LOGIC", "LOGIC/{0}/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),




        
        #endregion Droppable Platforms

        #region Solid Objects
        // solid objects
        new LevelObject("platform-solid-brotherhood1",
            new SceneLoader("D17Z01S04_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-brotherhood2",
            new SceneLoader("D17Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_41"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-brotherhood3",
            new SceneLoader("D17Z01S13_DECO", "MIDDLEGROUND/AfterPlayer/Floor/chapel-exterior-spritesheet_2"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-brotherhood4",
            new SceneLoader("D17Z01S13_DECO", "MIDDLEGROUND/AfterPlayer/Floor/chapel-exterior-spritesheet_4"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-brotherhood5",
            new SceneLoader("D17Z01S13_DECO", "MIDDLEGROUND/AfterPlayer/Floor/chapel-exterior-spritesheet_1"),
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-brotherhood6",
            new SceneLoader("D17Z01S15_DECO", "MIDDLEGROUND/AfterPlayer/Floor/mea-culpa-chapel-spritesheet_14"),//有细微差别：12 16
            new ColliderModifer("Floor", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-brotherhood-slope",
            new SceneLoader("D17Z01S01_DECO", "MIDDLEGROUND/AfterPlayer/Floor/brotherhood-spritesheet_3"),
            new ColliderModifer("OneWayDown", new Vector2(2f, 1f), new Vector2(0f, -0.3f))),
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
        new LevelObject("platform-solid-Santos", //喇叭哥
            new SceneLoader("D01BZ07S01_DECO", "MIDDLEGROUND/{3}/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Boss Ossary", //死歌
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
        new LevelObject("platform-solid-wasteLand-floor",
            new SceneLoader("D01Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/churches-field-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-floor",
            new SceneLoader("D01Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/churches-field-spritesheet-improved_1"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-floor2",
            new SceneLoader("D01Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/churches-field-spritesheet-improved_0(5)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-trunkfloor",
            new SceneLoader("D01Z03S01_LOGIC", "Logic/{0}/Geo_Block_Actuable_Bridge(128x32)/{1}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-floor-inside-house",
            new SceneLoader("D01Z03S02_DECO", "MIDDLEGROUND/{1}/Floor/{32}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-floor-inside-house",
            new SceneLoader("D01Z03S02_DECO", "MIDDLEGROUND/{1}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-floor3",
            new SceneLoader("D01Z03S05_DECO", "MIDDLEGROUND/{1}/Floor/churches-field-spritesheet_improved_21(8)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-wasteLand-floor4",
            new SceneLoader("D01Z03S05_DECO", "MIDDLEGROUND/{1}/Floor/churches-field-spritesheet_improved_32(6)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Blood",
            new SceneLoader("D01Z03S05_LOGIC", "LOGIC_{1}/{1}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-MercyDreams-Stairs",
            new SceneLoader("D01Z04S01_DECO", "MIDDLEGROUND/{1}/Floor/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-MercyDreams-Stairs",
            new SceneLoader("D01Z04S01_DECO", "MIDDLEGROUND/{1}/Floor/mercy-dreams-spritesheet_0(1)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-MercyDreams-Stairs",
            new SceneLoader("D01Z04S01_DECO", "MIDDLEGROUND/{1}/WoodenPlatforms/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-MercyDreams-Boss",
            new SceneLoader("D01Z04S18_DECO", "Afterplayer/Floor/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Sheng-Rong",
            new SceneLoader("D01Z04S19_DECO", "MIDDLEGROUND/{1}/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Sheng-Rong",
            new SceneLoader("D01Z04S19_DECO", "MIDDLEGROUND/{1}/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Cristern1",
            new SceneLoader("D01Z05S01_DECO", "MIDDLEGROUND/{0}/{1}/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),//如果需要合并注册，请和下一条合并
        new LevelObject("platform-solid-Cristern1(附件)",
            new SceneLoader("D01Z05S01_DECO", "MIDDLEGROUND/{0}/{1}/sewers-spritesheet_3(1)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Cristern2",
            new SceneLoader("D01Z05S01_DECO", "MIDDLEGROUND/{0}/{1}/sewers-spritesheet_0(1)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-电梯井",
            new SceneLoader("D01Z05S25_DECO", "MIDDLEGROUND/{0}/{1}/tree-metal-tower-spritesheet_0(1)"),//电梯的文件非常复杂，因此我只给出路径。要注册的元素很多：D1Z05S25_LOGIC/Interactables/Act_Elevator/其中的所有文件均有关
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-水银提交处",
            new SceneLoader("D01Z05S27_DECO", "MIDDLEGROUND/{0}/{1}/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Jibrael's-Cave",
            new SceneLoader("D01Z06S01_DECO", "MIDDLEGROUND/{0}/{1}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Mountain1",
            new SceneLoader("D02Z01S01_DECO", "MIDDLEGROUND/{0}/{1}/{2}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Mountain1-Wooden1",
            new SceneLoader("D02Z01S02_DECO", "MIDDLEGROUND/{0}/{2}/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-solid-Mountain1-Wooden1",
            new SceneLoader("D02Z01S02_DECO", "MIDDLEGROUND/{0}/{2}/{1}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-三结秘舌-横置",
            new SceneLoader("D02Z01S02_LOGIC", "LOGIC/GEO_RootsBlock/{0}/{0}/{0}/{0}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-ice-cave",//那个棺材房间
            new SceneLoader("D02Z01S04_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Mountain-Underground",
            new SceneLoader("D02Z01S06_DECO", "MIDDLEGROUND/{0}/{1}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Mountain-Underground2",
            new SceneLoader("D02Z01S08_DECO", "MIDDLEGROUND/{0}/{1}/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard",
            new SceneLoader("D02Z02S01_DECO", "MIDDLEGROUND/{0}/{2}/{2}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard-Wodden",
            new SceneLoader("D02Z02S03_DECO", "MIDDLEGROUND/{0}/Woodenfloor/{2}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard2",
            new SceneLoader("D02Z02S08_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard3",
            new SceneLoader("D02Z02S10_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard-elevator",
            new SceneLoader("D02Z02S11_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard-elevator",
            new SceneLoader("D02Z02S11_DECO", "MIDDLEGROUND/{0}/Floor/tree-metal-tower-spritesheet7(2)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Vertical Graveryard-elevator",
            new SceneLoader("D02Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Convent",
            new SceneLoader("D02Z03S02_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Convent-wooden1",
            new SceneLoader("D02Z03S05_DECO", "MIDDLEGROUND/{0}/Woodenplatforms/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Convent-wooden2",
            new SceneLoader("D02Z03S05_DECO", "MIDDLEGROUND/{0}/Woodenplatforms/{1}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-TheBorder",
            new SceneLoader("D03Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-TheBorder2",
            new SceneLoader("D03Z01S03_DECO", "MIDDLEGROUND/{0}/Floor/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-TheBorder3",
            new SceneLoader("D03Z01S03_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-TheBorder",
            new SceneLoader("D03Z01S03_DECO", "MIDDLEGROUND/{0}/Floor/{3}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Moveable in TheBorder",
            new SceneLoader("D03Z01S03_LOGIC", "LOGIC/{0}/EO_BlockOWD_Moving (128x32) (1)/{3}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-descent",
            new SceneLoader("D03Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/descent-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-descent",
            new SceneLoader("D03Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/descent-spritesheet_13"),
            new ColliderModifer("Onewaydown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-不知道是啥玩意",
            new SceneLoader("D04BZ03_DECO", "MIDDLEGROUND/{0}/Floor/{3}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Garden",
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/garden-spritesheet_55"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Garden",
            new SceneLoader("D04Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/garden-spritesheet_13"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Garden",
            new SceneLoader("D04Z01S04_DECO", "MIDDLEGROUND/{0}/Floor/garden-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Archcathedral",
            new SceneLoader("D04Z02S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Archcathedral Boss-掘墓大主教",
            new SceneLoader("D04Z02S22_DECO", "MIDDLEGROUND/{0}/Floor/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-三结",
            new SceneLoader("D04Z03S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}，{3}"),//合并注册
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-跑酷DLC",
            new SceneLoader("D04Z04S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-跑酷DLC",
            new SceneLoader("D04Z04S02_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Library-wooden1",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/{2}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Library-wooden2",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/library_spritesheet_34"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Library-stone",
            new SceneLoader("D05Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Cemetery",
            new SceneLoader("D05Z02S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Cemetery2",
            new SceneLoader("D05Z02S02_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Rooftop1",
            new SceneLoader("D06Z01S01_DECO", "MIDDLEGROUND/{0}/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Rooftop-大电梯",
            new SceneLoader("D06Z01S01_LOGIC", "LOGIC/ACT_BridgeElevator"),//这个文件相当复杂，复杂程度超过了之前所有的合并注册文件，所以只是给出模糊地址，其中文件自行选用
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Rooftop2",
            new SceneLoader("D06Z01S04_DECO", "MIDDLEGROUND/{0}/Floor/towers-roofs-spritesheet_2 (2)"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Rooftop3",
            new SceneLoader("D06Z01S25_DECO", "MIDDLEGROUND/{0}/Floor/roofts-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Rooftop3",
            new SceneLoader("D06Z01S25_DECO", "MIDDLEGROUND/{0}/Floor/roofts-spritesheet_1"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),//这一条和上一条看起来似乎不像地板，实际上他们应该联合注册
        new LevelObject("platform-Rooftop3",
            new SceneLoader("D07Z01S01_DECO", "MIDDLEGROUND/{0}/Floor"),//这里面的所有地板文件有许多艺术元素，但是实际上只有全部注册才能起到原有艺术效果
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Rooftop4-up",
            new SceneLoader("D07Z01S01_DECO", "MIDDLEGROUND/{0}/Stairs"),//这里面的所有地板文件有许多艺术元素，但是实际上只有全部注册才能起到原有艺术效果
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Ash Mount",
            new SceneLoader("D07Z03S03_DECO", "MIDDLEGROUND/{0}/Stairs"),//这里面的所有地板文件有许多艺术元素，但是实际上只有全部注册才能起到原有艺术效果
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Ending Boss",
            new SceneLoader("D07Z01S02_DECO", "MIDDLEGROUND/Afterplayer/Floor/{1}，pontiff-boss-fight-spritesheet_0；MIDDLEGROUND/Beforeplayer/{0}/{0}"),//这里面的所有地板文件有许多艺术元素，但是实际上只有全部注册才能起到原有艺术效果
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-boss-Ending2",
            new SceneLoader("D07Z01S05_DECO", "Beforeplayer/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-boss-SisterBridge",
            new SceneLoader("D08Z01S01_DECO", "Middleground/AfterPlayer/BridgeAfter/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-boss-SisterBridge",
            new SceneLoader("D08Z01S01_DECO", "Middleground/AfterPlayer/Gate/{0}"),//文件里有些不是地板 但是起到了装饰作用 由你来看看怎么注册比较好吧
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Tree1",
            new SceneLoader("D08Z02S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Tree2",
            new SceneLoader("D08Z02S01_DECO", "Middleground/AfterPlayer/Floor/tree-metal-tower-spritesheet-remake_3 (3)"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Dawn Room",
            new SceneLoader("D08Z03S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),//这两条能绷住的都是圣人了
        new LevelObject("platform-Dawn Room",
            new SceneLoader("D08Z03S01_DECO", "Middleground/AfterPlayer/Floor/{1}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),//这两条能绷住的都是圣人了
        new LevelObject("platform-Tree2",
            new SceneLoader("D08Z02S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison1",
            new SceneLoader("D09BZ01S01_DECO", "Middleground/Cell17-18/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison2",
            new SceneLoader("D09BZ01S01_DECO", "Middleground/Cell14-15/AfterPlayer/Floor/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison3",
            new SceneLoader("D09BZ01S01_DECO", "Middleground/Cell14-15/AfterPlayer/Floor/{1}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison4",
            new SceneLoader("D09Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison-boss",
            new SceneLoader("D09Z01S03_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison5",
            new SceneLoader("D09Z01S05_DECO", "Middleground/AfterPlayer/Floor/prision-spritesheet_14"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison5",
            new SceneLoader("D09Z01S05_DECO", "Middleground/AfterPlayer/Floor/prision-spritesheet_16"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Prison5",
            new SceneLoader("D09Z01S05_DECO", "Middleground/AfterPlayer/Floor/prision-spritesheet_10"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Dreamsea",
            new SceneLoader("D14Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-竞技场1",
            new SceneLoader("D19Z01S01_DECO", "Middleground/AfterPlayer/Floor/combat-arena-spritesheet_6"),//相似的还有0 1 7
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-竞技场1",
            new SceneLoader("D19Z01S01_DECO", "Middleground/AfterPlayer/Floor/combat-arena-spritesheet_24"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Maze of the sleeper1",
            new SceneLoader("D20Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Maze of the sleeper2",
            new SceneLoader("D20Z01S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Maze of the sleeper3",
            new SceneLoader("D20Z01S02_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_14"),//还有13
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave1",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_64"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave2",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_22"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave3",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_20"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave4",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_16"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave5",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_0"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave-wooden1",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_44"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave-wooden2",
            new SceneLoader("D20Z02S01_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_45"),
            new ColliderModifer("OneWayDown", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave-ship1-1",
            new SceneLoader("D20Z02S03_DECO", "Middleground/AfterPlayer/Props/Ship1/naval-cave-spritesheet_14"),//这个船体构造很复杂，ship2和ship1都请自己注册
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-sea-cave-wooden",
            new SceneLoader("D20Z02S09_DECO", "Middleground/AfterPlayer/Floor/naval-cave-spritesheet_13"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Amanecidas1",
            new SceneLoader("D21Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Amanecidas2",
            new SceneLoader("D21Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Amanecidas3",
            new SceneLoader("D21Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Amanecidas4",
            new SceneLoader("D21Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-Amanecidas4",
            new SceneLoader("D21Z01S01_DECO", "Middleground/AfterPlayer/Floor/{0}"),
            new ColliderModifer("Floor", new Vector2(1, 1), new Vector2(0f, -0.3f))),
        new LevelObject("platform-圣三一",
            new SceneLoader("D21Z01S01_LOGIC", "LOGIC/{0}/{0}/Body/{0}"),
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
            new NoModifier("shelf03")),
        #endregion Shelf
        

        #region Liquid
        new LevelObject("Liquid-Mud of TheHolyLand",
            new SceneLoader("D01Z01S02_DECO", "MIDDLEGROUND/{0}/Mud/forest-spritsheet_34(8)"),
            new NoModifier("Liquid-Mud of TheHolyLand")),
        new LevelObject("Liquid-Water of Cistern",
            new SceneLoader("D01Z05S03_DECO", "MIDDLEGROUND/{1}/Poolpiece/{1}"),
            new NoModifier("Liquid-Water of Cistern")),
        #endregion Liquid
        






        #region Chest And Collectible
        new LevelObject("Golden-Chest",
            new SceneLoader("D01Z05S06_DECO", "LOGIC/Interactable/Act_WoodenChest/{1}"),//可能会用到{0}、{2}，下同
            new NoModifier("Golden-Chest")), 
        new LevelObject("Iron-Chest",
            new SceneLoader("D01Z05S11_DECO", "LOGIC/Interactable/Act_IronChest/{1}"),
            new NoModifier("Iron-Chest")),
        new LevelObject("Collectible",
            new SceneLoader("D01Z04S11_LOGIC", "LOGIC/{0}/{0}/{2}"),
            new NoModifier("Collectible")), 
        #endregion Chest And Collectible



        #region WallClimber
        new LevelObject("WallClimber-Mountain1",
            new SceneLoader("D02Z01S01_LOGIC", "Middleground/AfterPlayer/WallClimber/{0}"),
            new NoModifier("WallClimber-Mountain1")),
        new LevelObject("platform-三结秘舌-纵置",
            new SceneLoader("D02Z01S02_LOGIC", "LOGIC/GEO_RootsBlock(1)/{0}/{0}/{0}/{0}/{0}"),
            new NoModifier("WallClimber-SanjieMishe")),
        new LevelObject("WallClimber-Mountain2",
            new SceneLoader("D02Z01S06_DECO", "Middleground/AfterPlayer/WallClimber/{0}"),
            new NoModifier("WallClimber-Mountain2")),
        new LevelObject("WallClimber-VerticalGraveryard",
            new SceneLoader("D02Z02S01_DECO", "Middleground/AfterPlayer/WallClimber/{2}"),
            new NoModifier("WallClimber-Verticalgraveryard")),
        new LevelObject("WallClimber-Border  ",
            new SceneLoader("D03Z01S02_DECO", "Middleground/AfterPlayer/WallClimb/{0}"),
            new NoModifier("WallClimber-Border")),
        new LevelObject("WallClimber-Border2",
            new SceneLoader("D03Z01S03_DECO", "Middleground/AfterPlayer/WallClimb/{0}"),
            new NoModifier("WallClimber-Border2")),
        new LevelObject("WallClimber-Archcathedral",
            new SceneLoader("D04Z02S06_DECO", "Middleground/AfterPlayer/WallClimb/{0}"),
            new NoModifier("WallClimber-Archcathedral")),
        new LevelObject("WallClimber-tree",
            new SceneLoader("D08Z03S02_DECO", "Middleground/AfterPlayer/GamePlay/WallClimb/tree-metal-tower-spritesheet-remake_30"),
            new NoModifier("WallClimber-tree")),
        new LevelObject("WallClimber-Prison",
            new SceneLoader("D09BZ01S01_DECO", "Middleground/Cell23/AfterPlayer/Sidewalls/prision-spritesheet_56"),
            new NoModifier("WallClimber-Prison")),
        new LevelObject("WallClimber-竞技场",
            new SceneLoader("D19Z01S05_DECO", "Middleground/Cell23/AfterPlayer/Wallclimb/{0}"),
            new NoModifier("WallClimber-竞技场")),
    
        

        #endregion WallClimber

        #region Wall
        new LevelObject("Wall-Church1",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/{0}/Walls/{0}"),
            new NoModifier("Wall-Church1")),
        new LevelObject("Wall-Church2",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/{0}/Walls/{1}"),
            new NoModifier("Wall-Church2")),
        new LevelObject("Wall-Church3",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/{0}/Walls/{2}"),
            new NoModifier("Wall-Church3")),

        #endregion Wall//先不写
        #region Props
        new LevelObject("Props-Church1",
            new SceneLoader("D01BZ04S01_DECO", "Foreground/{0}/{0}"),
            new NoModifier("Props-Church1")),
        new LevelObject("Props-Church2",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/{0}/Arcs/{0}"),
            new NoModifier("Props-Church2")),
        new LevelObject("Props-Church2",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/{0}/Arcs/chapel-spritesheet_25 (1)"),
            new NoModifier("Props-Church3")),
        #endregion Props
        #region Door
        new LevelObject("Door-Church",
            new SceneLoader("D01BZ04S01_DECO", "MIDDLEGROUND/{0}/Arcs/{1}"),//和{2}合并注册
            new NoModifier("Door-Church")),
        new LevelObject("Door-stone",
            new SceneLoader("D01BZ09S01_DECO", "MIDDLEGROUND/{0}/Gameplay/{1}"),
            new NoModifier("Door-stone")),
        new LevelObject("Door-brotherhood",
            new SceneLoader("D01Z01S07_DECO", "MIDDLEGROUND/{0}/Gameplay/{1}"),
            new NoModifier("Door-brotherhood")),
        new LevelObject("Door-aerbeiluo",
            new SceneLoader("D01Z02S01_DECO", "MIDDLEGROUND/{0}/Gameplay/{1}"),
            new NoModifier("Door-aerbeiluo")),
        new LevelObject("Door-inner-aerbeiluo",
            new SceneLoader("D01Z02S02_DECO", "MIDDLEGROUND/{0}/Gameplay/Doors"),
            new NoModifier("Door-inner-aerbeiluo")),
        new LevelObject("Door-wallhole",
            new SceneLoader("D01Z02S02_DECO", "MIDDLEGROUND/{0}/WallHole"),
            new NoModifier("Door-wallhole")),
        new LevelObject("Door-aerbeiluo2",
            new SceneLoader("D01Z02S03_DECO", "MIDDLEGROUND/{0}/Gameplay/Doors"),
            new NoModifier("Door-aerbeiluo2")),
        new LevelObject("Door-地下",
            new SceneLoader("D01Z02S03_DECO", "MIDDLEGROUND/{0}/Gameplay/Doors"),
            new NoModifier("Door-地下")),

        #endregion Door//先不写
        


        #region ArtResource
        new LevelObject("Background-Store",
            new SceneLoader("D01BZ02S01_DECO", "MIDDLEGROUND/{0}/{1}/{0}"),
            new NoModifier("Background-Store")),
        new LevelObject("Background-石头背景",
            new SceneLoader("D01BZ02S01_DECO", "MIDDLEGROUND/{0}/{0}/{1}"),//和{2}合并注册
            new NoModifier("Background-Store")),

        #endregion ArtResource//先不写



        #region NPCs and builds
        new LevelObject("Shopkeeper",
            new SceneLoader("D01BZ02S01_DECO", "Logic/Character/NPCs/{0}"),
            new NoModifier("Shopkeeper")),
        new LevelObject("捐钱处",
            new SceneLoader("D01BZ02S01_DECO", "Logic/INTERACTABLES/{0}"),//这里的元素过于复杂，不能有效的注册，请你来注册
            new NoModifier("捐钱处")),
        new LevelObject("祭坛",
            new SceneLoader("D01Z02S06_DECO", "MIDDLEGROUND/{0}/Gameplay/alter"),
            new NoModifier("祭坛")),
        new LevelObject("teleport",
            new SceneLoader("D01Z02S07_DECO", "MIDDLEGROUND/{0}/Gameplay/{0}，{2}"),
            new NoModifier("teleport")),
        new LevelObject("teleport",
            new SceneLoader("D01Z03S07_LOGIC", "LOGIC/{0}/{0}"),
            new NoModifier("teleport")),
        new LevelObject("predieu",
            new SceneLoader("D01Z04S03_LOGIC", "LOGIC/{0}/{0}/{1}"),
            new NoModifier("predieu")),
        new LevelObject("蜡烛后面的石像",
            new SceneLoader("D01Z04S08_DECO", "Middleground/{0}/props/candle-virgin"),
            new NoModifier("蜡烛后面的石像")),
        new LevelObject("boss战帮手",
            new SceneLoader("D01Z04S12_LOGIC", "Characters/NPCs/{0}"),
            new NoModifier("boss战帮手")),
        new LevelObject("赎罪雕像",
            new SceneLoader("D01Z04S17_LOGIC", "LOGIC/{0}/{0}"),
            new NoModifier("赎罪雕像")),
        new LevelObject("水银哥",
            new SceneLoader("D01Z05S27_LOGIC", "CHARACTERS/NPCs/{0}"),
            new NoModifier("水银哥")),
        new LevelObject("加血",
            new SceneLoader("D01Z05S21_LOGIC", "CHARACTERS/NPCs/{0}"),
            new NoModifier("加血")),//这个npc的模型找不到
        new LevelObject("加蓝",
            new SceneLoader("D02Z02S10_LOGIC", "LOGIC/Interactables/{0}"),
            new NoModifier("加蓝")),

        #endregion NPCs and builds

        

        
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
