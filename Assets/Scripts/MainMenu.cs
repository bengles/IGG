using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

	// Use this for initialization
	void Start () {
       
	}
	
    public void Demo1Press()
    {
        SceneManager.LoadScene(2);
    }

    public void Demo2Press()
    {
        SceneManager.LoadScene(2);
    }
}
