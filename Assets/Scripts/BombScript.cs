using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

	public Vector3 direction;
	public float thrust = 6f;
	public int bombIndex = 0;

	private Rigidbody2D _rb;
	private Collider2D _col;

	// Use this for initialization
	void Start () {
		_rb.AddForce (direction * thrust, ForceMode2D.Force);
	}

	void OnCollisionEnter2D(Collision2D coll) 
	{ 
		if (coll.gameObject.tag == "Player")
			Physics2D.IgnoreCollision (_col, coll.collider);
		
		switch (bombIndex) {
		case 0:
			Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
			Destroy (this.gameObject);
			break;
		case 1:
			// Freeze opponent

			break;
		}
	}


	public void Initialize (Vector3 direction)
	{
		this.direction = direction;
	}

	public void Initialize (Vector3 direction, int bombIndex)
	{
		this.direction = direction;
		this.bombIndex = bombIndex;
	}

	public void Initialize (Vector3 direction, float thrust)
	{
		this.direction = direction;
		this.thrust = thrust;
	}

	void Awake () {
		_rb = GetComponent<Rigidbody2D>();
		_col = GetComponent<Collider2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
