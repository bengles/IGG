using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

	public Vector3 direction;
	public float thrust = 3;

	private Rigidbody2D _rb;

	// Use this for initialization
	void Start () {
		_rb.AddRelativeForce (direction * thrust, ForceMode2D.Impulse);

	}

	void OnCollisionEnter2D(Collision2D coll) 
	{ 
		Object anim = Object.Instantiate (Resources.Load("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
		Destroy (this.gameObject);
	}


	public void Initialize (Vector3 direction)
	{
		this.direction = direction;
	}

	public void Initialize (Vector3 direction, float thrust)
	{
		this.direction = direction;
		this.thrust = thrust;
	}

	void Awake () {
		_rb = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
