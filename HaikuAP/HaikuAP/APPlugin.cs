using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Helpers;
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
        public static long BaseID = 0;
        public void Awake()
        {
            Logger.LogInfo("AP Haiku Loaded");
            BepInEx.Logging.Logger.Sources.Add(Settings.apConnection);
            BepInEx.Logging.Logger.Sources.Add(ItemMachine.Logging);
            Settings.Init(Config);
        }

        public static void InitHooks()
        {
            apSession.Items.ItemReceived += _registerItem;
        }

        private static void _registerItem(ReceivedItemsHelper helper)
        {
            ItemMachine.RunThroughQueue();
        }
    }
}
