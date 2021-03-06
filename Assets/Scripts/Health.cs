﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Health : MonoBehaviour {

	private List<GameObject> hpList;

	private PlayerScript player;

	// Use this for initialization
	void Start () {

		hpList = new List<GameObject> ();
		player = GameObject.Find ("Player").GetComponent<PlayerScript> ();

		for (int i = 0; i < player.currentHP; ++i) {
			GameObject go = new GameObject ("HP" + i);
			go.transform.parent = transform;
			go.transform.position = transform.position;
			go.AddComponent<SpriteRenderer> ();
			go.GetComponent<SpriteRenderer> ().sortingLayerName = "CameraUI";
			go.GetComponent<SpriteRenderer> ().sprite = Resources.Load ("Sprites/Heart", typeof(Sprite)) as Sprite;
			go.transform.localScale = new Vector3 (0.1f, 0.1f, go.transform.localScale.z);
			go.transform.localPosition = new Vector3 (go.transform.localPosition.x + 0.6f * i, go.transform.localPosition.y, go.transform.localPosition.z);
			hpList.Add (go);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (hpList.Count != player.currentHP && hpList.Count != 0) {
			Destroy (hpList [hpList.Count - 1]);
			hpList.RemoveAt (hpList.Count - 1);
		}
	}
}
