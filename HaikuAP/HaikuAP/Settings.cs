using System;
using Archipelago.MultiClient.Net;
using Archipelago.MultiClient.Net.Enums;
using BepInEx.Configuration;
using BepInEx;
using BepInEx.Logging;
using Modding;
using UnityEngine;
using Logger = HarmonyLib.Tools.Logger;

namespace HaikuAP
{
    class Settings
    {
        public static ConfigEntry<string> apIP { get; private set; }
        public static ConfigEntry<string> apPort { get; private set; }
        public static ConfigEntry<string> apSlotName { get; private set; }
        public static ConfigEntry<bool> apUsePassword { get; private set; }
        public static ConfigEntry<string> apPassword { get; private set; }

        private const string archipelago = "AP";
        public static ManualLogSource apConnection = new ManualLogSource("AP Connection");
        
        public static void Init(ConfigFile config)
        {
            apIP = config.Bind(archipelago, "Archipelago IP", "archipelago.gg", 
                "IP address used to connect");
            apPort = config.Bind(archipelago, "Archipelago Port", "38281", 
                "Port used to connect");
            apSlotName = config.Bind(archipelago, "Archipelago Slot", "Player", 
                "Slot to connect to");
            apUsePassword = config.Bind(archipelago, "Use Password?", false,
                "Should the client connect with a password?");
            apPassword = config.Bind(archipelago, "Archipelago Password", "", 
                "The password to use if enabled");
            
            ConfigManagerUtil.createButton(config, Connect, archipelago, "Connect", 
                "Connect to the Multiworld");
            
            config.Save();
        }
    
        private static void Connect()
        {
            if (!apUsePassword.Value)
            {
                apPassword.Value = null;
            }
            
            APPlugin.apSession = ArchipelagoSessionFactory.CreateSession(apIP.Value, Int32.Parse(apPort.Value));
            APPlugin.apResult = APPlugin.apSession.TryConnectAndLogin(
                "Haiku, The Robot",
                apSlotName.Value,
                ItemsHandlingFlags.AllItems,
                new Version(0, 4, 5),
                null,
                null,
                apPassword.Value);

            if (!APPlugin.apResult.Successful)
            {
                LoginFailure fail = (LoginFailure)APPlugin.apResult;
                foreach (string error in fail.Errors)
                {
                    apConnection.LogFatal(error);
                }
            }
            else
            {
                APPlugin.InitHooks();
                apConnection.LogInfo("Connection Success");
                APPlugin.BaseID = long.Parse(((LoginSuccessful)APPlugin.apResult).SlotData["baseID"].ToString());
                ItemMachine.UpdateID(APPlugin.BaseID);
                //ItemMachine.RunThroughQueue();
            }
        }
    }
}

