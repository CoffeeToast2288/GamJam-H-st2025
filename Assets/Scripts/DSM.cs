using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DSM : MonoBehaviour
{
    PlayerHealth isdead;
    PlayerStats stats;
    public GameObject deathScreen;
    private bool dead;

    public void Update()
    {
        dead = isdead.dead;
        if (dead == true)
        {
            deathScreen.SetActive(true);
        }
    }

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
