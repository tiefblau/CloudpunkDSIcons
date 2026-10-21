using BepInEx;
using BepInEx.Logging;
using BepInEx.Unity.IL2CPP;

namespace CloudpunkDSIcons;

[BepInPlugin(MyPluginInfo.PLUGIN_GUID, MyPluginInfo.PLUGIN_NAME, MyPluginInfo.PLUGIN_VERSION)]
[BepInProcess("Cloudpunk.exe")]
[BepInProcess("Cloudpunk - City of Ghosts.exe")]
public class PluginLoader : BasePlugin
{
    internal static new ManualLogSource Log;

    public override void Load()
    {
        // Plugin startup logic
        Log = base.Log;
        Log.LogInfo($"Plugin {MyPluginInfo.PLUGIN_GUID} is loaded!");

        IL2CPPChainloader.AddUnityComponent<Plugin>();
    }
}
