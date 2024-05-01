using System.Collections;
using BepInEx.Logging;
using UnityEngine;

namespace HaikuAP;

public class LocationMachine
{
    public static ManualLogSource Logging = new ManualLogSource("Location Machine");
    private static long _baseID = 0;

    public static void Init()
    {
        On.UnlockTutorial.PowerUpSequence += _apVerUnlockTutorial;
    }
    
    public static void UpdateID(long val)
    {
        _baseID = val;
    }

    private static IEnumerator _apVerUnlockTutorial(On.UnlockTutorial.orig_PowerUpSequence orig, UnlockTutorial self)
    {
        yield return new WaitForSeconds(0.15f);
        self.PlayPowerUpSFX();
        CameraBehavior.instance.Shake(4f, 0.1f);
        yield return new WaitForSeconds(4f);
        self.PlayPowerUpFinish();
        CameraBehavior.instance.Shake(0.2f, 0.2f);
        if (self.bomb)
        {
            _sendLocation(8);
        }
        if (self.roll)
        {
            _sendLocation(1);
        }
        if (self.grapple)
        {
            _sendLocation(6);
        }
        if (self.doublejump)
        {
            _sendLocation(5);
        }
        if (self.walljump)
        {
            _sendLocation(0);
        }
        if (self.teleport)
        {
            _sendLocation(7);
        }
        yield return new WaitForSeconds(1f);
        if (!GameManager.instance.corruptMode)
        {
            PlayerHealth.instance.ReplenishHealth();
        }
        ManaManager.instance.RemoveHeat(271f);
        self.UnfreezePlayer();
        yield break;
    }

    private static void _sendLocation(long id)
    {
        Logging.LogInfo(APPlugin.apSession.Locations.GetLocationNameFromId(_baseID + id));
        APPlugin.apSession.Locations.CompleteLocationChecks(_baseID + id);
    }
}