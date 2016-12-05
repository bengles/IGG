using UnityEngine;
using System.Collections;

public class FrostScript : MonoBehaviour {

	private GameObject parent;

	// Use this for initialization
	void Start () {
		parent = transform.parent.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = parent.transform.position;
	}
}
