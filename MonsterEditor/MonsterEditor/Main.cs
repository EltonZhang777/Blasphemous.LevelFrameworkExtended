using BepInEx;

namespace MonsterEditor;

[BepInPlugin(ModInfo.MOD_ID, ModInfo.MOD_NAME, ModInfo.MOD_VERSION)]
[BepInDependency("Blasphemous.ModdingAPI", "3.0.1")]
[BepInDependency("Blasphemous.Framework.Levels", "0.2.0")]
internal class Main : BaseUnityPlugin
{
    public static MonsterEditorMod MonsterEditorMod { get; private set; }

    private void Start()
    {
        MonsterEditorMod = new MonsterEditorMod();
    }
}