using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Melee : MonoBehaviour
{
    PlayerStats Stats;
    float damage = 1;
    
    void update()
    {
        damage += Stats.damage;


    }

    private void OnCollisionEnter2D(Collider2D collision)
    {
        Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
        if(enemy != null)
        {


            enemy.TakeDamage(damage);

        }
    }
}
