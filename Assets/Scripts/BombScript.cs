using UnityEngine;
using System.Collections;

public class BombScript : MonoBehaviour {

	public Vector3 direction;
	public float thrust = 6f;
	public int bombIndex = 0;

	private Rigidbody2D _rb;
	private SpriteRenderer _sr;

	// Use this for initialization
	void Start () {
		_rb.AddForce (direction * thrust, ForceMode2D.Force);
		_rb.AddTorque (-5*thrust);
	}

	void OnTriggerEnter2D(Collider2D col)
	{
		if (!col.CompareTag ("Player") && !col.CompareTag("Trigger") && !col.CompareTag("Poison") && !col.CompareTag("PatrolObject")) {
			switch (bombIndex) {
			    case -1:
				    break;
			    case 0:
                    // Freeze opponent
                    if (col.gameObject.tag == "Enemy")
                    {
						//col.gameObject.GetComponent<MonsterAI> ().Freeze();
						Destroy (this.gameObject);
                    } else if (col.gameObject.tag == "Water") {
					    col.isTrigger = false;
					    col.gameObject.layer = 0;
					    col.transform.parent.GetComponent<WaterScript> ().Freeze();
					    Destroy (this.gameObject);
				    } else {
                        //despawn or they exist forever
                        Destroy(this.gameObject);
                    }
				break;
			case 1:
				// Mammoth Bait
				break;
			case 2:
				// Goo Bomb
				break;
			case 10:
				Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
				Destroy (this.gameObject);
				break;
			}
		}
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		if (!col.gameObject.CompareTag ("Player") && !col.gameObject.CompareTag("Trigger") && !col.gameObject.CompareTag("Poison") && !col.gameObject.CompareTag("PatrolObject")) {
			switch (bombIndex) {
			case -1:
				break;
			case 0:
				// Freeze opponent
				if (col.gameObject.tag == "Enemy") {
					Destroy (this.gameObject);
				} else if (col.gameObject.tag == "Water") {
					col.transform.parent.GetComponent<WaterScript> ().Freeze();
					Destroy (this.gameObject);
				} else {
					//despawn or they exist forever
					Destroy(this.gameObject);
				}
				break;
			case 1:
				// Mammoth Bait
				break;
			case 2:
				// Goo Bomb
				break;
			case 10:
				Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
				Destroy (this.gameObject);
				break;
			}
		}
	}

	public void Initialize (Vector3 direction, int bombIndex)
	{
		this.direction = direction;
		this.bombIndex = bombIndex;

		switch (bombIndex) {
		case -1:
			Destroy (this.gameObject);
			break;
		case 0:
			_sr.sprite = Resources.Load ("Sprites/Items/Snowball", typeof(Sprite)) as Sprite;
			this.gameObject.tag = "IceBomb";
			break;
		case 1:
			// Mammoth Bait
			_sr.sprite = Resources.Load ("Sprites/Items/Mammoth Bait", typeof(Sprite)) as Sprite;
			break;
		case 2:
			// Goo Bomb
			break;
		case 10:
			_sr.sprite = Resources.Load ("Sprites/Items/Bomb", typeof(Sprite)) as Sprite;
			break;
		}

	}

	void Awake () {
		_rb = GetComponent<Rigidbody2D>();
		_sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
