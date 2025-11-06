using UnityEngine;

public class Bullet_Script : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 10f;
    public float lifetime = 5f;
    public PlayerStats player;
    public float damage;
    public string targetTag = "Enemy";

    [Header("Explosion Upgrade")]
    public bool explosionEnabled = false;      // ✅ PlayerAttack sets this
    public float radius = 1f;                  // ✅ PlayerAttack sets this
    public GameObject explosionPrefab;         // Assign your explosion prefab

    [Header("Piercing")]
    public bool piercing = false;

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
        if (!collision.CompareTag(targetTag))
            return;

        Enemy_Script enemy = collision.GetComponent<Enemy_Script>();

        if (enemy != null)
        {
            enemy.TakeDamage(damage);
        }

        // ✅ Explosion logic
        if (explosionEnabled)
        {
            GameObject boom = Instantiate(explosionPrefab, transform.position, Quaternion.identity);

            // ✅ Scale explosion visual
            boom.transform.localScale = new Vector3(radius, radius, 1);

            explosion exp = boom.GetComponent<explosion>();
            if (exp != null)
            {
                exp.damage = damage;   // explosion damage = bullet damage
                exp.radius = radius;
            }
        }

        // ✅ Piercing = bullet keeps going
        if (!piercing)
            Destroy(gameObject);
    }

    public void damageupdate()
    {
        damage = player.damage;
    }
}
