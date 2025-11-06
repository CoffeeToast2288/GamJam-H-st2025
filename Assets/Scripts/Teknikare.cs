using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Tekniikare : MonoBehaviour
{
    public Collider2D aoura;
    public Enemy_Script enemy;
    float damage = 1;
    public float Cooldown;
    public float Cooldownmax;
    public bool unlocked = false;
    private void OnCollisiderStay2D(Collider2D collision)
    {
        if(enemy != null && unlocked == true)
        {
            Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
            if (Cooldown >= 0)
            {
                enemy.TakeDamage(damage);
                Cooldown = Cooldownmax;
                startcooldown();

            }


        }
       
       
       
    }
    void startcooldown()
    {
        if(Cooldown > 0)
        {
            Cooldown -= Time.deltaTime;
        }
        if(Cooldown < 0)
        {
            Cooldown = 0;

        }
        
    }
    
}
