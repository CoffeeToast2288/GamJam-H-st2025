using UnityEngine;
using TMPro;

public class HealthPack : MonoBehaviour
{
    [Header("Heal Settings")]
    public float healAmount = 10f;           // How much HP to restore
    public string playerTag = "Player";      // Player tag
    public float magnetRange = 6f;           // Distance at which the pack starts moving
    public float magnetSpeed = 5f;           // How fast it moves toward the player
    public PlayerHealth player;


    private Transform playerTransform;
    public void Start()
    {
        // Automatically find the player if not manually assigned
        if (player == null)
        {
            player = FindFirstObjectByType<PlayerHealth>();
        }
    }

    void Update()
    {
        if (playerTransform == null) return;

        // Calculate distance to player
        float distance = Vector2.Distance(transform.position, playerTransform.position);

        // Magnetize if within range
        if (distance <= magnetRange)
        {
            Vector2 newPos = Vector2.MoveTowards(transform.position, playerTransform.position, magnetSpeed * Time.deltaTime);
            transform.position = newPos;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(playerTag))
        {
            // Cache player reference
            playerTransform = other.transform;
            player.Heal(healAmount);



            // Destroy the health pack after use
            Destroy(gameObject);
        }
    }


}
