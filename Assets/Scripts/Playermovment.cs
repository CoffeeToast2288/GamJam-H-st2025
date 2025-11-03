using UnityEngine;
using UnityEngine.Rendering;

public class Playermovment : MonoBehaviour
{
    public float speed = 10f;
    private Rigidbody2D rb;
    private Vector2 input;
    Vector2 mousepos;

     public float dashtime;
    public float totaldashtime = 1f;
    public float dashspeed = 20f;
    public float dashcooldown;
    public float totaldashcooldown = 5f;
    
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
        
    }

    private void FixedUpdate()
    {

        if (Input.GetKeyDown(KeyCode.Space) && dashcooldown == 0)
        {
            dashtime = totaldashtime;
            rb.linearVelocity = input * dashspeed;
            dashcooldown = totaldashcooldown;

        }
        else if (dashtime <= 0)
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
     
    
}
