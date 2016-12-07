using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {

	private GameObject liquidWater;
	private GameObject iceWater;

	private Collider2D col;

	// Use this for initialization
	void Start () {
		liquidWater = transform.FindChild ("Liquid Water").gameObject;
		iceWater = transform.FindChild ("Freezed Water").gameObject;

		col = transform.FindChild ("Collision").GetComponent<Collider2D>();

		liquidWater.SetActive (true);
		iceWater.SetActive (false);
	}

	public void Freeze() {
		col.isTrigger = false;
		gameObject.layer = 0;
		liquidWater.SetActive (false);
		iceWater.SetActive (true);
	}
}