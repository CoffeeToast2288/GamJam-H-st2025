using UnityEngine;
using TMPro;
using static UnityEngine.Rendering.DebugUI;
using System.Collections;
using JetBrains.Annotations;

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
    public bool is_below_7, is_below_10, is_taking_damage, is_healing;
    public Sprite[] numbers;
    public string[] animtions_heal, animations_hurt;
    public SpriteRenderer card_1, card_2;
    public int damage_animation_charges, heal_animation_charges;

    public Animator player_animator;
    public string[] play_animations;
    public AudioSource heal;

    private void Start()
    {
        health_card_change();

    }

    void Update()
    {
        healthText.text = Hp + "/" + Hp_max + " HP";

        if (Hp > Hp_max)
            Hp = Hp_max;

    if (Input.GetKeyDown(KeyCode.I))
        {
            TakeDamage(2);
           
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            Heal(2);
            Debug.Log("did the thing");
        }

        if (damage_animation_charges > 0 && !is_taking_damage)
        {
            is_taking_damage = true;
            Debug.Log($"⏩ Continuing animation chain. Charges left: {damage_animation_charges}" + " is looping");
            StartCoroutine(health_charge_animation_damage());
        }

    }

    public void Heal(float healing)
    {
        if (Hp < Hp_max)
        {
            check_health();
            Hp += healing;
            heal.Play();
            if (flashText != null)
                StartCoroutine(ShowFlash($"+{healing} HP!"));

            
            int val = Mathf.FloorToInt(healing);
            heal_animation_charges += val;


            if (is_below_7)
            {

                StartCoroutine(health_charge_animation_damage());
                health_card_change();
            }
            else
            {
                health_card_change();
            }

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

    public IEnumerator health_charge_animation_heal()
    {
        int val = Mathf.FloorToInt(Hp - heal_animation_charges);
       
      

        Debug.Log(val);
      
        if (val < 7)
        {
            animator.Play(animtions_heal[val]);
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            float clipLength = clipInfo[0].clip.length;
            Debug.Log("clip length " + clipLength);
            yield return new WaitForSeconds(clipLength);
        }
        else
        {

            yield return new WaitForSeconds(0.1f);
        }
        if (heal_animation_charges >= 0)
        {
            heal_animation_charges--;

        }
        
        if (heal_animation_charges > 0)
        {
            Debug.Log($"⏩ Continuing animation chain. Charges left: {heal_animation_charges}" + " is looping");
            StartCoroutine(health_charge_animation_damage());
        }
    }

    public IEnumerator health_charge_animation_damage()
    {
        
        
        int val = Mathf.FloorToInt(Hp + damage_animation_charges);

        Debug.Log(val);
      
        
       
        if (val < 7)
        {
            Debug.Log("animation");
            animator.Play(animations_hurt[val]);
            AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
            float clipLength = clipInfo[0].clip.length;
            Debug.Log("clip length " + clipLength);
            yield return new WaitForSeconds(clipLength);
        }
        else
        {
            Debug.Log("confusion");

            yield return new WaitForSeconds(0.1f);
        }
       
        is_taking_damage = false;
        damage_animation_charges -= 1;



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

    public IEnumerator player_animations_reset()
    {
        AnimatorClipInfo[] clipInfo = player_animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("clip length " + clipLength);
        yield return new WaitForSeconds(clipLength);
        player_animator.CrossFade(play_animations[0],0.2f);
    }

    public void TakeDamage(float damage)
    {
        // ✅ BLOCK DAMAGE DURING I-FRAMES
        if (framed)
            return;

        player_animator.CrossFade(play_animations[1], 0.2f);
        StartCoroutine(player_animations_reset());

        Hp -= damage;
        check_health();
        int val = Mathf.FloorToInt(damage);
        damage_animation_charges += val;
        
       
        if (is_below_7)
        {
            
            StartCoroutine(health_charge_animation_damage());
            health_card_change();
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
