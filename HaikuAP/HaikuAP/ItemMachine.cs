using System.Collections.Generic;
using Archipelago.MultiClient.Net.Models;
using BepInEx.Logging;
using Rewired;

namespace HaikuAP;

public class ItemMachine
{
    public static ManualLogSource Logging = new ManualLogSource("Item Machine");
    private static List<long> _processedItems = new List<long>();
    private static long _baseID = 0;
    
    public static void RunThroughQueue()
    {
        while (APPlugin.apSession.Items.Any())
        {
            if (_processedItems.Contains(APPlugin.apSession.Items.PeekItem().ItemId))
            {
                _processedItems.Remove(APPlugin.apSession.Items.PeekItem().ItemId);
                APPlugin.apSession.Items.DequeueItem();
                continue;
            }
            _processItem(APPlugin.apSession.Items.PeekItem().ItemId);
            SaveHijacker.ProcessedItems.Add(APPlugin.apSession.Items.PeekItem().ItemId);
            APPlugin.apSession.Items.DequeueItem();
        }
    }

    public static void UpdateID(long val)
    {
        _baseID = val;
    }

    public static void UpdateProcessedItems(List<long> items)
    {
        _processedItems.AddRange(items);
    }

    private static void _processItem(long id)
    {
        Logging.LogInfo(APPlugin.apSession.Items.GetItemName(id));
        if (!_abilityHandle(id))
        {
            if (!_chipHandle(id))
            {
                if (!_powerCellHandle(id))
                {
                    if (!_inventoryItemHandle(id))
                    {
                        if (!_coolantHandle(id))
                        {
                            if (!_chipSlotHandle(id))
                            {
                                _handleJunk(id);
                            }
                        }
                    }
                }
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
            InventoryManager.instance.AddItem(5);
            return true;
        }

        return false;
    }

    private static bool _chipHandle(long id)
    {
        id -= _baseID;
        if (10 <= id && id <= 37)
        {
            GameManager.instance.chip[GameManager.instance.getChipNumber(IDTranslate.APIDToChipIdent[id])].collected = true;
            return true;
        }
        return false;
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

    private static bool _inventoryItemHandle(long id)
    {
        id -= _baseID;
        if (39 <= id && id <= 45)
        {
            InventoryManager.instance.AddItem(IDTranslate.APIDToItemID[id]);
            return true;
        }
        return false;
    }

    private static bool _coolantHandle(long id)
    {
        if (id != _baseID + 46) return false;
        GameManager.instance.coolingPoints++;
        return true;
    }

    private static bool _chipSlotHandle(long id)
    {
        id -= _baseID;
        if (47 <= id && id <= 49)
        {
            string color = "";
            switch (id)
            {
                case 47:
                    color = "red";
                    break;
                case 48:
                    color = "blue";
                    break;
                case 49:
                    color = "green";
                    break;
            }

            for (int i = 0; i < GameManager.instance.chipSlot.Length; i++)
            {
                if (GameManager.instance.chipSlot[i].collected) continue;
                if (GameManager.instance.chipSlot[i].chipSlotColor.Equals(color))
                {
                    GameManager.instance.chipSlot[i].collected = true;
                    return true;
                }
            }
        }
        return false;
    }

    private static void _handleJunk(long id)
    {
        id -= _baseID;
        switch (id)
        {
            case 50:
                InventoryManager.instance.AddSpareParts(10);
                break;
            case 51:
                InventoryManager.instance.AddSpareParts(50);
                break;
            case 52:
                InventoryManager.instance.AddSpareParts(100);
                break;
        }
    }
}