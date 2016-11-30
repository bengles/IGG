using UnityEngine;
using System.Collections;
using Prime31;

public class ParallaxBackground : MonoBehaviour {

	public float offsetMovement = 0.05f;

	private GameObject Player;


	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float velocity = Player.GetComponent<Rigidbody2D> ().velocity.x;
		if( velocity < -1f)
		{
			this.gameObject.transform.position = new Vector3( this.gameObject.transform.position.x - velocity/offsetMovement, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		}
		else if( velocity > 1f )
		{
			this.gameObject.transform.position = new Vector3( this.gameObject.transform.position.x - velocity/offsetMovement, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		}
	}
}
