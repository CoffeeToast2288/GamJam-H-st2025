using UnityEngine;
using UnityEngine.SceneManagement;

public class DSM : MonoBehaviour
{
    public void MainMenuTime()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
    public void Restarting()
    {
        SceneManager.LoadSceneAsync("Isak");
    }
    public void Quiter()
    {
        Application.Quit();
    }
}
