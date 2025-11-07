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
    public float explosionDamageMultiplier = 0.5f;

    [Header("Piercing")]
    public bool piercing = false;

    void Start()
    {
        if (player == null)
            player = FindFirstObjectByType<PlayerStats>(); // Finds player - Isac

        damage = player.damage; // Gets the damage stat from player stats - Isac

        Destroy(gameObject, lifetime); //Destroyes bullet once liftime is up - Isac 
    } 

    void Update()
    {
        transform.Translate(Vector2.up * speed * Time.deltaTime);
       
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (!col.CompareTag(targetTag)) return;

        Enemy_Script enemy = col.GetComponent<Enemy_Script>();

        if (enemy != null)
            enemy.TakeDamage(damage);

        if (explosionEnabled)
            DoExplosion();

        if (!piercing)
        {
            Destroy(gameObject);
        }
    }

    void DoExplosion()
    {
        if (explosionPrefab != null)
        {
            GameObject boom = Instantiate(explosionPrefab, transform.position, Quaternion.identity);
            boom.transform.localScale = Vector3.one * (radius * 2f);
        }

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius);

        foreach (Collider2D h in hits)
        {
            Enemy_Script e = h.GetComponent<Enemy_Script>();
            if (e != null)
            {
                float aoeDamage = damage * explosionDamageMultiplier;
                e.TakeDamage(aoeDamage);
            }
        }
    }

    public void damageupdate()
    {
        damage = player.damage;
    }
}
