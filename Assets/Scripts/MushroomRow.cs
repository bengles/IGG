using UnityEngine;
using System.Collections;

public class MushroomRow : MonoBehaviour {

	private MineMushroom[] mushrooms;

	// Use this for initialization
	void Start () {
		mushrooms = GetComponentsInChildren<MineMushroom> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
			foreach (MineMushroom m in mushrooms)
				m.Explode ();
		}
		Destroy (this.gameObject);
	}
}
