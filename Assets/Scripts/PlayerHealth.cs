using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public float Hp;
    public float Hp_max;
    PlayerStats stats;
    public bool dead = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Hp > Hp_max)
        {

            Hp = Hp_max;

        }
        Hp_max += stats.hp;
        if(Hp<= 0)
        {
            dead = true;
        }
    }


    public void Damage(float damage)
    {
        Hp -= damage;

    }
}
