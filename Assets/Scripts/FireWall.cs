using UnityEngine;
using System.Collections;
using System;

public class FireWall : MonoBehaviour {

	public float speed = 0.05f;
	public int direction = -1;
	public float travelDistance = 10f;


	public void Initialize(float speed, int direction, float travelDistance) {
		this.speed = speed;
		this.direction = direction;
		this.travelDistance = travelDistance;
	}

	// Update is called once per frame
	void Update () {
		transform.position = new Vector2 (transform.position.x + direction * speed, transform.position.y);

		if (Math.Abs (transform.localPosition.x) > travelDistance)
			Destroy (this.gameObject);

	}
}
