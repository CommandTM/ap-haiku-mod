using System;
using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using UnityEngine;

namespace HaikuAP;

public class SaveHijacker
{
    public static List<NetworkItem> ProcessedItems = new List<NetworkItem>();
    public static List<int> SentPowerCells = new List<int>();
    public static bool FirstShopHFSent = false;
    
    public static void Hijack()
    {
        On.PCSaveManager.Save += _saveAPThings;
        On.PCSaveManager.Load += _loadAPThings;
    }

    public static bool CollectedPowerCell(int saveID)
    {
        return SentPowerCells.Contains(saveID);
    }

    private static void _saveAPThings(On.PCSaveManager.orig_Save orig, PCSaveManager self, string filepath)
    {
        orig(self, filepath);
        self.es3SaveFile.Save("processedItems", ProcessedItems);
        self.es3SaveFile.Save("sentCells", SentPowerCells);
        self.es3SaveFile.Save("firstShopHFSent", FirstShopHFSent);
        self.es3SaveFile.Sync();
    }
    
    private static void _loadAPThings(On.PCSaveManager.orig_Load orig, PCSaveManager self, string filepath)
    {
        orig(self, filepath);
        ProcessedItems = self.es3SaveFile.Load("processedItems", new List<NetworkItem>());
        SentPowerCells = self.es3SaveFile.Load("sentCells", new List<int>());
        FirstShopHFSent = self.es3SaveFile.Load("firstShopHFSent", false);
        ItemMachine.UpdateProcessedItems(ProcessedItems);
    }
}