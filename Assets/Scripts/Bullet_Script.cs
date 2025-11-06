using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    public PlayerStats player;
    public float damage;
    public string targetTag = "Enemy"; // who this bullet can hit

    public GameObject explosion;
    void Start()
    {
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerStats>();
        }
        damage = player.damage;
        // Automatically destroy after 'lifetime' seconds
        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        // Move bullet forward constantly (local up direction)
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // Only damage valid targets
        if (collision.CompareTag(targetTag))
        {
            // Example: deal damage if target has a TakeDamage() method
            var enemy = collision.GetComponent<Enemy_Script>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
            }

            Instantiate(explosion, transform.position, transform.rotation);
            // Destroy bullet after hitting something
            Destroy(gameObject);
        }
    }
}
