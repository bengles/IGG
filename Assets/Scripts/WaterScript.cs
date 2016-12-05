using UnityEngine;
using System.Collections;

public class WaterScript : MonoBehaviour {

	private GameObject liquidWater;
	private GameObject iceWater;

	// Use this for initialization
	void Start () {
		liquidWater = transform.FindChild ("Liquid Water").gameObject;
		iceWater = transform.FindChild ("Ice Water").gameObject;

		liquidWater.SetActive (true);
		iceWater.SetActive (false);
	}

	public void Freeze() {
		liquidWater.SetActive (false);
		iceWater.SetActive (true);
	}
		
}
