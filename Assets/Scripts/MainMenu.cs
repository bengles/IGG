using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
	}
	
    public void Demo1Press()
    {
        SceneManager.LoadScene(1);
    }

    public void Demo2Press()
    {
        //SceneManager.LoadScene(2);
    }
}
