using System;
using Archipelago.MultiClient.Net;
using BepInEx;

namespace HaikuAP
{
    [BepInPlugin("haiku.apclient", "Haiku AP Client", "0.5")]
    [BepInDependency("haiku.mapi", "1.0.1.0")]
    [BepInDependency("com.bepis.bepinex.configurationmanager", "18.1")]
    public sealed class APPlugin : BaseUnityPlugin
    {
        public static ArchipelagoSession apSession;
        public static LoginResult apResult;
        public void Awake()
        {
            BepInEx.Logging.Logger.Sources.Add(Settings.apConnection);
            // Plugin startup logic
            Logger.LogError($"AW-03=AW-01 Is A Useless Entrance");
            Logger.LogError($"TLB-T=TLB-03 Is A Useless Entrance");
            Logger.LogError($"IB-01=AW-02 Is A Useless Entrance");
            Logger.LogError($"SW-TW=SW-01 Is A Useless Entrance");
            Logger.LogError($"CC-01=CC-QP Is A Useless Entrance");
            Logger.LogError($"CC-04=CC-01 Is A Useless Entrance");
            Logger.LogFatal($"CC-QP Is A Useless Region");
            
            Settings.Init(Config);
        }
    }
}
