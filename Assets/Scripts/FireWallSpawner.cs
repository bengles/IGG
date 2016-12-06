using UnityEngine;
using System.Collections;

public class FireWallSpawner : MonoBehaviour {

	public float speed = 0.125f;
	public int direction = -1;
	public float travelDistance = 10f;
	public float spawnFrequency = 0.5f;

	private float spawnTimer = 0.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		spawnTimer += Time.deltaTime;
		if (spawnTimer >= spawnFrequency) {
			SpawnFireWall ();
			spawnTimer = 0.0f;
		}
	}
		
	void SpawnFireWall () {
		GameObject go = Object.Instantiate (Resources.Load ("Prefabs/Fire Wall"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		FireWall firewall  = go.GetComponent<FireWall> ();
		firewall.Initialize (speed, direction, travelDistance);
		firewall.transform.parent = transform;
	}
}
