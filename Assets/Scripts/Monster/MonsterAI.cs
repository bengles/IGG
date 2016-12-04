using UnityEngine;
using System.Collections;

/**
Monster ska kunna frysas (som player, fast monster fryses för alltid) 
Line of sight-aggro,(det den ser) 
Lägg till HP + ta skada av player
*/

public class MonsterAI : MonoBehaviour {


    public MonsterState startState  = MonsterState.Walk;
    public PatrolEndpoint firstPatrol;
    public GameObject player;
    public float aggroDistance      = 5;
    public float runSpeed           = 2;
    public int HP                   = 10;
    public float meleeRange         = 4f;
    public int meleeDmg             = 1;
    //OBS: Changes here require update in animator-speed (in editor)
    private float attackSpeed       =  1f; 

    private MonsterState currentState;
    private Rigidbody2D monsterBody;
    private Vector2 targetPosition;
    private Vector2 spawnPosition;
	private Animator _animator;
    

    private int directionChangeCount;
    private int lastMovement;
    private float timeSinceAttack;
	private bool facingRight = false;
    private int currentHP;

	// Use this for initialization
	void Start () {
        currentState = startState;
        currentHP = HP;
        monsterBody = this.GetComponent<Rigidbody2D>();
		_animator = GetComponent<Animator> ();
        if (monsterBody == null) Debug.Log("No Rigidbody on monster");
        spawnPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        directionChangeCount = 0;
        switch(currentState)
        {
            case MonsterState.Walk:
                walkState();
                break;
        }
	}
	
	// Update is called once per frame
	void FixedUpdate () {
	    switch(currentState)
        {
            case MonsterState.Sleep:
                 // check for aggro
                 break;
            case MonsterState.Walk:
                moveTowardsTarget();

                if (aggroDistance > Vector3.Distance(this.transform.position, player.transform.position))
                {
                    //player got in aggro range
                    aggroState();
                }
                break;
            case MonsterState.Aggro:
                targetPosition = player.transform.position;
                moveTowardsTarget();
                break;
            default:
                Debug.Log("Unknown state, investigate");
                break;
        }
	}

    void moveTowardsTarget()
    {
        //anim.SetFloat("Speed", Mathf.Abs(move));

        int moveDirection;
        if (Mathf.Abs(targetPosition.x - transform.position.x) < meleeRange / 3)
        {
            moveDirection = 0;
        } else if (targetPosition.x < transform.position.x)
        {
            moveDirection = -1;
        } else
        {
            moveDirection = 1;
        }

        timeSinceAttack += Time.deltaTime;
        if (currentState == MonsterState.Aggro && 
            Vector3.Distance(this.transform.position, player.transform.position) < meleeRange
            && timeSinceAttack > 1 / attackSpeed)
        {
            Debug.Log("SHOULD MELEE THIS FUKKER");
            _animator.Play(Animator.StringToHash("Troll_Bash"));
            Invoke("MeleeAttack", 1 / attackSpeed);
            timeSinceAttack = 0;
        }

        /*
        if (directionChangeCount > 1)
        {
            Debug.Log("Supressing movement due to direction changes");
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
            lastMovement = 0;
        }*/
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
        targetPosition = new Vector2(firstPatrol.transform.position.x, firstPatrol.transform.position.y);
        currentState = MonsterState.Walk;
    }

    void sleepState()
    {
        Debug.Log("Entering Sleep-state.");
        targetPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        currentState = MonsterState.Sleep;
		_animator.Play (Animator.StringToHash("Troll_Idle"));
    }

    void aggroState()
    {
        Debug.Log("Entering Aggro-state.");
        targetPosition = player.transform.position;
        currentState = MonsterState.Aggro;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PatrolObject"))
        {
            if (currentState == MonsterState.Walk)
            {
                PatrolEndpoint targetPatrol = other.gameObject.GetComponent<PatrolEndpoint>().target;
                targetPosition = new Vector2(targetPatrol.transform.position.x, targetPatrol.transform.position.y);
                Debug.Log("Monster new targetPos (x,y): (" + targetPosition.x + ", " + targetPosition.y + ")");
            }
        }
        else if (other.CompareTag("Water"))
        {
            Destroy(this.gameObject);
        }
        else if (other.CompareTag("Mushroom"))
            Destroy(this.gameObject);
        else if (other.CompareTag("Stick"))
        {
            currentHP -= other.gameObject.GetComponentInParent<PlayerScript>().getStickDmg();
            if (currentHP < 1)
            {
                Die();
            }
        }
    }

    private void Die()
    {
        //play sound, play animation
        Destroy(this.gameObject);
    }

    public bool FacingRight()
    {
        return facingRight;
    }
}

public enum MonsterState { Sleep, Walk, Aggro };
