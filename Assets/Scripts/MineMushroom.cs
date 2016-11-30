using UnityEngine;
using System.Collections;

public class MineMushroom : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void Explode()
	{
		Object anim = Object.Instantiate (Resources.Load ("Prefabs/Explosion"), transform.position, Quaternion.identity) as GameObject;
		Destroy (this.gameObject);
	}
}
