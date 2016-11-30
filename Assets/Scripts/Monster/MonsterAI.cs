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
    public float meleeRange         = 1.5f;
    public float attacksPerSecond   = 1;

    private MonsterState currentState;
    private Rigidbody2D monsterBody;
    private Vector2 targetPosition;
    private Vector2 spawnPosition;
    
    private int directionChangeCount;
    private int lastMovement;
    private float timeSinceAttack;

	// Use this for initialization
	void Start () {
        currentState = startState;
        monsterBody = this.GetComponent<Rigidbody2D>();
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
                //attack if in-range
                break;
            default:
                Debug.Log("Unknown state, investigate");
                break;
        }
	}

    void moveTowardsTarget()
    {
        //anim.SetFloat("Speed", Mathf.Abs(move));

        timeSinceAttack += Time.deltaTime;
        if (currentState == MonsterState.Aggro && 
            Vector3.Distance(this.transform.position, player.transform.position) < meleeRange)
        {
            //In attack-range
            float attackFreq = 1 / attacksPerSecond; //calculate every time as to allow changing value direct in editor
            if (timeSinceAttack > attackFreq)
            {
                Debug.Log("SHOULD MELEE THIS FUKKER");
                //attack-animation etc
                timeSinceAttack = 0;
            } 
            lastMovement = 0;
        } else if (Mathf.Approximately(targetPosition.x, transform.position.x)) {
            //do nuffin
        } else if (targetPosition.x < transform.position.x)
        {
            //Make sure we dont just swap all the time
            if (lastMovement != 1)
            {
                directionChangeCount = 0;
            } else
            {
                directionChangeCount++;
            }
            //Move in negative direction, 
            lastMovement = -1;
        } else
        {
            if (lastMovement != -1)
            {
                directionChangeCount = 0;
            }
            else
            {
                directionChangeCount++;
            }
            lastMovement = 1;
        }
        if (directionChangeCount > 1)
        {
            Debug.Log("Supressing movement due to direction changes");
            transform.position = new Vector3(targetPosition.x, transform.position.y, transform.position.z);
            lastMovement = 0;
        }
        monsterBody.velocity = new Vector2(lastMovement * runSpeed, monsterBody.velocity.y);


        /*
        //check if changed direction
        if (movement > 0 && !facingRight)
            Flip();
        else if (movement < 0 && facingRight)
            Flip();
        */
    }

    //Flip scale of monster (as to avoid implementing walking left animation)
    /*
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }*/

    /*
     * Switches monster into walkState.
     */
    void walkState()
    {
        Debug.Log("Entering Walk-state");
        targetPosition = new Vector2(firstPatrol.transform.position.x, firstPatrol.transform.position.y);
        currentState = MonsterState.Walk;
    }

    void sleepState()
    {
        Debug.Log("Entering Sleep-state.");
        targetPosition = new Vector2(this.transform.position.x, this.transform.position.y);
        currentState = MonsterState.Sleep;
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
        } else if (other.CompareTag("Water"))
        {
            Destroy(this);
        }
    }
}

public enum MonsterState { Sleep, Walk, Aggro };
