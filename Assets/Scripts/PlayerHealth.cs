using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    public float Hp;
    public float Hp_max;
    public PlayerStats stats;
    public TextMeshProUGUI healthText;

    [Header("UI Feedback")]
    public TextMeshProUGUI flashText;        // Assign a TMP UI Text prefab in Inspector
    public float flashDuration = 1f;         // How long the flash shows

    public bool dead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        healthText.text = (Hp + "/"+Hp_max+" hp");

        if(Hp > Hp_max)
        {

            Hp = Hp_max;

        }
    }

    public void Heal(float healing)
    {
        if (Hp < Hp_max)
        {
            Hp += healing;
            // Show UI flash
            if (flashText != null)
            {
                StartCoroutine(ShowFlash($"+{healing} HP!"));
            }
        }
    }
    public void TakeDamage(float damage)
    {       
        Hp -= damage;
        if (Hp <= 0)
        {
            Hp = 0;
            dead = true;
        }       
    }
    // Coroutine to show temporary UI flash
    private System.Collections.IEnumerator ShowFlash(string message)
    {
        flashText.text = message;
        flashText.gameObject.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        flashText.gameObject.SetActive(false);
    }
}
