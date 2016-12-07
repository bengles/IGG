using UnityEngine;
using System.Collections;

public class SpiderTroll : MonoBehaviour {

	private Animator _animator;

	// Use this for initialization
	void Start () {
		_animator = GetComponent<Animator> ();

	}
	
	void OnTriggerEnter2D (Collider2D col) {
		Debug.Log ("STAND");
		if (col.gameObject.CompareTag ("Player"))
			_animator.Play (Animator.StringToHash("Spider_Troll_Stand"));
	}
}
