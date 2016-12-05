using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	public Item item;

	public string name;
	public string flavorText;
	public int index;
	public ItemCategory cat;

	void Awake() {
		item = new Item (name, flavorText, index, cat);
		foreach (Item i in GlobalData.Instance.currentInventory)
			if (i.name == name)
				Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player")
			Destroy (this.gameObject);
	}
}
