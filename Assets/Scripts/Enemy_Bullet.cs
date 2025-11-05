using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [Header("Enemy Bullet Settings")]
    public float speed = 6.5f;              // How fast the bullet travels
    public float lifetime = 7.5f;           // How long before it despawns
    public float damage; // Assigned by the enemy on spawn

    [Header("References")]
    public Enemy_Script enemy;              // Reference to the parent enemy
    public PlayerHealth player;             // Player health script reference
    public string targetTag = "Player";     // Tag to identify the player target

    void Start()
    {
        // Get Enemy_Script from parent (assuming this hitbox is a child of the enemy)
        enemy = GetComponentInParent<Enemy_Script>();

        // Get the damage value from the enemy
        if (enemy != null)
            damage = enemy.damage;

        // Automatically destroy the bullet after its lifetime expires
        Destroy(gameObject, lifetime);

        // Automatically find the player if not manually assigned
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerHealth>();
        }
    }

    void Update()
    {
        // Move bullet forward in its local "up" direction every frame
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {

            player.TakeDamage(damage);
            Debug.Log($"Enemy bullet hit player for {damage} damage!");
            
            Destroy(gameObject);
        }
    }
}
