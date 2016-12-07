using UnityEngine;
using System.Collections;

public class Item {


	public string name;
	public string flavorText;
	public int index;
	public ItemCategory cat;

	public Item (string name, string flavorText, int index, ItemCategory cat) {
		this.name = name;
		this.flavorText = flavorText;
		this.index = index;
		this.cat = cat;
	}

    public Item(int index, ItemCategory cat)
    {
        this.index = index;
        this.cat = cat;
        if (cat == ItemCategory.Staff)
        {
            switch (index)
            {
                case -1:
                    this.name = "Staff";
                    this.flavorText = "The staff is the witch’s preferred weapon in close combat. Though weak in itself, dipping it in the right brews can give it peculiar properties.";
                    break;
                case 0:
                    this.name = "Bone Axe";
                    this.flavorText = "A primitive but deadly tool, effective against primitive and deadly foes, like trolls.";
                    break;
                case 1:
                    this.name = "Shrink Ray";
                    this.flavorText = "A weapon that miniaturizes other creatures. Do not use on children.";
                    break;
            }
        }
        if (cat == ItemCategory.Bomb)
        {
            switch (index)
            {
                case -1:
                    this.name = "Bomb";
                    this.flavorText = "The bomb case is easy to throw in the direction of an arc, and its contents usually react when it touches the environment or other creatures.";
                    break;
                case 0:
                    this.name = "Snowball";
                    this.flavorText = "A magic snowball that freezes creatures. Using it in snowball fights is considered cheating.";
                    break;
                case 1:
                    this.name = "Mammoth Bait";
                    this.flavorText = "Once used to attract mammoths, but by now they’ve all gone to heaven...";
                    break;
                case 2:
                    this.name = "Goo Bomb";
                    this.flavorText = "Simply a bomb covered in sticky goo that attaches to certain surfaces.";
                    break;
            }
        }
        if (cat == ItemCategory.Potion)
        {
            switch (index)
            {
                case -1:
                    this.name = "Potion";
                    this.flavorText = "Before she can read, the first thing the witch learns is how to use the cauldron to brew the simplest of potions. Potions transform Willow in many kind of ways.";
                    break;
                case 0:
                    this.name = "Chilled Drink";
                    this.flavorText = "Just a cold drink for a warm summer day. Drinking too fast might cause brain freeze.";
                    break;
                case 1:
                    this.name = "Curry Sauce";
                    this.flavorText = "Delicious curry. If you consume it you won’t be able to stop!";
                    break;
                case 2:
                    this.name = "Pixifier";
                    this.flavorText = "A drink that will make you the size of a pixie. Unfortunately, it doesn’t let you grow wings.";
                    break;
                case 3:
                    this.name = "Peppermint Tea";
                    this.flavorText = "A delicious cup of tea is all a witch needs to restore her spirits.";
                    break;
				case 4:
					this.name = "Sniffle Enhancer";
					this.flavorText = "Just gives you a bad case of the sniffles.";
					break;
                case 5:
                    this.name = "Mushroom Proof Lotion";
                    this.flavorText = "This lotion protects witches from the damage of exploding mushrooms, but not the impact.";
                    break;
            }
        }

        if (cat == ItemCategory.Ingredient)
        {
            switch (index)
            {
                case 3:
                    this.name = "Winter Mint";
                    this.flavorText = "A refreshing herb that tastes like the Arctic winds.";
                    break;
                case 4:
                    this.name = "Mammoth Bone";
                    this.flavorText = "Remains from an extinct beast known for its massive size.";
                    break;
                case 5:
                    this.name = "Hot Spices";
                    this.flavorText = "A mixture of spices so hot a dragon won’t touch it.";
                    break;
                case 6:
                    this.name = "Pixie Dust";
                    this.flavorText = "A dust used for shrinking things. Made by grinding pixie cocoons.";
                    break;
                case 7:
                    this.name = "Sniffle Snot";
                    this.flavorText = "Disgusting mucus from a troll who caught a nasty cold.";
                    break;
                case 8:
                    this.name = "Familiar Mushroom";
                    this.flavorText = "A toadstool that makes things… wait a minute, this looks familiar…";
                    break;
            }
        }
    }
}

public enum ItemCategory { Bomb, Staff, Potion, Ingredient };