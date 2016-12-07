using UnityEngine;
using System.Collections;

/**
Monster ska kunna frysas (som player, fast monster fryses för alltid) 
Line of sight-aggro,(det den ser) 
Lägg till HP + ta skada av player
*/

public class MonsterAI : MonoBehaviour {


    public MonsterState startState  = MonsterState.Walk;
    public PatrolEndpoint targetPatrol;
    public float aggroDistance      = 10;
    public float runSpeed           = 2;
    public float meleeRange         = 4f;
	public int currentHP = 3;
    public int meleeDmg             = 1;
    //OBS: Changes here require update in animator-speed (in editor)
    private float attackSpeed       =  1f; 

	private GameObject player;
    private MonsterState currentState;
    private Rigidbody2D monsterBody;
    private Vector2 targetPosition;
	private Animator _animator;
    

    private int directionChangeCount;
    private int lastMovement;
    private float timeSinceAttack;
	private bool facingRight = false;
	private bool isDead = false;
	private bool frozen = false;
	private GameObject frost;
	private float frostTimer = 0.0f;

    private float lastDmgTime;

	// Use this for initialization
	void Start () {
		player = GameObject.Find ("Player");
        currentState = startState;
        monsterBody = this.GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator> ();
        directionChangeCount = 0;
        lastDmgTime = Time.time;


        walkState();
	}
	
	// Update is called once per frame
	void Update () {

		if (currentHP <= 0)
			StartCoroutine(Die());
		if (!isDead && !frozen) {
			switch (currentState) {
			case MonsterState.Sleep:
                 // check for aggro
				break;
			case MonsterState.Walk:
				moveTowardsTarget ();

				if (aggroDistance > Vector3.Distance (transform.position, player.transform.position)) {
					//player got in aggro range
					aggroState ();
				} else {
					currentState = MonsterState.Walk;
				}
				break;
			case MonsterState.Aggro:
				targetPosition = player.transform.position;
				moveTowardsTarget ();
				break;
			default:
				Debug.Log ("Unknown state, investigate");
				break;
			}
		} else {
			frostTimer += Time.deltaTime;
			if (frostTimer > 2.0f) {
				frozen = false;
				Destroy (frost.gameObject);
				frostTimer = 0f;
			}
		}
	}

    void moveTowardsTarget()
    {
        //anim.SetFloat("Speed", Mathf.Abs(move));

		_animator.Play(Animator.StringToHash("Troll_Walk"));

        int moveDirection;

		if (Vector2.Distance (player.transform.position, transform.position) < meleeRange/2)
			moveDirection = 0;
		else 
			if (targetPosition.x < transform.position.x)
        {
            moveDirection = -1;
        } else
        {
            moveDirection = 1;
        }

        timeSinceAttack += Time.deltaTime;

        if (currentState == MonsterState.Aggro && 
            Vector2.Distance(this.transform.position, player.transform.position) < meleeRange
            && timeSinceAttack > 1 / attackSpeed)
        {
            Debug.Log("SHOULD MELEE THIS FUKKER");
            _animator.Play(Animator.StringToHash("Troll_Bash"));
			MeleeAttack();
            timeSinceAttack = 0;
        }

        monsterBody.velocity = new Vector2(moveDirection * runSpeed, monsterBody.velocity.y);

		if (monsterBody.velocity.x > 0 && !facingRight) {
			Flip ();
		} else if (monsterBody.velocity.x < 0 && facingRight) {
			Flip ();
		}

    }

    void MeleeAttack()
    {
        float attackFreq = 1 / attackSpeed;
        Vector3 pos = transform.position;
        int direction;
        if (facingRight) direction = 1; else direction = -1;
        pos.x += direction * 0.3f;
        pos.y += 1.3f;
        Quaternion q = Quaternion.Euler(new Vector3(0, 0, direction * 45));
        GameObject meleeSwing = (GameObject) Instantiate(Resources.Load("Prefabs/MonsterMelee"), pos, q);
        meleeSwing.transform.parent = transform;
        meleeSwing.transform.localScale = new Vector3(1.5f, 2, 0);
        Destroy(meleeSwing, attackFreq / 5);

        _animator.Play(Animator.StringToHash("Troll_Walk"));
    }

    //Flip scale of monster (as to avoid implementing walking left animation)
    void Flip()
    {
		facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    /*
     * Switches monster into walkState.
     */
    void walkState()
    {
        Debug.Log("Entering Walk-state");
		_animator.Play (Animator.StringToHash ("Troll_Walk"));
		targetPosition = targetPatrol.transform.position;
        currentState = MonsterState.Walk;
    }

    void sleepState()
    {
        Debug.Log("Entering Sleep-state.");
		targetPosition = targetPatrol.transform.position;
        currentState = MonsterState.Sleep;
		_animator.Play (Animator.StringToHash("Troll_Idle"));
    }

    void aggroState()
    {
        Debug.Log("Entering Aggro-state.");
        targetPosition = player.transform.position;
        currentState = MonsterState.Aggro;
    }

    void OnCollisionEnter2D(Collision2D coll)
    {
        Debug.Log("Monster collide with: " + coll.gameObject.tag);
        if (coll.gameObject.tag == "Player")
        {
            if (Time.time - lastDmgTime > 0.2f)
            {
                Debug.Log("Monster take dmg from stick");
                currentHP -= coll.gameObject.GetComponentInParent<PlayerScript>().getStickDmg();
                lastDmgTime = Time.time;
            }
		} else if (coll.gameObject.CompareTag ("IceBomb"))
			Freeze ();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
		Debug.Log("Monster collide with: " + other.gameObject.tag);
		if (other.CompareTag ("PatrolObject")) {
			if (currentState == MonsterState.Walk) {
				targetPatrol = other.gameObject.GetComponent<PatrolEndpoint> ();
				targetPosition = other.gameObject.GetComponent<PatrolEndpoint> ().target.transform.position;
				//Debug.Log ("Monster new targetPos (x,y): (" + targetPosition.x + ", " + targetPosition.y + ")");
			}
		} else if (other.CompareTag ("Water")) {
			StartCoroutine (Die ());
		} else if (other.CompareTag ("Mushroom")) {
			StartCoroutine (Die ());
			if (other.gameObject.GetComponent<MineMushroom> () != null)
				other.gameObject.GetComponent<MineMushroom> ().Explode ();
		} else if (other.CompareTag ("IceBomb"))
			Freeze ();
    }

	private IEnumerator Die()
    {
		if (frozen)
			Destroy (frost);
		this.gameObject.tag = "Untagged";
		isDead = true;
        //play sound, play animation
		_animator.Play(Animator.StringToHash("Troll_Die"));
		yield return new WaitForSeconds (2.0f);
        Destroy(this.gameObject);
    }

    public bool FacingRight()
    {
        return facingRight;
    }

	public void Freeze() {
		frozen = true;
		frost = Object.Instantiate (Resources.Load ("Prefabs/Frost"), new Vector3 (transform.position.x, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
		frost.gameObject.transform.parent = this.transform;
		frost.transform.localScale = transform.localScale;
		_animator.Play(Animator.StringToHash("Troll_Idle"));
	}
}

public enum MonsterState { Sleep, Walk, Aggro };
