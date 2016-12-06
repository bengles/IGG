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

	// Use this for initialization
	void Start () {
		_sr = GetComponent<SpriteRenderer> ();
		sprites = Resources.LoadAll<Sprite> ("Sprites/exp3_0");

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
		foreach (Collider2D mm in Physics2D.OverlapCircleAll (transform.position, 10)) {
			if (mm.CompareTag("Mushroom") && mm.gameObject.GetComponent<MineMushroom>() != null)
				mm.gameObject.GetComponent<MineMushroom>().Explode ();
		}
	}

}