using UnityEngine;
using System.Collections;

public class TestCharacterController : MonoBehaviour {


	public int bombIndex = 0;
	public int potionIndex = 0;

	private bool isGrounded;
	private bool facingRight;
	private bool isDead;
	private bool isFrozen;
	private bool potionActivated = false;
	private float potionDuration;
	private float potionTime;


	private float velocityMultiplier = 1.0f;

	private Rigidbody2D _rb;
	private Animator _anim;
	private SpriteRenderer _sr;

	// Use this for initialization
	void Start () {
		_rb = GetComponent<Rigidbody2D> ();
		_anim = GetComponent<Animator> ();
		_sr = GetComponent<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (_rb.velocity.y == 0)
			isGrounded = true;
		else
			isGrounded = false;

		float horizontal = Input.GetAxis ("Horizontal");

		Debug.Log (horizontal);

		if (horizontal > 0.5) {
			facingRight = true;
			_sr.flipX = false;
			_rb.velocity = new Vector2 (5f * horizontal * velocityMultiplier, _rb.velocity.y);
			if (isGrounded)
				_anim.Play (Animator.StringToHash ("Witch_Run"));
		} else if (horizontal < -0.5) {
			facingRight = false;
			_sr.flipX = true;
			_rb.velocity = new Vector2 (5f * horizontal * velocityMultiplier, _rb.velocity.y);
			if (isGrounded)
				_anim.Play (Animator.StringToHash ("Witch_Run"));
		} else {
			if (isGrounded)
				_anim.Play (Animator.StringToHash ("Witch_Idle"));
		}

		if (Input.GetButtonDown ("Jump") && isGrounded) {
			Debug.Log ("Jymping");
			_rb.AddForce (new Vector2 (0.0f, 10.0f), ForceMode2D.Impulse);
		}

		if (Input.GetButtonDown ("B") && !isDead && !isFrozen) {
			print ("B pressed");
			StickHit ();
		}

		if (Input.GetButtonUp ("B") && !isDead && !isFrozen) {
			StickWithdraw ();
		}

		if (Input.GetButtonDown ("X") && !isDead && !isFrozen) {
			ThrowBomb ();
		}

		if (Input.GetButtonDown ("Y") && !isDead && !isFrozen) {
			ActivatePotion ();
		}

		if (Input.GetButtonDown ("Fire2") && !isDead && !isFrozen && !potionActivated) {
			potionIndex = (potionIndex + 1) % 3;
		}

		if (Input.GetButtonDown ("Fire3") && !isDead && !isFrozen) {
			bombIndex = (bombIndex + 1) % 2;
		}


	}

	private void ThrowBomb ()
	{

		if (facingRight == true) {
			GameObject bomb = Object.Instantiate (Resources.Load("Prefabs/Bomb"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			BombScript bs = bomb.GetComponent<BombScript> ();
			bs.Initialize(new Vector3(_rb.velocity.x * velocityMultiplier, 1f, 0f), bombIndex);
		} else {
			GameObject bomb = Object.Instantiate (Resources.Load("Prefabs/Bomb"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			BombScript bs = bomb.GetComponent<BombScript> ();
			bs.Initialize (new Vector3(_rb.velocity.x * velocityMultiplier, 1f, 0f), bombIndex);
		}
	}

	private void StickHit ()
	{
		GameObject stick;
		if (facingRight == true) {
			stick = Object.Instantiate (Resources.Load ("Prefabs/Stick"), new Vector3 (transform.position.x + 1f, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		} else {
			stick = Object.Instantiate (Resources.Load ("Prefabs/Stick"), new Vector3 (transform.position.x - 1f, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		}
		stick.transform.parent = this.gameObject.transform;
	}

	private void StickWithdraw ()
	{
		Destroy (GameObject.FindGameObjectWithTag ("Stick"));
	}

	private void ActivatePotion ()
	{
		if (!potionActivated) 
		{
			potionTime = 0f;
			potionActivated = true;

			switch (potionIndex)
			{
			case 0: 
				// Speed potion
				velocityMultiplier = 2.0f;
				potionDuration = 3.0f;
				break;
			case 1: 
				// Small potion
				this.gameObject.transform.localScale /= 4;
				_rb.mass /= 4;
				potionDuration = 3.0f;
				break;
			case 2: 
				// Freeze potion
				isFrozen = true;
				potionDuration = 2.0f;
				break;
			} 
		}
	}

	private void DeactivatePotion ()
	{
		potionActivated = false;

		switch (potionIndex) 
		{
		case 0: 
			// Speed potion
			velocityMultiplier = 1.0f;
			break;
		case 1:
			// Small potion
			this.gameObject.transform.localScale *= 4;
			_rb.mass *= 4;
			this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z);
			break;
		case 2:
			// Freeze potion
			isFrozen = false;
			break;
		} 
	}

	private void ActivateWaterPhysics ()
	{
		_rb.gravityScale /= 5f;
		velocityMultiplier = 0.5f;
	}

	private void DeactivateWaterPhysics ()
	{
		_rb.gravityScale *= 5f;
		velocityMultiplier = 1.0f;
	}

	private void Die()
	{
		velocityMultiplier = 0.0f;
		_anim.Play( Animator.StringToHash( "Witch_Dead" ) );
		isDead = true;
	}

	void OnCollisionEnter2D(Collision2D col)
	{
		Debug.Log ("Collision happened");
	}
}