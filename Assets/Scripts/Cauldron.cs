using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Cauldron : MonoBehaviour {

	public int levelReturnIndex = 3;

	void OnTriggerEnter2D (Collider2D col) {
		if (col.CompareTag ("Player")) {
			GlobalData.Instance.currentLevel = levelReturnIndex;
			if (col.gameObject.GetComponent<PlayerScript>().GetVelocity().y < 0f)
				SceneManager.LoadScene (2);
		}
	}

}
