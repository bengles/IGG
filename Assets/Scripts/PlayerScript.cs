﻿using UnityEngine;
using System.Collections;
using Prime31;
using UnityEngine.SceneManagement;
using System.Collections.Generic;


public class PlayerScript : MonoBehaviour
{
	// movement config
	public float gravity = -25f;
	public float runSpeed = 8f;
	public float groundDamping = 20f; // how fast do we change direction? higher means faster
	public float inAirDamping = 5f;
	public float jumpHeight = 3f;
    public int currentHP = 3;
	public int potionIndex = -1;
	public int bombIndex = -1;
	public int staffIndex = -1;
	public bool hasAllItems = false;

	[HideInInspector]
	private float normalizedHorizontalSpeed = 0;
	private bool facingRight = true;
	private float potionTime;
	private bool potionActivated = false;
	private float potionDuration;
	private bool frozen = false;
	private bool inWater = false;
	private float waterTimer = 0.0f;
	private bool isDead = false;
	private float deadTimer = 0.0f;
	private bool inAir;
	private float poisonTimer = 0.0f;
	private bool InPoison = false;
    private int stickDmg = 1;
	private float damageTimer = 0.0f;

	private int[] potionIndexes;
	private int potionArrayIndex = 0;

	private int[] bombIndexes;
	private int bombArrayIndex = 0;

	private int[] staffIndexes;
	private int staffArrayIndex = 0;

	private CharacterController2D _controller;
	private Animator _animator;
	private RaycastHit2D _lastControllerColliderHit;
	private Vector3 _velocity;
	private Rigidbody2D _rb;
	private AudioSource _as;
	private bool hitting = false;
	private bool poisonImmune = false;
	private bool invulnerable = false;
	private bool XaxisInUse = false;
	private bool YaxisInUse = false;

	private AudioClip[] audioClips;
	private AudioClip[] audioHurt;
	private GameObject frost;


	void Awake()
	{
		_animator = GetComponent<Animator>();
		_controller = GetComponent<CharacterController2D>();
		_rb = GetComponent<Rigidbody2D> ();
		_as = GetComponent<AudioSource> ();
		audioClips = new AudioClip[] {Resources.Load ("Audio/Witch/haha_1") as AudioClip, 
			Resources.Load ("Audio/Witch/haha_2") as AudioClip,
			Resources.Load ("Audio/Witch/humdidum_1") as AudioClip,
			Resources.Load ("Audio/Witch/humdidum_2") as AudioClip,
			Resources.Load ("Audio/Witch/whoa_1") as AudioClip,
			Resources.Load ("Audio/Witch/laugh_short_1") as AudioClip,
			Resources.Load ("Audio/Witch/laugh_long_1") as AudioClip
		};
		audioHurt = new AudioClip[] {Resources.Load ("Audio/Witch/hurt_1") as AudioClip, 
			Resources.Load ("Audio/Witch/hurt_2") as AudioClip
		};

		// listen to some events for illustration purposes
		_controller.onControllerCollidedEvent += onControllerCollider;
		_controller.onTriggerEnterEvent += onTriggerEnterEvent;
		_controller.onTriggerStayEvent += onTriggerStayEvent;
		_controller.onTriggerExitEvent += onTriggerExitEvent;

		if (hasAllItems)
			AddAllItems ();

	}

	void Start()
	{
		potionIndexes = new int[GlobalData.Instance.equippedPotions.Count];
		bombIndexes = new int[GlobalData.Instance.equippedBombs.Count];
		staffIndexes = new int[GlobalData.Instance.equippedStaffs.Count];


		foreach (Item i in GlobalData.Instance.equippedBombs)
			if (i.cat == ItemCategory.Bomb && i != null) {
				bombIndex = i.index;
				break;
			}
		foreach (Item i in GlobalData.Instance.equippedBombs)
			if (i.cat == ItemCategory.Bomb && i != null) {
				bombIndexes [bombArrayIndex] = i.index;
				bombArrayIndex++;
			}
		bombArrayIndex = 0;

		foreach (Item i in GlobalData.Instance.equippedPotions)
			if (i.cat == ItemCategory.Potion && i != null) {
				potionIndex = i.index;
				break;
			}
		foreach (Item i in GlobalData.Instance.equippedPotions)
			if (i.cat == ItemCategory.Potion && i != null) {
				potionIndexes [potionArrayIndex] = i.index;
				potionArrayIndex++;
			}
		potionArrayIndex = 0;

		foreach (Item i in GlobalData.Instance.equippedStaffs) 
			if (i.cat == ItemCategory.Staff && i != null) {
				staffIndex = i.index;
				break;
			}
		foreach (Item i in GlobalData.Instance.equippedStaffs) 
			if (i.cat == ItemCategory.Staff && i != null) {
				staffIndexes [staffArrayIndex] = i.index;
				staffArrayIndex++;
			}
		staffArrayIndex = 0;
	}

	#region Event Listeners

	void onControllerCollider( RaycastHit2D hit )
	{
		// bail out on plain old ground hits cause they arent very interesting
		if( hit.normal.y == 1f )
			return;

		if (hit.collider.gameObject.tag == "Enemy")
		{
			InflictDamage (1);
		}

		if (hit.collider.gameObject.tag == "Mushroom") {
			currentHP = 0;
			Die ();
		}

		// logs any collider hits if uncommented. it gets noisy so it is commented out for the demo
	    //Debug.Log( "flags: " + _controller.collisionState + ", hit.normal: " + hit.normal );
	}


	void onTriggerEnterEvent( Collider2D col )
	{
		if (col.gameObject.layer == 4) 
		{
			if (col.gameObject.tag == "Mushroom") {
				Vector3 direction = transform.position - col.gameObject.transform.position;
				_velocity = 35f * direction.normalized;
				if (!invulnerable)
					Die ();
			}
			// Debug.Log( "onTriggerEnterEvent: " + col.gameObject.name );
			if (col.gameObject)
			if (col.gameObject.tag == "Water")
				ActivateWaterPhysics ();
			if (col.gameObject.tag == "Enemy")
            {
				Vector3 direction = transform.position - col.gameObject.transform.position;
				_velocity = 100f * direction.normalized;
				InflictDamage (1);
            }
			if (col.gameObject.tag == "Poison") { 
				InPoison = true;
    		}
			if (col.gameObject.tag == "FireWall") 
			{
				if (!frozen) {
					currentHP = 0;
					Die ();
				}
				else
					DeactivatePotion ();
			}
			if (col.gameObject.tag == "Item")
			{
				Pickup (col.gameObject.GetComponent<ItemScript>().item);
			}
				
		} else if (col.gameObject.layer == 0)           //this shit doesnt seem to fire /Johan
        {
            Debug.Log("Layer 9 event");
            if (col.gameObject.tag == "MonsterAttack")
            {
                int dmg = col.transform.gameObject.GetComponentInParent<MonsterAI>().meleeDmg;
                Debug.Log("Took " + dmg + " dmg.");
            }
        }	
	}

	void onTriggerStayEvent( Collider2D col)
	{
		
	}

	void onTriggerExitEvent( Collider2D col )
	{
		//Debug.Log( "onTriggerExitEvent: " + col.gameObject.name );

		if (col.gameObject.layer == 4) 
		{
			if (col.gameObject.tag == "Water") 
				DeactivateWaterPhysics ();
			if (col.gameObject.tag == "Poison") 
				InPoison = false;
		}
	}

	#endregion



	// the Update loop contains a very simple example of moving the character around and controlling the animation
	void Update()
	{

		damageTimer += Time.deltaTime;

		if (currentHP <= 0)
			Die ();

		if (potionTime >= potionDuration && potionActivated) {
			DeactivatePotion ();
		}

		if (isDead)
			deadTimer += Time.deltaTime;

		if (InPoison && !poisonImmune)
			poisonTimer += Time.deltaTime;
		else
			poisonTimer = 0;

		if (poisonTimer > 1f && !poisonImmune)
			Die ();

		if (deadTimer > 2.0f)
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);

		if (inWater)
			waterTimer += Time.deltaTime;

		if (waterTimer > 0.3f)
			Die ();

		potionTime += Time.deltaTime;

		if (_controller.isGrounded) {
			_velocity.y = 0;
			if (inAir) {
				inAir = false;
				_as.Stop ();
				_as.clip = Resources.Load ("Audio/Witch/land_1") as AudioClip;
				_as.Play ();
			}
		}

		if (Input.GetButtonDown ("Reset"))
			SceneManager.LoadScene (SceneManager.GetActiveScene().buildIndex);
		if (Input.GetButtonDown ("Submit"))
			SceneManager.LoadScene (0);

		if (Input.GetButton ("B") && !isDead && !frozen) {
			if (staffIndex != -1) {
				if (_controller.isGrounded)
					normalizedHorizontalSpeed = 0;
				StickHit ();
			}
		}  else if ( Input.GetAxis( "Horizontal" ) > 0  && !isDead && !frozen) {
			
			normalizedHorizontalSpeed = 1;
			if( transform.localScale.x < 0f )
				transform.localScale = new Vector3( -transform.localScale.x, transform.localScale.y, transform.localScale.z );

			if( _controller.isGrounded )
				_animator.Play( Animator.StringToHash( "Witch_Run" ) );

			facingRight = true;
		} else if( Input.GetAxis( "Horizontal" ) < 0  && !isDead && !frozen) {
			
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

			if (_controller.isGrounded && !isDead)
				_animator.Play (Animator.StringToHash ("Witch_Idle"));
		}

		if (!_controller.isGrounded && !isDead && !hitting)
			_animator.Play (Animator.StringToHash ("Witch_Fall"));

		// we can only jump whilst grounded
		if( _controller.isGrounded && Input.GetButtonDown("Jump")  && !isDead && !frozen)
		{
			inAir = true;
			_velocity.y = Mathf.Sqrt( 3f * jumpHeight * -gravity );
			_animator.Play( Animator.StringToHash( "Witch_Jump" ) );
			if (Random.Range (0, 2) == 1) {
				_as.Stop ();
				_as.clip = Resources.Load ("Audio/Witch/jump_1") as AudioClip;
				_as.Play ();
			}
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

		if (Input.GetAxisRaw ("DX") != 0f  && !isDead ) {
			if (XaxisInUse == false && Input.GetAxisRaw("DX") == -1f) {
				XaxisInUse = true;
				if (GlobalData.Instance.equippedBombs.Count > 1) {
					bombIndex = bombIndexes [bombArrayIndex];
					bombArrayIndex = (bombArrayIndex + 1) % GlobalData.Instance.equippedBombs.Count;
				}
			}
		} 

		if (Input.GetAxisRaw ("DX") != 0f  && !isDead ) {
			if (XaxisInUse == false && Input.GetAxisRaw("DX") == 1f) {
				XaxisInUse = true;
				if (GlobalData.Instance.equippedStaffs.Count > 1) {
					staffIndex = staffIndexes [staffArrayIndex];
					staffArrayIndex = (staffArrayIndex + 1) % GlobalData.Instance.equippedStaffs.Count;
				}
			}
		} 

		if (Input.GetAxisRaw ("DX") == 0)
			XaxisInUse = false;

		if (Input.GetKeyDown(KeyCode.Alpha1) && !isDead ) {
			if (GlobalData.Instance.equippedBombs.Count > 1) {
				bombIndex = bombIndexes [bombArrayIndex];
				bombArrayIndex = (bombArrayIndex + 1) % GlobalData.Instance.equippedBombs.Count;
			}
		} 

		if (Input.GetAxisRaw ("DY") == 1f && !isDead && !potionActivated) {
			if (YaxisInUse == false && Input.GetAxisRaw ("DY") == 1f) {
				YaxisInUse = true;
				if (GlobalData.Instance.equippedPotions.Count > 1) {
					potionIndex = potionIndexes [potionArrayIndex];
					potionArrayIndex = (potionArrayIndex + 1) % GlobalData.Instance.equippedPotions.Count;
				}
			}
		}

		if (Input.GetAxisRaw ("DY") == 0)
			YaxisInUse = false;

		if (Input.GetKeyDown(KeyCode.Alpha2) && !isDead && !potionActivated) {
			if (GlobalData.Instance.equippedPotions.Count > 1) {
				potionIndex = potionIndexes [potionArrayIndex];
				potionArrayIndex = (potionArrayIndex + 1) % GlobalData.Instance.equippedPotions.Count;
			}
		}

		if (Input.GetKeyDown(KeyCode.Alpha3) && !isDead) {
			if (GlobalData.Instance.equippedStaffs.Count > 1) {
				staffIndex = staffIndexes [staffArrayIndex];
				staffArrayIndex = (staffArrayIndex + 1) % GlobalData.Instance.equippedStaffs.Count;
			}
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
		if (bombIndex != -1) {
			if (facingRight == true) {
				GameObject bomb = Object.Instantiate (Resources.Load ("Prefabs/Bomb"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
				BombScript bs = bomb.GetComponent<BombScript> ();
				bs.Initialize (new Vector3 (runSpeed, 1f, 0f), bombIndex);
			} else {
				GameObject bomb = Object.Instantiate (Resources.Load ("Prefabs/Bomb"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
				BombScript bs = bomb.GetComponent<BombScript> ();
				bs.Initialize (new Vector3 (-runSpeed, 1f, 0f), bombIndex);
			}
			PlayRandomSound ();
		}
	}

	private void StickHit ()
	{
		_animator.Play (Animator.StringToHash("Witch_Melee"));
		GameObject stick = null;

        if (!hitting)
        {
            //Debug.Log("Instantiating stick1");
            int direction = facingRight ? 1 : -1;
            Vector3 pos = transform.position;
            pos.x += direction * 1f;
            pos.y -= 0.22f;
            stick = (GameObject)Instantiate(Resources.Load("Prefabs/Stick"), pos, Quaternion.identity);
            stick.transform.parent = transform;
            stick.transform.localScale = 2 * stick.transform.localScale;
            Destroy(stick, 0.1f);
        }

		hitting = true;
	}

	private void StickWithdraw ()
	{
		hitting = false;
		foreach (GameObject go in GameObject.FindGameObjectsWithTag ("Stick"))
			Destroy (go);
	}

	private void ActivatePotion ()
	{
		if (!potionActivated) 
		{
			potionTime = 0f;
			potionActivated = true;

			switch (potionIndex) {
			case -1:
				break;
			case 0: 
				// Freeze potion
				frozen = true;
				frost = Object.Instantiate (Resources.Load ("Prefabs/Frost"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
				frost.gameObject.transform.parent = this.transform;
				potionDuration = 2.0f;
				break; 
			case 1: 
				// Curry potion
				runSpeed *= 2f;
				potionDuration = 3.0f;
				break;
			case 2: 
				// Small potion
				this.gameObject.transform.localScale /= 4;
				_rb.mass /= 4;
				potionDuration = 3.0f;
				break;
			case 3:
				// Peppermint Tea
				break;
			case 4:
				// Sniffle Enhancer
				poisonImmune = true;
				potionDuration = 10f;
				break;
			case 5:
				// Invulnerable
				invulnerable = true;
				potionDuration = 5f;
				break;
			default:
				PlayRandomSound ();
				break;
			}
		}
	}

	private void DeactivatePotion ()
	{
		potionActivated = false;

		switch (potionIndex) 
		{
		case -1:
			break;
		case 1: 
			// Speed potion
			runSpeed /= 2f;
			break;
		case 2:
			// Small potion
			this.gameObject.transform.localScale *= 4;
			_rb.mass *= 4;
			this.gameObject.transform.position = new Vector3 (this.gameObject.transform.position.x, this.gameObject.transform.position.y + 0.5f, this.gameObject.transform.position.z);
			break;
		case 0:
			// Freeze potion
			frozen = false;
			Destroy (frost);
			break;
		case 3:
			// Peppermint Tea
			break;
		case 4:
			// Sniffle Enhancer
			poisonImmune = false;
			break;
		case 5:
			invulnerable = false;
			break;
		} 
	}

	private void ActivateWaterPhysics ()
	{
		gravity /= 5f;
		runSpeed /= 2f;
		inAirDamping /= 2f;
		groundDamping -= 5f;
		inWater = true;
	}

	private void DeactivateWaterPhysics ()
	{
		waterTimer = 0.0f;
		gravity *= 5f;
		runSpeed *= 2f;
		inAirDamping *= 2f;
		groundDamping += 5f;
		inWater = false;
	}

	private void Die()
	{
		runSpeed = 0f;
		_animator.Play( Animator.StringToHash( "Witch_Dead" ) );
		_as.Stop ();
		_as.clip = Resources.Load ("Audio/Witch/die_1") as AudioClip;
		_as.Play ();
		isDead = true;	
	}

    private void PlayRandomSound()
	{
		_as.Stop ();
		_as.clip = audioClips[Random.Range(0,7)];
		_as.Play ();
	}

    public int getStickDmg()
    {
        return stickDmg;
    }

	private void Pickup (Item item) {
		if (item.name == "Extra Slot")
			GlobalData.Instance.nrPotionSlots++;
		else if (!GlobalData.Instance.HasItemID(item.index, item.cat))
			GlobalData.Instance.currentInventory.Add (item);
	}

	public void InflictDamage (int damage) {
		if (damageTimer > 1f) {
			if (!invulnerable) {
				_as.Stop ();
				_as.clip = audioHurt[Random.Range(0,2)];
				_as.Play ();
				currentHP -= damage;
				damageTimer = 0;
			}
			if (currentHP <= 0)
				Die ();
		}
	}

	public Vector3 GetVelocity () {
		return _velocity;
	}

	public void AddAllItems () {
		GlobalData.Instance.equippedBombs.Add (new Item(-1, ItemCategory.Bomb));
		GlobalData.Instance.equippedBombs.Add (new Item(0, ItemCategory.Bomb));
		GlobalData.Instance.equippedBombs.Add (new Item(1, ItemCategory.Bomb));

		GlobalData.Instance.equippedPotions.Add (new Item(-1, ItemCategory.Potion));
		GlobalData.Instance.equippedPotions.Add (new Item(0, ItemCategory.Potion));
		GlobalData.Instance.equippedPotions.Add (new Item(1, ItemCategory.Potion));
		GlobalData.Instance.equippedPotions.Add (new Item(2, ItemCategory.Potion));
		GlobalData.Instance.equippedPotions.Add (new Item(3, ItemCategory.Potion));
		GlobalData.Instance.equippedPotions.Add (new Item (4, ItemCategory.Potion));
		GlobalData.Instance.equippedPotions.Add (new Item (5, ItemCategory.Potion));

		GlobalData.Instance.equippedStaffs.Add (new Item (-1, ItemCategory.Staff));
		GlobalData.Instance.equippedStaffs.Add (new Item (0, ItemCategory.Staff));

	}
}
