using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameObject Attack;
    public bool Isattacking = false;
    PlayerStats Stats;
    float attack_duratin = 0.3f;
    float attack_timer;
    public float colldown;
    public float colldown_max = 5;
    public bool colldown_active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        colldown_max *= Stats.attack_speed; // scales colldown with attack speed (attack sped will be a desimal)
        CheckTimer();
        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0) && colldown == 0) // attack when buton is presed and no colldown 
        {

            attacking();




        }
        if (colldown_active) //starts colldown
        {
            colldown -= Time.deltaTime;
        }
        if(colldown < 0)
        {
            colldown = 0;
        }
        if(colldown <= 0)
        {
            colldown_active = false;
           
        }
    }


    void attacking() //atack funktion
    {

        if (!Isattacking)
        {
            Attack.SetActive(true);
            Isattacking = true;
            // Put animator script hear
            colldown_active = true;
            colldown = colldown_max;
        }


    }

    void CheckTimer() // checks attack duration
    {

        if (Isattacking)
        {
            attack_timer += Time.deltaTime;
            if(attack_timer > attack_duratin)
            {
                Isattacking = false;
                Attack.SetActive(false);
                attack_timer = 0;
                

            }
        }


    }
    
}
