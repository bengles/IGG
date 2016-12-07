using UnityEngine;
using System.Collections.Generic;

public class GlobalData : Singleton<GlobalData> {

	protected GlobalData () {} // guarantee this will be always a singleton only - can't use the constructor!

	public static GlobalData i;

	public List<Item> currentInventory;
	public static int equippedBombIndex;
	public static int equippedStaffIndex;
	public static int equippedPotionIndex;

	void Awake () {
		// Your initialization code here

		this.Reload ();

		currentInventory = new List<Item>();

		equippedPotionIndex = -1;
		equippedBombIndex = -1;
		equippedStaffIndex = 0;

		// Add base items
//		currentInventory.Add(new Item( "Potion", 
//			"Before she can read, the first thing the witch learns is how to use the " +
//			"cauldron to brew the simplest of potions. Potions transform Willow in many kind of ways.", 
//			equippedPotionIndex, ItemCategory.Potion)); //add Potion
//		currentInventory.Add(new Item( "Bomb",
//			"The bomb case is easy to throw in the direction of an arc, and its contents " + 
//			"usually react when it touches the environment or other creatures.",
//			equippedBombIndex, ItemCategory.Bomb)); //add Bomb
//		currentInventory.Add(new Item( "Staff",
//			"The staff is the witch’s preferred weapon in close combat. Though weak in itself, " +
//			"dipping it in the right brews can give it peculiar properties.",
//			equippedStaffIndex, ItemCategory.Staff)); //add Staff
	}

    public bool HasItemID(int itemID, ItemCategory cat)
    {
        foreach(Item item in currentInventory)
        {
            if (item.cat == cat && item.index == itemID)
            {
                return true;
            }
        }
        return false;
    }

    public List<Item> GetIngredients()
    {
        List<Item> ingredientList = new List<Item>();
        foreach(Item item in currentInventory)
        {
            if (item.cat == ItemCategory.Ingredient)
            {
                ingredientList.Add(item);
            }
        }
        return ingredientList;
    }

    public void AddTestItems()
    {
        currentInventory.Add(new Item("WinterMint",
            "Blabla ha blablahahabla", 3, ItemCategory.Ingredient));
    }
}