using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    [Header("Health Settings")]
    public float Hp;
    public float Hp_max;
    public float iFramesDuration = 1f;



    [Header("References")]
    public SpriteRenderer playerSprite; //  Drag your Player Sprite object here
    public PlayerStats stats;
    public TextMeshProUGUI healthText;
    public GameObject playerHurtBox;

    private bool framed = false;

    [Header("UI Feedback")]
    public TextMeshProUGUI flashText;
    public float flashDuration = 1f;


    [Header("Upgrade + death stuff")]
    public bool dead = false;
    public bool revive = false;

    void Update()
    {
        healthText.text = Hp + "/" + Hp_max + " HP";

        if (Hp > Hp_max)
            Hp = Hp_max;
    }

    public void Heal(float healing)
    {
        if (Hp < Hp_max)
        {
            Hp += healing;

            if (flashText != null)
                StartCoroutine(ShowFlash($"+{healing} HP!"));
        }
    }

    public void TakeDamage(float damage)
    {
        // ✅ BLOCK DAMAGE DURING I-FRAMES
        if (framed)
            return;

        Hp -= damage;

        StartCoroutine(IFrames());

        if (Hp <= 0)
        {
            Hp = 0;
           if(revive == true)
            {
                revive = false;
                Hp = Hp_max / 2;
            }
            else
            {
                dead = true;
            }
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

    private System.Collections.IEnumerator ShowFlash(string message)
    {
        flashText.text = message;
        flashText.gameObject.SetActive(true);

        yield return new WaitForSeconds(flashDuration);

        flashText.gameObject.SetActive(false);
    }

   public void updatehealth()
    {
        Hp_max = stats.hp;

    }
}
