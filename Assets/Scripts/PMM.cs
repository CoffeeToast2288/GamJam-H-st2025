using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class PMM : MonoBehaviour
{
    private bool isPaused = false;
    public GameObject pauseMenu, volumeMenu, statsMenu;
    public TextMeshProUGUI healthText, damageText, speedText, attackSpeedText, dashChargesText, dashcooldownText;
    [SerializeField] PlayerStats stats;


    public void Update()
    {
        // Toggle pause when pressing Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }
    // Goes back to main menu
    public void MainMenuTime()
    {
        SceneManager.LoadSceneAsync("Main Menu");
    }
    // Call this to pause the game
    public void Pause()
    {
        pauseMenu.SetActive(true);   // Show the menu
        volumeMenu.SetActive(false); // incase the volume menu is active it is hidden 
        statsMenu.SetActive(false);
        Time.timeScale = 0f;           // Stop time
        isPaused = true;
    }

    // Call this to resume the game 
    public void Resume()
    {
        pauseMenu.SetActive(false);  // Hide the menu
        volumeMenu.SetActive(false); // incase the volume menu is active it is hidden 
        statsMenu.SetActive(false);
        Time.timeScale = 1f;           // Resume time
        isPaused = false;
    }
    // brings up the volume menu
    public void volume()
    {
        pauseMenu.SetActive(false);  // Hide the menu
        volumeMenu.SetActive(true);//  shows the volume menu 
    }
    public void StatsMenu()
    {
        pauseMenu.SetActive(false);  // Hide the menu
        statsMenu.SetActive(true);
        healthText.text = "Your hp is: "+ stats.hp;
        damageText.text = "Your damage is: " + stats.damage;
        speedText.text = "Your walkingspeed is: " + stats.speed;
        attackSpeedText.text = "Your attackspeed is: " + stats.attack_speed;
        dashChargesText.text = "Your amount of dash Charges are: " + stats.dash_chargers;
        dashcooldownText.text = "Your cooldown for dashing is: " + stats.dash_coldown_reduction;
    }
    public void GoBack()
    {
        statsMenu.SetActive(false);
        volumeMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }
}
