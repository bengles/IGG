using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CraftingScene : MonoBehaviour {

    public Toggle enchantedTurnip;
    public Canvas craftComplete;

	// Use this for initialization
	void Start () {
        craftComplete = craftComplete.GetComponent<Canvas>();
        craftComplete.enabled = false;
	}
	
    public void CraftingFunction()
    {
        craftComplete.enabled = true;
    }

}
