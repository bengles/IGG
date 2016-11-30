using UnityEngine;
using System.Collections;

public class ItemScript : MonoBehaviour {

	private Collider2D _col;

	// Use this for initialization
	void Start () {
		_col = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player")
			Destroy (this.gameObject);
	}
}