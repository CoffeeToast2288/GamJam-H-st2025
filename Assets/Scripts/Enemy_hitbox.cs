using UnityEngine;

public class Enemy_hitbox : MonoBehaviour
{
    [Header("References")]
    public Enemy_Script enemy;        // Reference to the parent enemy
    public PlayerHealth player;       // Reference to the player’s health script

    private float damage;             // Cached damage amount

    void Start()
    {
        // Get Enemy_Script from parent (assuming this hitbox is a child of the enemy)
        enemy = GetComponentInParent<Enemy_Script>();

        // Get the damage value from the enemy
        if (enemy != null)
            damage = enemy.damage;
        else
            Debug.LogWarning($"{name} has no Enemy_Script parent!");

        // Optionally, find the player automatically if not set in Inspector
        if (player == null)
            player = FindFirstObjectByType<PlayerHealth>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && player != null)
        {
            player.TakeDamage(damage);
            Debug.Log($"Enemy {enemy.name} hit player for {damage} damage!");
        }
    }
}
