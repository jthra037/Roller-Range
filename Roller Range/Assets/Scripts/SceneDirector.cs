using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneDirector : MonoBehaviour {
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
