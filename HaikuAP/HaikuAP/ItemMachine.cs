using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;

namespace HaikuAP;

public class ItemMachine
{
    public static ManualLogSource Logging = new ManualLogSource("Item Machine");
    private static List<NetworkItem> _processedItems = new List<NetworkItem>();
    private static long _baseID = 0;
    
    public static void RunThroughQueue()
    {
        while (APPlugin.apSession.Items.Any())
        {
            if (_processedItems.Contains(APPlugin.apSession.Items.PeekItem()))
            {
                APPlugin.apSession.Items.DequeueItem();
                _processedItems.Remove(APPlugin.apSession.Items.PeekItem());
                continue;
            }
            _processItem(APPlugin.apSession.Items.PeekItem().Item);
            SaveHijacker.ProcessedItems.Add(APPlugin.apSession.Items.PeekItem());
            APPlugin.apSession.Items.DequeueItem();
        }
    }

    public static void UpdateID(long val)
    {
        _baseID = val;
    }

    public static void UpdateProcessedItems(List<NetworkItem> items)
    {
        _processedItems.AddRange(items);
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
        id -= _baseID;
        if (id == 0)
        {
            GameManager.instance.canWallJump = true;
            return true;
        }

        if (id == 1)
        {
            GameManager.instance.canRoll = true;
            return true;
        }

        if (id == 2)
        {
            GameManager.instance.lightBulb = true;
            return true;
        }

        if (id == 3)
        {
            GameManager.instance.fireRes = true;
            return true;
        }

        if (id == 4)
        {
            GameManager.instance.waterRes = true;
            return true;
        }

        if (id == 5)
        {
            GameManager.instance.canDoubleJump = true;
            return true;
        }

        if (id == 6)
        {
            GameManager.instance.canGrapple = true;
            return true;
        }

        if (id == 7)
        {
            GameManager.instance.canTeleport = true;
            return true;
        }

        if (id == 8)
        {
            GameManager.instance.canBomb = true;
            return true;
        }

        if (id == 9)
        {
            GameManager.instance.canHeal = true;
            return true;
        }

        return false;
    }

    private static bool _chipHandle(long id)
    {
        id -= _baseID;
        if (10 <= id && id <= 37)
        {
            
        }
        else
        {
            return false;
        }
    }

    private static bool _powerCellHandle(long id)
    {
        if (id != _baseID + 38) return false;
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