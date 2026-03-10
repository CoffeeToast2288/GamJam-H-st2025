using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

// Isak has been here no one else
public class DSM : MonoBehaviour
{
    public Health_Script playerHealth;
    public GameObject deathScreen;
    public GameObject buttons;
    public Animator animator;
    public Animator animator1;



    public void Start()
    {
        // If not assigned in Inspector, try to find it on the same GameObject or in the scene
        if (playerHealth == null)
            playerHealth = GetComponent<Health_Script>();

    }
    public void Update()
    {
        // Check the PlayerHealth's dead bool directly
        if (playerHealth != null && playerHealth.dead)
        {
            StartCoroutine(Dies());
        }
    }

    public IEnumerator Dies()
    {
        deathScreen.SetActive(true);
        animator.SetTrigger("Start");
        yield return new WaitForSeconds(2.5f);
        buttons.SetActive(true);
        animator1.SetTrigger("Start2");

    }
    


    public void MainMenuTime()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
    public void Restarting()
    {
        SceneManager.LoadSceneAsync("Main scene");
    }
    public void Quiter()
    {
        Application.Quit();
    }
}
