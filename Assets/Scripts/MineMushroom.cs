using UnityEngine;
using System.Collections;

public class MineMushroom : MonoBehaviour {


	private Collider2D _col;

	// Use this for initialization
	void Start () {
		_col = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D coll)
	{
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemy")
		{
			Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
			Destroy (this.gameObject);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{ 
		if (coll.gameObject.tag == "Player" || coll.gameObject.tag == "Enemy")
		{
			Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
			Destroy (this.gameObject);
		}
	}
}
