using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class Main_Menu_Manager : MonoBehaviour
{

    public void Starting()
    {
        SceneManager.LoadSceneAsync("Isak");
    }

    //FUCKING QUITS THE GAME DUH.
    public void Quiter()
    {
        Application.Quit();
    }
}
