using UnityEngine;
using System.Collections;

public class MineMushroom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player") || col.CompareTag ("Enemy")) {
			Explode ();
		}
	}

	public void Explode()
	{
		Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
		Destroy (this.gameObject);
	}
}
