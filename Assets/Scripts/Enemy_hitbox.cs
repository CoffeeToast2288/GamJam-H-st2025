using UnityEngine;

public class Enemy_hitbox : MonoBehaviour
{
    Enemy_Script enemy;
    float damage;

    private void Update()
    {
        damage = enemy.damage;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerHealth player = other.GetComponent<PlayerHealth>();
            player.Damage(damage);
        }
    }
}
