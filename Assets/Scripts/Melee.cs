using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class Melee : MonoBehaviour
{
    public PlayerStats stats;
    float damage = 1;
    
    void update()
    {
      


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
