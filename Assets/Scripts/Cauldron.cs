using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cauldron : MonoBehaviour {

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player")) {
			if (col.gameObject.GetComponent<PlayerScript>().GetVelocity().y < 0f)
				SceneManager.LoadScene (2);
		}
	}

}
