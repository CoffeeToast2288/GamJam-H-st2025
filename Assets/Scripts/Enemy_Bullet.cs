using UnityEngine;

public class Enemy_Bullet : MonoBehaviour
{
    [Header("Enemy Bullet Settings")]
    public float speed = 6.5f;
    public float lifetime = 7.5f;
    public float damage = 2f;
    public string targetTag = "Player"; // who this bullet can hit

    void Start()
    {
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
            //var enemy = collision.GetComponent<Enemy_Script>();
            //if (enemy != null)
            {
                //enemy.TakeDamage(damage);
            }

            // Destroy bullet after hitting something
            Destroy(gameObject);
        }
    }
}
