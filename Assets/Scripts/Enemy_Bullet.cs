using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    // Isaks code!!!!
    [Header("Enemy Bullet Settings")]
    public float speed = 6.5f;
    public float lifetime = 7.5f;
    public float damage; // damage is set by the enemy when spawned

    public PlayerHealth player;
    public string targetTag = "Player";

    void Start()
    {
        // Auto-destroy
        Destroy(gameObject, lifetime);

        // Find player if not assigned
        if (player == null)
            player = FindFirstObjectByType<PlayerHealth>();
    }

    void Update()
    {
        // Move forward in local up direction
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    // ✅ This function is called by the ENEMY when bullet is created
    public void SetDamage(float dmg)
    {
        damage = dmg;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            player.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
