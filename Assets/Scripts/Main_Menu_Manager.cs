using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Main_Menu_Manager : MonoBehaviour
{

    // It is Isak's code back off
    public void Start()
    {
        // just if you pause and go back
        Time.timeScale = 0;
    }
    // Starts the game
    public void Starting()
    {
        Time.timeScale = 1;
        SceneManager.LoadSceneAsync("Maine Scean");
    }

    // FUCKING QUITS THE GAME DUH.
    public void Quiter()
    {
        Application.Quit();
    }
}
