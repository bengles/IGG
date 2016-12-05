﻿using UnityEngine;
using System.Collections;

public class FireWallScript : MonoBehaviour {

	public float speed = 0.05f;
	public int direction = -1;

	private GameObject fireWall;
	private Trigger trigger;

	// Use this for initialization
	void Start () {
		fireWall = transform.FindChild ("Fire Wall").gameObject as GameObject;
		trigger = transform.FindChild ("Trigger").GetComponent<Trigger>();
	}
	
	// Update is called once per frame
	void Update () {
		if (trigger.triggered)
			transform.position = new Vector2 (transform.position.x + direction * speed, transform.position.y);
	}
}
