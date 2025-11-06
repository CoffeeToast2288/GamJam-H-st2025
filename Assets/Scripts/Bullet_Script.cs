using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    public PlayerStats player;
    public float damage;
    public string targetTag = "Enemy";

    [Header("Upgrade Settings")]
    public bool piercing = false;

    [Tooltip("Enable explosion effect + AoE damage")]
    public bool explosionUpgrade = false;

    public GameObject explosionPrefab;  // Visual effect
    public float explosionRadius = 2f;  // Area of effect radius
    public float explosionDamageMultiplier = 0.5f; // half damage by default
    public bool explosionEnabled = false;
    public float radius = 1f;

    void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerStats>();

        damage = player.damage;

        Destroy(gameObject, lifetime);
    }

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(targetTag))
        {
            Enemy_Script enemy = collision.GetComponent<Enemy_Script>();
            if (enemy != null)
                enemy.TakeDamage(damage);

            // ✅ Explosion logic
            if (explosionUpgrade)
            {
                DoExplosion();
            }

            // ✅ Only destroy if not piercing
            if (!piercing)
            {
                Destroy(gameObject);
            }
        }
    }

    private void DoExplosion()
    {
        // ✅ Spawn explosion and scale it to radius
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            float scale = explosionRadius * 2f;
            explosion.transform.localScale = new Vector3(scale, scale, 1f);
        }

        // ✅ Damage enemies in radius
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, explosionRadius);

        foreach (Collider2D hit in hits)
        {
            Enemy_Script enemy = hit.GetComponent<Enemy_Script>();
            if (enemy != null)
            {
                float aoeDamage = damage * explosionDamageMultiplier;
                enemy.TakeDamage(aoeDamage);
            }
        }
    }


    public void damageupdate()
    {
        damage = player.damage;
    }

    private void OnDrawGizmosSelected()
    {
        // Shows explosion radius in the editor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
