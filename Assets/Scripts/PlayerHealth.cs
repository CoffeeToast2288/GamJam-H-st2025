using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;

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

    public Animator animator;
    public bool is_below_7, is_below_10;
    public Sprite[] numbers;
    public string[] animtions_heal, animations_hurt;
    public SpriteRenderer card_1, card_2;
    public int animation_charges;

    void Update()
    {
        healthText.text = Hp + "/" + Hp_max + " HP";

        if (Hp > Hp_max)
            Hp = Hp_max;

    if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(2);
        }

    }

    public void Heal(float healing)
    {
        if (Hp < Hp_max)
        {
            Hp += healing;

            if (flashText != null)
                StartCoroutine(ShowFlash($"+{healing} HP!"));
            check_health();


        }
    }
    public void check_health()
    {
        if (Hp < 7)
        {
            is_below_7 = true;   
            is_below_10 = true;   

        }
        else if (Hp < 10)
        {
            is_below_7 = false;
            is_below_10 = true;

        }
        else
        {
            is_below_7 = false;
            is_below_10 = false;

        }
        

    }

    public void health_charge_animation_heal()
    {
        int val = Mathf.FloorToInt(Hp);
        animator.Play(animtions_heal[val]);

    }

    public IEnumerator health_charge_animation_damage()
    {
        int val = Mathf.FloorToInt(Hp +animation_charges-1);
        animator.Play(animations_hurt[val]);
        animation_charges -= 1;
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        yield return new WaitForSeconds(clipLength);
        if (animation_charges < 0)
        {
            StartCoroutine(health_charge_animation_damage());
        }

    }

    public void health_card_change()
    {
        if (is_below_10)
        {
            int val = Mathf.FloorToInt(Hp);
            card_1.sprite = numbers[val];
            card_2.sprite = numbers[0];

        }
        else
        {int val = Mathf.FloorToInt(Hp)/10;
         int val_2 = (int)Hp - val;
            card_1.sprite = numbers[val_2];
            card_2.sprite = numbers[val];

        }


    }


    public void TakeDamage(float damage)
    {
        // ✅ BLOCK DAMAGE DURING I-FRAMES
        if (framed)
            return;

        Hp -= damage;
        int val = Mathf.FloorToInt(Hp);
        animation_charges = val;
        if (is_below_7)
        {


        }
        else
        {
            health_card_change();
        }

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
