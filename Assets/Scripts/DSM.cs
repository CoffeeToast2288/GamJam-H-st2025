using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Isak has been here no one else
public class DSM : MonoBehaviour
{
    public PlayerHealth playerHealth;
    public GameObject deathScreen;

    public Animator player_animator;
    public string[] play_animations;
    public AudioSource die;

    public void Start()
    {
        // If not assigned in Inspector, try to find it on the same GameObject or in the scene
        if (playerHealth == null)
            playerHealth = GetComponent<PlayerHealth>();

    }
    public void Update()
    {
        // Check the PlayerHealth's dead bool directly
        if (playerHealth != null && playerHealth.dead)
        {
            
            player_animator.Play(play_animations[3]);
           
            // Activate death screen only once
            if (!deathScreen.activeSelf)

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
