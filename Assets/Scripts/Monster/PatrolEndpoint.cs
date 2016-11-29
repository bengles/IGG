using UnityEngine;
using System.Collections;

public class PatrolEndpoint : MonoBehaviour {
    public PatrolEndpoint target;

    void Start()
    {
        GetComponent<SpriteRenderer>().enabled = false; //don't want red arrows to show up in-game
    }
}
