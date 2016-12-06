using UnityEngine;
using System.Collections;

public class Anim : MonoBehaviour {

	public bool loop;
	public string location;

	private SpriteRenderer _sr;
	private Sprite [] sprites;
	private int frame = 0;
	private float deltaTime = 0;
	private float frameSeconds = 0.05f;

	private Collider2D _col;

	// Use this for initialization
	void Start () {
		_sr = GetComponent<SpriteRenderer> ();
		sprites = Resources.LoadAll<Sprite> ("Sprites/exp3_0");

		_col = GetComponent<Collider2D> ();
	}

	public void Initialize (bool loop, string location)
	{
		this.loop = loop;
		this.location = location;
	}
	
	// Update is called once per frame
	void Update () {
		deltaTime += Time.deltaTime;

		while (deltaTime >= frameSeconds) {
			deltaTime -= frameSeconds;
			frame++;
			if(loop)
				frame %= sprites.Length;
			//Max limit
			else if(frame >= sprites.Length) {
				frame = sprites.Length - 1;
				Destroy (this.gameObject);
			}
		}

		//Animate sprite with selected frame
		_sr.sprite = sprites [frame];
	}

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Mushroom"))
			col.gameObject.GetComponent<MineMushroom> ().Explode ();
		else if (col.CompareTag ("Player") || col.CompareTag ("Enemy")) {
			Vector3 direction = col.gameObject.transform.position - transform.position;
			_col.gameObject.GetComponent<Rigidbody2D> ().velocity = 50f * direction.normalized;
		}
	}

}
