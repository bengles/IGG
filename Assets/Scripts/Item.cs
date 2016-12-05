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
}

public enum ItemCategory { Bomb, Staff, Potion, Ingredient };