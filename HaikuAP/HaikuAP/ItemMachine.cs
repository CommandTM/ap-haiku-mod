using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;

namespace HaikuAP;

public class ItemMachine
{
    public static ManualLogSource Logging = new ManualLogSource("Item Machine");
    private static long _baseID = 0;
    
    public static void RunThroughQueue()
    {
        while (APPlugin.apSession.Items.Any())
        {
            _processItem(APPlugin.apSession.Items.PeekItem().Item);
            APPlugin.apSession.Items.DequeueItem();
        }
    }

    public static void UpdateID(long val)
    {
        _baseID = val;
    }

    private static void _processItem(long id)
    {
        Logging.LogInfo(APPlugin.apSession.Items.GetItemName(id));
        if (!_abilityHandle(id))
        {
            if (!_powerCellHandle(id))
            {
                
            }
        }
    }

    private static bool _abilityHandle(long id)
    {
        if (id == _baseID)
        {
            GameManager.instance.canWallJump = true;
            return true;
        }

        if (id == _baseID + 1)
        {
            GameManager.instance.canRoll = true;
            return true;
        }

        if (id == _baseID + 5)
        {
            GameManager.instance.canDoubleJump = true;
            return true;
        }

        if (id == _baseID + 6)
        {
            GameManager.instance.canGrapple = true;
            return true;
        }

        if (id == _baseID + 7)
        {
            GameManager.instance.canTeleport = true;
            return true;
        }

        if (id == _baseID + 8)
        {
            GameManager.instance.canBomb = true;
            return true;
        }

        return false;
    }

    private static bool _powerCellHandle(long id)
    {
        if (id != _baseID + 61) return false;
        for (int i = 0; i < GameManager.instance.powerCells.Length; i++)
        {
            if (!GameManager.instance.powerCells[i].collected)
            {
                GameManager.instance.powerCells[i].collected = true;
                return true;
            }
        }
        return false;
    }
}