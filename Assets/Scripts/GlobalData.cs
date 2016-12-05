using UnityEngine;
using System.Collections.Generic;

public static class GlobalData
{

    public static List<ItemInfo> currentInventory;
    public static int equippedStaffID;
    public static int equippedBombID;
    public static int equippedPotionID;

    public static void init()
    {
        currentInventory = new List<ItemInfo>();
        currentInventory.Add(new ItemInfo(4)); //add WinterMint
        currentInventory.Add(new ItemInfo(5)); //add MammothBone

        equippedStaffID = 1;
        equippedBombID = 2;
        equippedPotionID = 3;
    }
}

public class ItemInfo
{
    private string itemName;
    private string flavorText;
    private int itemID;
    //dmg, etc?

    public ItemInfo(int id)
    {
        itemID = id;
        switch (id)
        {
            case 4:
                itemName = "Winter Mint";
                flavorText = "";
                break;
            case 5:
                itemName = "Mammoth Bone";
                break;
            default:
                Debug.Log("ItemInfo got bogus id?");
                break;
        }
    }

    public string getName()
    {
        return itemName;
    }

    public int getID()
    {
        return itemID;
    }
}