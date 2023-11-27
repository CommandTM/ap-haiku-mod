using BepInEx;

namespace HaikuAP;

[BepInPlugin("haiku.apclient", "Haiku AP Client", "0.5")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogError($"AW-03=AW-01 Is A Useless Entrance");
        Logger.LogError($"TLB-T=TLB-03 Is A Useless Entrance");
        Logger.LogError($"IB-01=AW-02 Is A Useless Entrance");
        Logger.LogError($"SW-TW=SW-01 Is A Useless Entrance");
        Logger.LogError($"CC-01=CC-QP Is A Useless Entrance");
        Logger.LogError($"CC-04=CC-01 Is A Useless Entrance");
        
        Logger.LogError($"CC-QP Is A Useless Region");
    }
}