using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Playermovment : MonoBehaviour
{
    //  Writen by Nino unles specified otherwhise in anotation
    
    [Header("Refrences")] //Refrenc player stats and attack scripts for future use 
    public PlayerStats stats;
    public PlayerAttack attack;
    public Health_Script deathLogic;

    [Header("General veriables")] 
    public float speed = 10f; // how fast is the player 
    private Rigidbody2D rb; // ridgid body 
    private Vector2 input; //Vecotor to determin witch direction the player will move in.
    Vector2 mousepos; // Vector to determin wher the cursor is.

    [Header("Dash")]
    public float dashDuration = 0.2f;
    public float dashSpeed = 20f;

    private float dashTimeRemaining;
    private float dashCooldownTimer;

    public float baseDashCooldown = 15f;
    private float totalDashCooldown;

    public int currentDashCharges;
    public int maxDashCharges;

    private bool isDashing;
    public AudioSource current_walking_sound; // curent walking sound so it kan sycal inbetwen them
    public int current_walking_sound_val; // in for the sound loop
    public bool walkingSoundsPlaying; // is the walking sound playing?

    [Header("Upgrade Stuff")] // variables for wether certain upgrades should be enabled or not 
    public bool dashattack = false;


    // Animator stuff - Benjamin
    public Animator animator;
    public string[] animations;
    public bool is_walking;
    public audiocontroler audiocontroler;




    public void speedupdate()
    {
        speed = stats.speed;

        totalDashCooldown = baseDashCooldown / stats.dash_coldown_reduction;

        maxDashCharges = Mathf.RoundToInt(stats.dash_chargers);

        currentDashCharges = Mathf.Clamp(currentDashCharges, 0, maxDashCharges);
    }


    //

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the RB2D

        
    }

    // Update is called once per frame
    void Update()
    {
        HandleMovementInput();
        HandleDashInput();
        HandleDashRecharge();
    }
    void HandleMovementInput()
    {
        if (!deathLogic.dead)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");
            if (input.y != 0 && input.x != 0 || input.x != 0 || input.y != 0) 
            {
                animator.SetBool("isRunning", true);
            
            }
            else
            {
                animator.SetBool("isRunning", false); 
            }     

            input.Normalize();

            lookattmous();

            // Start walking sound if moving and not already playing
            if ((input.x != 0 || input.y != 0) && !walkingSoundsPlaying)
            {
                walkingSoundsPlaying = true;
                StartCoroutine(WalkingSoundsPlay());
            }

        }
    }
    void HandleDashInput()
    {
        if (Input.GetKeyDown(KeyCode.Space) && currentDashCharges > 0 && !isDashing && !deathLogic.dead)
        {
            StartDash();
        }
    }
    void StartDash()
    {
        isDashing = true;
        dashTimeRemaining = dashDuration;

        currentDashCharges--;

        rb.linearVelocity = input * dashSpeed;
    }

    public IEnumerator player_animations_reset() // Benjamin
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("clip length " + clipLength);
        yield return new WaitForSeconds(clipLength);
        animator.CrossFade(animations[0], 0.2f);
    }

    void HandleDashRecharge()
    {
        if (currentDashCharges < maxDashCharges)
        {
            dashCooldownTimer += Time.deltaTime;

            if (dashCooldownTimer >= totalDashCooldown)
            {
                currentDashCharges++;
                dashCooldownTimer = 0f;
            }
        }
    }


    void FixedUpdate()
    {
        if (isDashing)
        {
            dashTimeRemaining -= Time.fixedDeltaTime;

            if (dashTimeRemaining <= 0f)
            {
                isDashing = false;
            }
        }
        else
        {
            rb.linearVelocity = input * speed;
        }
    }

    private void lookattmous()// Determins the postion of the mouse in World point and uses the transforms of wher the player is facting to rotate it to face the camera 
    {
        mousepos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector2)mousepos - new Vector2(transform.position.x, transform.position.y);
    }

    IEnumerator WalkingSoundsPlay()
    {
        while (input.x != 0 || input.y != 0)  // Loopar så länge spelaren rör sig
        {
            current_walking_sound = audiocontroler.audio_list[current_walking_sound_val];
            current_walking_sound.Play();

            if (current_walking_sound_val != 5)
                current_walking_sound_val += 1;
            else
                current_walking_sound_val = 3;

            yield return new WaitForSeconds(1f);
        }

        // Stop loop när spelaren slutar gå
        walkingSoundsPlaying = false;
    }


}
