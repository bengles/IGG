using UnityEngine;
using System.Collections;

public class PoisonCloud : MonoBehaviour {

	private SpriteRenderer _sr1;
	private SpriteRenderer _sr2;
	private Collider2D _col;

	public float cloudSpeed = 0.01f;
	public float interval = 0.5f;

	private int direction1;
	private int direction2;

	// Use this for initialization
	void Start () {

	}


	void Awake () {
		_sr1 = transform.FindChild ("Sprite 1").GetComponent<SpriteRenderer>();
		_sr2 = transform.FindChild ("Sprite 2").GetComponent<SpriteRenderer>();
	}

	// Update is called once per frame
	void Update () {

		if (_sr1.transform.localPosition.x >= interval) 
			direction1 = -1;
		else if (_sr1.transform.localPosition.x <= -interval)
			direction1 = 1;

		if (_sr2.transform.localPosition.x >= interval) 
			direction2 = -1;
		else if (_sr2.transform.localPosition.x <= -interval)
			direction2 = 1;

		_sr1.transform.localPosition = new Vector2 (_sr1.transform.localPosition.x + cloudSpeed * direction1, _sr1.transform.localPosition.y);
		_sr2.transform.localPosition = new Vector2 (_sr2.transform.localPosition.x + cloudSpeed * direction2, _sr2.transform.localPosition.y);

	}
}
