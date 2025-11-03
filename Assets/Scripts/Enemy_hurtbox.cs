using UnityEngine;

public class Enemy_hurtbox : MonoBehaviour
{
    public Enemy_Script enemy; // assign parent in inspector

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Example: check for player attacks
        if (other.CompareTag("PlayerAttack"))
        {
            enemy.TakeDamage(10f);
        }
    }
}
