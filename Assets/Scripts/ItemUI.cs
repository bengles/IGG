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

	}
	
	// Update is called once per frame
	void Update () {
		foreach (Item item in GlobalData.Instance.equippedBombs) 
			if (item != null) 
				if (item.index == player.bombIndex) 
					bombSlot.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Items/" + item.name, typeof(Sprite)) as Sprite;
				
		foreach (Item item in GlobalData.Instance.equippedPotions) 
			if (item != null) 
				if (item.index == player.potionIndex)
					potionSlot.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Items/" + item.name, typeof(Sprite)) as Sprite;
		
		foreach (Item item in GlobalData.Instance.equippedStaffs) 
			if (item != null) 
				if (item.index == player.staffIndex)
					stickSlot.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Items/" + item.name, typeof(Sprite)) as Sprite;
	}
}
