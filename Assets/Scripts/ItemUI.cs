using UnityEngine;
using System.Collections;

public class ItemUI : MonoBehaviour {

	private GameObject bombSlot;
	private GameObject potionSlot;
	private GameObject stickSlot;
	private PlayerScript player;


	// Use this for initialization
	void Start () {
		bombSlot = transform.FindChild ("BombSlot").gameObject;
		potionSlot = transform.FindChild ("PotionSlot").gameObject;
		stickSlot = transform.FindChild ("StickSlot").gameObject;

		player = GameObject.Find ("Player").GetComponent<PlayerScript>();

		GlobalData.Instance.currentInventory.Add (new Item(0, ItemCategory.Bomb));
		GlobalData.Instance.currentInventory.Add (new Item(0, ItemCategory.Potion));

	}
	
	// Update is called once per frame
	void Update () {
		foreach (Item item in GlobalData.Instance.currentInventory) {
			if (item != null) {
				switch (item.cat) {
				case ItemCategory.Bomb:
					if (item.index == player.bombIndex) {
						bombSlot.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Items/" + item.name, typeof(Sprite)) as Sprite;
					}
					break;
				case ItemCategory.Potion:
					if (item.index == player.potionIndex)
						potionSlot.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Items/" + item.name, typeof(Sprite)) as Sprite;
					break;
				case ItemCategory.Staff:
					if (item.index == player.staffIndex)
						stickSlot.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Items/" + item.name, typeof(Sprite)) as Sprite;
					break;
				}
			}
		}
	}
}
