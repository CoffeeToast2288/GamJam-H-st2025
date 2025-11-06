using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Playermovment : MonoBehaviour
{
    
    [Header("Refrences")]
    public PlayerStats stats;
    public PlayerAttack attack;

    [Header("General veriables")]
    public float speed = 10f;
    private Rigidbody2D rb;
    private Vector2 input;
    Vector2 mousepos;

     public float dashtime;
    public float totaldashtime = 1f;
    public float dashspeed = 20f;
    public float dashcooldown;
    public float totaldashcooldown = 5f;
    public float DashCharges;
    public float MaxDashCharges; 
    public bool walking_sounds_playing;
    public AudioSource[] walking_sounds;
    public AudioSource current_walking_sound;
    public int current_walking_sound_val;

    [Header("Upgrade Stuff")]
    public bool dashattack = false;
    public bool dashtraile = false;
    public Transform dashstart;
    public Transform dashend;

    
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        

        input.x = Input.GetAxisRaw("Horizontal");
        input.y = Input.GetAxisRaw("Vertical");

        input.Normalize();//Normalises the diagonal inputs so that they arent faster then normal 

        lookattmous();

        if (dashtime > 0)
        {
            dashtime -= Time.deltaTime;
        }


        
        if (input.y!=0 && !walking_sounds_playing|| input.x != 0 && !walking_sounds_playing)
        {
            StartCoroutine(walking_sounds_play());
            walking_sounds_playing = true;

        }
        else if (input.y == 0 && input.x == 0 && walking_sounds_playing)
        {

            walking_sounds_playing = false;
        }
        

    }

    private void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space) && dashcooldown == 0)
        {
            dashtime = totaldashtime;
            rb.linearVelocity = input * dashspeed;
            dashcooldown = totaldashcooldown;
            dashstart.position = rb.position;
            if(dashattack == true)
            {

                attack.dashattack = true;

            }

        }
        else if (dashtime <= 0)
        {
            rb.linearVelocity = input * speed;
            dashtime = 0;
            dashend.position = rb.position;
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
