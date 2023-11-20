using BepInEx;
using On;

namespace HaikuAP;

[BepInPlugin("haiku.apclient", "Haiku AP Client", "0.5")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"AW-03=AW-01 Is A Useless Entrance");
        Logger.LogInfo($"TLB-T=TLB03 Is A Useless Entrance");
    }
}