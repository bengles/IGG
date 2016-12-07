using UnityEngine;
using UnityEngine.SceneManagement;

public class Demo1Menu : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}

    public void CraftingPress()
    {
        SceneManager.LoadScene(2);
    }

    public void PlayPress()
    {
		SceneManager.LoadScene(5);
    }
}
