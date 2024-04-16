namespace HaikuAP;

public class ItemMachine
{
    public static void GiveTenMoney()
    {
        InventoryManager.instance.AddSpareParts(10);
    }
}