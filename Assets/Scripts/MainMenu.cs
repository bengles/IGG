using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour {

    public Button playButton;
    public Button exitButton;

	// Use this for initialization
	void Start () {
        playButton = playButton.GetComponent<Button>();
        exitButton = exitButton.GetComponent<Button>();
	}
	
    public void ExitPress()
    {
        Application.Quit();
    }

    public void PlayPress()
    {
        SceneManager.LoadScene(1);
    }
}
