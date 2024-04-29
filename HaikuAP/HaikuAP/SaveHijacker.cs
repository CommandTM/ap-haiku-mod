using System;
using UnityEngine;

namespace HaikuAP;

public class SaveHijacker
{
    public static void Hijack()
    {
        On.PCSaveManager.Save += _saveAPThings;
    }

    private static void _saveAPThings(On.PCSaveManager.orig_Save orig, PCSaveManager self, string filepath)
    {
        orig(self, filepath);
        self.es3SaveFile.Save<String>("test", "Test");
    }
}