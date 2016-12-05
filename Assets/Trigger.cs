using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool triggered;

	private Collider2D _col;

	// Use this for initialization
	void Start () {
		triggered = false;
		_col = GetComponent<Collider2D> ();
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player")
			triggered = true;
	}
}
