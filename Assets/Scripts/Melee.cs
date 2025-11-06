using UnityEngine;
using UnityEngine.Rendering;

public class Melee : MonoBehaviour
{
    public PlayerStats stats;
    float damage = 1;
    float critdamage;
    int crit;
    bool kancrit = false;
    int critchanse; // chans to crit 1-100%
    private void Start()
    {
        damageupdate();
    }
    public void Awake()
    {
        
    }

    public void OnCollisiderEnter2D(Collider2D collision)
    {
        Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
        if(enemy != null)
        {
            crit = Random.Range(1, 101); // randomiser for crit
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

    public void damageupdate()
    {
        damage = stats.damage;
        critdamage = (float)(damage * 1.5);
        critchanse = stats.dash_chargers * 25;
    }
}
