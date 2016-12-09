using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneDirector : MonoBehaviour {
	void Start()
	{
		Cursor.visible = true;
		Cursor.lockState = CursorLockMode.Confined;
	}

    void goToGame()
    {
        SceneManager.LoadScene("main");
    }

    void goToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void goToCredits()
    {
        SceneManager.LoadScene("Credits");
    }

    void quitGame()
    {
        Application.Quit();
    }
}
