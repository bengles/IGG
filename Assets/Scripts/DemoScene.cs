﻿using UnityEngine;
using System.Collections;
using Prime31;


public class DemoScene : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
	public bool isDead = false;
	public int potionIndex = 0;
	public int bombIndex = 0;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private bool facingRight = true;
	private float potionTime;
	private bool potionActivated = false;
	private float potionDuration;
	private bool frozen = false;
	public float velocity;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private Rigidbody2D _rb;


	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		_rb = GetComponent<Rigidbody2D> ();

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerStayEvent += onTriggerStayEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;
	}


	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		if (hit.collider.gameObject.tag == "Enemy")
			Die ();

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
		//Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );

		if (col.gameObject.layer == 4) 
		{
			if (col.gameObject.tag == "Water")
				ActivateWaterPhysics ();
		}
			
	}

	void onTriggerStayEvent( Collider2D col)
	{
		
	}

	void onTriggerExitEvent( Collider2D col )
	{
		Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );

		if (col.gameObject.layer == 4) 
		{
			if (col.gameObject.tag == "Water")
				DeactivateWaterPhysics ();
		}
	}

	#endregion


	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{

		if (potionTime >= potionDuration && potionActivated) {
			DeactivatePotion ();
		}

		potionTime += Time.deltaTime;

		if( _controller.isGrounded )
			_velocity.y = 0;

		if( Input.GetAxis( "Horizontal" ) > 0  && !isDead && !frozen)
		{
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Witch_Run" ) );

			facingRight = true;
		}
		else if( Input.GetAxis( "Horizontal" ) < 0  && !isDead && !frozen)
		{
			normalizedHorizontalSpeed = -1;
			if( transform.localScale.x > 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Witch_Run" ) );

			facingRight = false;
		}
		else
		{
			normalizedHorizontalSpeed = 0;

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Witch_Idle" ) );
		}


		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetButtonDown("Jump")  && !isDead && !frozen)
		{
			_velocity.y = Mathf.Sqrt( 3f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Jump" ) );
		}

		if (Input.GetButtonDown ("B") && !isDead && !frozen) {
			print ("B pressed");
			StickHit ();
		}

		if (Input.GetButtonUp ("B") && !isDead && !frozen) {
			StickWithdraw ();
		}

		if (Input.GetButtonDown ("X") && !isDead && !frozen) {
			ThrowBomb ();
		}

		if (Input.GetButtonDown ("Y") && !isDead && !frozen) {
			ActivatePotion ();
		}

		if (Input.GetButtonDown ("Fire1") && !isDead && !frozen) {
			_controller.rigidBody2D.AddForce (new Vector2 (0f, 1000f));
		}

		if (Input.GetButtonDown ("Fire2") && !isDead && !frozen && !potionActivated) {
			potionIndex = (potionIndex + 1) % 3;
		}

		if (Input.GetButtonDown ("Fire3") && !isDead && !frozen) {
			bombIndex = (bombIndex + 1) % 2;
		}


		// apply horizontal speed smoothing it. dont really do this with Lerp. Use SmoothDamp or something that provides more control
		var smoothedMovementFactor = _controller.isGrounded ? groundDamping : inAirDamping; // how fast do we change direction?
		_velocity.x = Mathf.Lerp( _velocity.x, normalizedHorizontalSpeed * runSpeed, Time.deltaTime * smoothedMovementFactor );

		// apply gravity before moving
		_velocity.y += gravity * Time.deltaTime;

		// if holding down bump up our movement amount and turn off one way platform detection for a frame.
		// this lets us jump down through one way platforms
		if( _controller.isGrounded && Input.GetKey( KeyCode.DownArrow ) )
		{
			_velocity.y *= 3f;
			_controller.ignoreOneWayPlatformsThisFrame = true;
		}

		_controller.move( _velocity * Time.deltaTime );

		// grab our current _velocity to use as a base for all calculations
		_velocity = _controller.velocity;
	}

	private void ThrowBomb ()
	{
		
		if (facingRight == true) {
			GameObject bomb = Object.Instantiate (Resources.Load("Prefabs/Bomb"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			BombScript bs = bomb.GetComponent<BombScript> ();
			bs.Initialize(new Vector3(runSpeed, 1f, 0f), bombIndex);
		} else {
			GameObject bomb = Object.Instantiate (Resources.Load("Prefabs/Bomb"), new Vector3(transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
			BombScript bs = bomb.GetComponent<BombScript> ();
			bs.Initialize (new Vector3(-runSpeed, 1f, 0f), bombIndex);
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
				runSpeed *= 2f;
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
				frozen = true;
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
			runSpeed /= 2f;
			break;
		case 1:
			// Small potion
			this.gameObject.transform.localScale *= 4;
			potionDuration = 3.0f;
			_rb.mass *= 4;
			this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z);
			break;
		case 2:
			// Freeze potion
			frozen = false;
			potionDuration = 2.0f;
			break;
		} 
	}

	private void ActivateWaterPhysics ()
	{
		gravity /= 5f;
		runSpeed /= 2f;
		inAirDamping /= 2f;
		groundDamping -= 5f;
	}

	private void DeactivateWaterPhysics ()
	{
		gravity *= 5f;
		runSpeed *= 2f;
		inAirDamping *= 2f;
		groundDamping += 5f;
	}

	private void Die()
	{
		runSpeed = 0f;
		_animator.Play( Animator.StringToHash( "Witch_Dead" ) );
		isDead = true;
	}

}
