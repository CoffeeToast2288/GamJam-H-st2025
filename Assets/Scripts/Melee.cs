using UnityEngine;
using UnityEngine.Rendering;

public class Melee : MonoBehaviour
{
    public PlayerStats stats;
    float damage = 1;
    float critdamage;
    int crit = Random.Range(1, 101);
    bool kancrit = false;
    int critchanse;
    private void Start()
    {
        damageupdate();
    }
    void update()
    {
      


    }
    
    public void OnCollisionEnter2D(Collider2D collision)
    {
        Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
        if(enemy != null)
        {
            if (kancrit && crit <= critchanse)
            {

                enemy.TakeDamage(critdamage);

            }
            else 
            {
                enemy.TakeDamage(damage);

            }

            

        }
    }

    void damageupdate()
    {
        damage += stats.damage;
        critdamage = (float)(damage * 1.5);
        critchanse = stats.dash_chargers * 25;
    }
}
