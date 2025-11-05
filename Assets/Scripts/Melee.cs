using UnityEngine;

public class Melee : MonoBehaviour
{
    public PlayerStats stats;
    float damage = 1;
    
    void update()
    {
      


    }
    
    public void OnCollisionEnter2D(Collider2D collision)
    {
        Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
        if(enemy != null)
        {


            enemy.TakeDamage(damage);

        }
    }
}
