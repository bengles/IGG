using UnityEngine;
using System.Collections;

public class Trigger : MonoBehaviour {

	public bool triggered;

	// Use this for initialization
	void Start () {
		triggered = false;
	}

	void OnTriggerEnter2D(Collider2D col) {
		if (col.gameObject.tag == "Player")
			triggered = true;
	}
}
