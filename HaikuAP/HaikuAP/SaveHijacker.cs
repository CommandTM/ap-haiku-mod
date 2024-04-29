﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace HaikuAP;

public class SaveHijacker
{
    public static List<long> ProcessedItems = new List<long>();
    
    public static void Hijack()
    {
        On.PCSaveManager.Save += _saveAPThings;
        On.PCSaveManager.Load += _loadAPThings;
    }

    private static void _saveAPThings(On.PCSaveManager.orig_Save orig, PCSaveManager self, string filepath)
    {
        orig(self, filepath);
        self.es3SaveFile.Save("processedItems", ProcessedItems);
        self.es3SaveFile.Sync();
    }
    
    private static void _loadAPThings(On.PCSaveManager.orig_Load orig, PCSaveManager self, string filepath)
    {
        orig(self, filepath);
        ProcessedItems = self.es3SaveFile.Load<List<long>>("processedItems", new List<long>());
        ItemMachine.UpdateProcessedItems(ProcessedItems);
    }
}