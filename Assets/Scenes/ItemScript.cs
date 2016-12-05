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

	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player")
			Destroy (this.gameObject);
	}
}
