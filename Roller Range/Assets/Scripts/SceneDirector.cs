using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneDirector : MonoBehaviour {
	void Start()
	{
		Cursor.visible = true;
	}

    void goToGame()
    {
        SceneManager.LoadScene("main");
    }

    void goToMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    void quitGame()
    {
        Application.Quit();
    }
}
