using UnityEngine;
using System.Collections.Generic;

public class GlobalData : Singleton<GlobalData> {

	protected GlobalData () {} // guarantee this will be always a singleton only - can't use the constructor!

	public static GlobalData i;

	public List<Item> currentInventory;
	public int currentLevel;
	public int nrPotionSlots;
	public int nrBombSlots;
	public int nrStaffSlots;

	public List<Item> equippedBombs;
	public List<Item> equippedStaffs;
	public List<Item> equippedPotions;

	private AudioSource audio;

	void Awake () {
		// Your initialization code here

		audio = this.gameObject.AddComponent<AudioSource> ();
		audio.clip = (Resources.Load ("Audio/Level/tutorialmap") as AudioClip);
		audio.volume = 0.1f;
		audio.Play ();
		audio.loop = true;

		this.Reload ();

		currentInventory = new List<Item>();
        equippedBombs = new List<Item>();
        equippedStaffs = new List<Item>();
        equippedPotions = new List<Item>();
		currentLevel = 3;
		nrPotionSlots = 1;
		nrBombSlots = 1;
		nrStaffSlots = 1;
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
        currentInventory.Add(new Item("BoneAxe",
            "blahablohoblaha",
            0, ItemCategory.Staff)); //add BoneStaff
        currentInventory.Add(new Item(0, ItemCategory.Potion));
        currentInventory.Add(new Item(1, ItemCategory.Potion));
        currentInventory.Add(new Item(2, ItemCategory.Potion));
        currentInventory.Add(new Item(3, ItemCategory.Potion));
        currentInventory.Add(new Item(4, ItemCategory.Potion));
        currentInventory.Add(new Item(5, ItemCategory.Potion));
    }
}