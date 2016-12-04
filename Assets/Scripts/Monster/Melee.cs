using UnityEngine;
using System.Collections;

public class Melee : MonoBehaviour {

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false;
    }

	void FixedUpdate () {
        float attackSpeed = 1;
        
        transform.Rotate(Time.deltaTime * attackSpeed * Vector3.forward * 300);
        transform.Translate(new Vector3(0, attackSpeed * Time.deltaTime * -20, 0));
	}
}
