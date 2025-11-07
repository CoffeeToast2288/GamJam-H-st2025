using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;

public class Playermovment : MonoBehaviour
{
    //  Writen by Nino unles specified otherwhise in anotation
    
    [Header("Refrences")] //Refrenc player stats and attack scripts for future use 
    public PlayerStats stats;
    public PlayerAttack attack;

    [Header("General veriables")] 
    public float speed = 10f; // how fast is the player 
    private Rigidbody2D rb; // ridgid body 
    private Vector2 input; //Vecotor to determin witch direction the player will move in.
    Vector2 mousepos; // Vector to determin wher the cursor is.

    [Header("Dash")]
     public float dashtime; // how long have you bean dashing
    public float totaldashtime = 1f; // for how long should the dash last
    public float dashspeed = 20f; // how long is the dash
    public float dashcooldown; // how long od you have left on your dashcharges cooldown 
    public float totaldashcooldown = 5f; // how long the total coldown of the dash is 
    public float DashCharges; // how many dash charges do you curently have
    public float MaxDashCharges; // what is the maximum amount of dashcharges you kan have
    public bool walking_sounds_playing; // is the walking sound playing?
    public AudioSource[] walking_sounds; // walking sound
    public AudioSource current_walking_sound; // curent walking sound so it kan sycal inbetwen them
    public int current_walking_sound_val; // in for the sound loop

    [Header("Upgrade Stuff")] // variables for wether certain upgrades should be enabled or not 
    public bool dashattack = false; 
   
   
    // Animator stuff - Benjamin
    public Animator animator, bar_1,bar_2;
    public string[] animations;
    public bool is_walking;

    

    
    public void speedupdate() // Update variables to be in acordance with stat upgrades 
    {
        speed = stats.speed;
        totaldashcooldown = 5f;
        StartCoroutine(dash_charge_cooldown());//Benjamin
        if (totaldashcooldown == 5f) // Some math for the dash cooldwon
        {
            totaldashcooldown /= dashcooldown;

        }
        MaxDashCharges = stats.dash_chargers;

    }
   
   
    //

    void Start()
    {
        rb = GetComponent<Rigidbody2D>(); // Get the RB2D
        animator.Play(animations[0]); // Benjamin
        
    }

    // Update is called once per frame
    void Update()
    {
        

        input.x = Input.GetAxisRaw("Horizontal"); // Get horizontal imput
        input.y = Input.GetAxisRaw("Vertical"); // Get vertical imput 

        input.Normalize();//Normalises the diagonal inputs so that they arent faster then normal 

        lookattmous(); // call the function to make the player rotate towards the mouse 

        if (dashtime > 0) // Start dashtimer
        {
            dashtime -= Time.deltaTime;
        }


        
        if (input.y!=0 && !walking_sounds_playing|| input.x != 0 && !walking_sounds_playing) // walk soun - Benjamin
        {
            StartCoroutine(walking_sounds_play());
            walking_sounds_playing = true;

        }
        else if (input.y == 0 && input.x == 0 && walking_sounds_playing) // Stops walking sound from playing when it shouldent - Benjamin
        {

            walking_sounds_playing = false;
        }
        

    }

    public IEnumerator player_animations_reset() // Benjamin
    {
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("clip length " + clipLength);
        yield return new WaitForSeconds(clipLength);
        animator.CrossFade(animations[0], 0.2f);
    }

    public IEnumerator dash_charge_cooldown()
    {

        yield return new WaitForSeconds(dashcooldown);
        if (DashCharges< MaxDashCharges)
        {
            DashCharges++;
            switch (DashCharges)
            {
                case 1:
                    bar_1.Play("dash charge 0-1");

                    break;
                case 2:
                    bar_1.Play("1-2");

                    break;
                case 3:
                    bar_2.Play("dash charge 2-3");

                    break;
                case 4:
                    bar_2.Play("dash charge 3-4");

                    break;

            }
            StartCoroutine(dash_charge_cooldown());
        }
        
    }


    private void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space) && DashCharges > 0 ) // checs so that you have dashcharges and the starts the dash scipt stuff if you do so and pres space 
        {
            DashCharges--;
            animator.CrossFade(animations[4], 0.2f);
            StartCoroutine(player_animations_reset());
            StartCoroutine(dash_charge_cooldown());
            switch (DashCharges)
            {
                case 0:
                    bar_1.Play("1-0 dash charges");

                    break;
                case 1:
                    bar_1.Play("2-1 dash charges");

                    break;
                case 2:
                    bar_2.Play("3-2");

                    break;
                case 3:
                    bar_2.Play("4-3");

                    break;
                
            }
            


            dashtime = totaldashtime; // set dahstime to total dashtime 
            rb.linearVelocity = input * dashspeed; // sets linear velocity to be = to dashspeed in the direction of your imput 
            dashcooldown = totaldashcooldown; // sets dashcooldwon to total cooldown
          
            if(dashattack == true) // If you have unlocked dashattack, do the dash attack withc is in player attack script 
            {

                attack.dashattack = true;

            }

        }
        else if (dashtime <= 0) // stops the dash if your dashtime runs out and starts the cooldwon.
        {
            rb.linearVelocity = input * speed;
            dashtime = 0;
           
            if (dashcooldown > 0)
            {
                dashcooldown -= Time.deltaTime;

            }
            if (dashcooldown < 0)
            {
                dashcooldown = 0;
            }
        }
        
    }
    
    private void lookattmous()// Determins the postion of the mouse in World point and uses the transforms of wher the player is facting to rotate it to face the camera 
    {
        mousepos = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        transform.up = (Vector2)mousepos - new Vector2(transform.position.x, transform.position.y);
    }
     
     IEnumerator walking_sounds_play()
    {
        current_walking_sound = walking_sounds[current_walking_sound_val];
        current_walking_sound.Play();
        if (current_walking_sound_val != 2)
        {
            current_walking_sound_val += 1;
        }
        else
        {
            current_walking_sound_val = 0;
        }
        yield return new WaitForSeconds(1f);

        if (walking_sounds_playing)
        {
            StartCoroutine(walking_sounds_play());

        }


    }

}
