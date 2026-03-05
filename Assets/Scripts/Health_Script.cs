using TMPro;
using UnityEngine;

public class Health_Script : MonoBehaviour
{
    [Header("Health Settings")]
    public float health;
    public float maxHealth;
    public float iFramesDuration = 1f;
    public bool dead = false;


    [Header("References")]
    public SpriteRenderer playerSprite; //  Drag your Player Sprite object here
    public PlayerStats stats;
    public TextMeshProUGUI healthText;
    public GameObject playerHurtBox;
    public GameObject Warning;

    private bool framed = false;

    [Header("UI Feedback")]
    public TextMeshProUGUI flashText;
    public float flashDuration = 1f;

    public void Start()
    {
        health = stats.baseHP;
        healthText.text = health + "/" + maxHealth;
    }
    // Update is called once per frame
    void Update()
    {

        if (health <= 0)
        {
            health = 0;

            dead = true;
            Die();   // ✅ IMPORTANT
        }
    }
    public void Heal(float healing)
    {
        if (health < maxHealth)
        {
            health += healing;
            if (health > maxHealth) health = maxHealth;
            if (health >= maxHealth) Warning.SetActive(false);
            healthText.text = health + "/" + maxHealth;
            if (flashText != null)
                StartCoroutine(ShowFlash($"+{healing} HP!"));
        }
    }
    public void TakeDamage(float damage)
    {
        // ✅ BLOCK DAMAGE DURING I-FRAMES
        if (framed)
            return;
        health -= damage;
        healthText.text = health + "/" + maxHealth;
        StartCoroutine(IFrames());
        if (health <= maxHealth / 2) Warning.SetActive(true);
        if (health <= 0)
        {
            health = 0;


            dead = true;
            Die();   // ✅ IMPORTANT
        }

    }
    private System.Collections.IEnumerator IFrames()
    {
        framed = true;
        playerHurtBox.SetActive(false);

        SpriteRenderer sr = playerSprite;

        float timer = 0;
        while (timer < iFramesDuration)
        {
            sr.enabled = false;
            yield return new WaitForSeconds(0.1f);

            sr.enabled = true;
            yield return new WaitForSeconds(0.1f);

            timer += 0.2f;
        }

        playerHurtBox.SetActive(true);
        framed = false;
    }

    void Die()
    {
        Debug.Log("PLAYER DIED!");

        // Stop movement
        var move = FindFirstObjectByType<Playermovment>();
        if (move) move.enabled = false;

        // Disable hurtbox
        playerHurtBox.SetActive(false);

        // Stop i-frames if active
        framed = false;

    }
    private System.Collections.IEnumerator ShowFlash(string message)
    {
        flashText.text = message;
        flashText.gameObject.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        flashText.gameObject.SetActive(false);
    }

    public void updatehealth()
    {       
        maxHealth = stats.hp;
        healthText.text = health + "/" + maxHealth;
    }
}
