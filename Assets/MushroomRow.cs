using UnityEngine;
using System.Collections;

public class MushroomRow : MonoBehaviour {

	private MineMushroom[] mushrooms;
	private Collider2D _col;

	// Use this for initialization
	void Start () {
		mushrooms = GetComponentsInChildren<MineMushroom> ();
		_col = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D col)
	{
		Debug.Log ("Woop");
		if (col.gameObject.tag == "Player" || col.gameObject.tag == "Enemy") {
			foreach (MineMushroom m in mushrooms)
				m.Explode ();
		}
		Destroy (this.gameObject);
	}
}
