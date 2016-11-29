using UnityEngine;
using System.Collections;

public class ParallaxBackground : MonoBehaviour {

	public float offsetMovement = 0.05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if( Input.GetAxis( "Horizontal" ) < 0 )
		{
			this.gameObject.transform.position = new Vector3( this.gameObject.transform.position.x + offsetMovement, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		}
		else if( Input.GetAxis( "Horizontal" ) > 0 )
		{
			this.gameObject.transform.position = new Vector3( this.gameObject.transform.position.x - offsetMovement, this.gameObject.transform.position.y, this.gameObject.transform.position.z);
		}
	}
}
