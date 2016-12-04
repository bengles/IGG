using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

	public Vector3 direction;
	public float thrust = 6f;
	public int bombIndex = 0;

	private Rigidbody2D _rb;
	private Collider2D _col;
	private SpriteRenderer _sr;

	// Use this for initialization
	void Start () {
		_rb.AddForce (direction * thrust, ForceMode2D.Force);
		_rb.AddTorque (-5*thrust);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.CompareTag ("Player")) {
			switch (bombIndex) {
			    case -1:
				    break;
			    case 0:
				    Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
				    Destroy (this.gameObject);
				    break;
			    case 1:
                    // Freeze opponent
                    if (col.gameObject.tag == "Enemy")
                    {
                        Debug.Log("SUCCESS");
                    } else if (col.gameObject.tag == "Water") {
					    col.isTrigger = false;
					    col.gameObject.layer = 0;
					    col.transform.parent.GetComponent<WaterScript> ().Freeze();
					    Debug.Log ("IT IS FROZEN?!");
					    Destroy (this.gameObject);
				    } else {
                        //despawn or they exist forever
                        Destroy(this.gameObject);
                    }
				break;
			}
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

		switch (bombIndex) {
		case 0:
			_sr.sprite = Resources.Load ("Sprites/bomb", typeof(Sprite)) as Sprite;
			break;
		case 1:
			_sr.sprite = Resources.Load ("Sprites/iceBomb", typeof(Sprite)) as Sprite;
			break;
		}

	}

	public void Initialize (Vector3 direction, float thrust)
	{
		this.direction = direction;
		this.thrust = thrust;
	}

	void Awake () {
		_rb = GetComponent<Rigidbody2D>();
		_col = GetComponent<Collider2D> ();
		_sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
