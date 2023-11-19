using BepInEx;
using On;

namespace HaikuAP;

[BepInPlugin("haiku.apclient", "Haiku AP Client", "0.5")]
public class Plugin : BaseUnityPlugin
{
    private void Awake()
    {
        // Plugin startup logic
        Logger.LogInfo($"HI, IT'S COMMAND!");
    }
}