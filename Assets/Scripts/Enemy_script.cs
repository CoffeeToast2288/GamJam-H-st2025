using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    // ===== ENEMY SETTINGS =====
    [Header("Enemy Settings")]
    public float moveSpeed;               // Movement speed
    public float health;                  // Enemy HP
    public float damage;                  // Base damage dealt to player
    public float stopDistance;            // Distance to stop before reaching the player
    public float attackCooldown = 0.5f;   // Delay between melee attacks
    public float shootCooldown = 1f;      // Delay between ranged shots
    public float attackRange = 1.2f;      // Range for melee attacks
    public float shootrange = 6f;         // Range for shooting
    public float lungeSpeed = 10f;        // Speed when lunging
    public float lungeDuration = 0.3f;    // How long the lunge lasts
    public float lungeCooldown = 1.5f;    // Time before the next lunge
    public float chargeTime = 0.5f;       // Charging time before lunging
    public float lungeRange = 8f;         // Distance within which the enemy can lunge
    public float healthPackDropChance = 0.1f;  // 10% chance
    public GameObject hitboxObject;       // Object used to apply melee hit detection

    // ===== REFERENCES =====
    [Header("References")]
    public Transform player;              // Player reference
    public GameObject tankita;            // Used to scale tanky enemies visually
    private Rigidbody2D rb;               // Cached Rigidbody for movement
    public Transform spawnPos;            // Position to spawn bullets from
    public GameObject enemyBullet;        // Bullet prefab
    public SpriteRenderer spriteRenderer; // Sprite for this enemy
    public TrailRenderer trailRenderer;   // Trail used for lunging enemies
    public GameObject healthPackPrefab;


    // ===== TYPE FLAGS =====
    [Header("Type Flags")]
    public bool shooty;                   // Marks this enemy as a shooter
    public bool hitty;                    // Marks this enemy as melee
    public bool tanky;                    // Marks this enemy as tank type
    public bool lungie;                   // Marks this enemy as lunging type

    // ===== ELITE SETTINGS =====
    [Header("Elite Settings")]
    [Tooltip("Whether this enemy is an elite variant")]
    public bool isElite = false;          // If true, enemy stats are boosted

    [Tooltip("How much stronger elites are (stat multiplier)")]
    public float eliteMultiplier = 1.5f;  // How much to multiply health & damage by

    // ===== CONTROL FLAGS =====
    private bool isDead = false;          // Used to stop actions when dead
    private bool canAttack = true;        // Melee attack cooldown control
    private bool canshoot = true;         // Shooting cooldown control
    private bool isShooty = false;        // Internal state for ranged type
    private bool isLungie = false;        // Internal state for lunging type
    private bool isLunging = false;       // True while lunging
    private bool canLunge = true;         // Cooldown control for lunging

    // ===== INITIALIZATION =====
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Automatically find player if not set in Inspector
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogWarning("Enemy_Script could not find player in scene!");
            }
        }

        if (trailRenderer != null)
            trailRenderer.emitting = false;

        // Automatically assign behavior based on type flag
        if (hitty)
        {
            Hitty();
        }
        else if (shooty)
        {
            Shooty();
        }
        else if (tanky)
        {
            Tanky();
        }
        else if (lungie)
        {
            Lungie();
        }
    }

    // ===== MAIN UPDATE LOOP =====
    void Update()
    {
        // Stop all logic if dead or player missing
        if (isDead || player == null) return;

        // Calculate distance to player
        float distance = Vector2.Distance(transform.position, player.position);

        // If in range and allowed, perform a lunge attack
        if (distance <= lungeRange && canLunge && isLungie)
        {
            StartCoroutine(LungeAttack());
        }
        // Otherwise move toward player until close enough to stop
        else if (distance > stopDistance && !isLunging)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
        // If close enough, perform melee attack
        else if (distance <= attackRange)
        {
            TryAttack();
        }

        // Handle ranged shooting
        if (distance <= shootrange && isShooty)
        {
            TryFire();
            RotateTowardsPlayer();
        }
    }

    // ===== ENEMY TYPE SETUPS =====
    // Adjust stats depending on enemy type
    public void Hitty()
    {
        moveSpeed += 3f;
        damage += 1f;
        health += 3f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
    }

    public void Shooty()
    {
        moveSpeed = 3f;
        damage += 1f;
        health += 2f;
        stopDistance = 6f;
        shootrange += 10f;
        attackRange = 1.2f;
        isShooty = true;
    }

    public void Tanky()
    {
        // Make it visually bigger
        tankita.transform.localScale += new Vector3(1.2f, 1.2f, 1.2f);
        moveSpeed += 1.5f;
        damage += 2f;
        health += 6f;
        stopDistance = 1.8f;
        attackRange = 1.9f;
    }

    public void Lungie()
    {
        moveSpeed = 4f;
        damage += 1f;
        health += 2f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
        isLungie = true;
    }

    // ===== ELITE HANDLING =====
    public void SetElite(bool eliteStatus)
    {
        isElite = eliteStatus;

        if (isElite)
        {
            // Boost stats by multiplier
            health *= eliteMultiplier;
            damage *= eliteMultiplier;

            // Example: could add red outline here later if desired
        }
    }

    // ===== MOVEMENT =====
    void MoveTowardsPlayer()
    {
        // Move directly toward player until within stopDistance
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        // Rotate enemy to face player
        Vector2 direction = (player.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    // ===== MELEE ATTACK =====
    void TryAttack()
    {
        if (!canAttack) return;
        StartCoroutine(AttackRoutine());
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        canAttack = false;

        // Enable hitbox briefly for attack
        hitboxObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        hitboxObject.SetActive(false);

        // Wait before next attack
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    // ===== RANGED ATTACK =====
    void TryFire()
    {
        if (!canshoot) return;
        StartCoroutine(ShootRoutine());
    }

    System.Collections.IEnumerator ShootRoutine()
    {
        canshoot = false;

        GameObject bulletObj = Instantiate(enemyBullet, spawnPos.position, spawnPos.rotation);

        // Pass damage to bullet
        Enemy_Bullet bullet = bulletObj.GetComponent<Enemy_Bullet>();
        if (bullet != null)
        {
            bullet.SetDamage(damage);
        }

        yield return new WaitForSeconds(shootCooldown);
        canshoot = true;
    }

    // ===== LUNGE ATTACK =====
    System.Collections.IEnumerator LungeAttack()
    {
        canLunge = false;
        isLunging = true;

        // Save starting point
        Vector3 originalPosition = transform.position;

        // Find direction toward player
        Vector2 direction = (player.position - transform.position).normalized;

        // Charge-up phase with shake
        float chargeTimer = 0f;
        while (chargeTimer < chargeTime)
        {
            chargeTimer += Time.deltaTime;
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * 0.05f);
            yield return null;
        }

        // Reset position after charge
        transform.position = originalPosition;

        // Lock target position and face it
        Vector2 lockedDirection = player.position;
        float lockedAngle = Mathf.Atan2(lockedDirection.y, lockedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lockedAngle - 90f);

        // Enable trail effect during lunge
        if (trailRenderer != null)
            trailRenderer.emitting = true;

        // Move rapidly toward player
        float lungeTimer = 0f;
        while (lungeTimer < lungeDuration)
        {
            transform.position = Vector2.MoveTowards(transform.position, lockedDirection, lungeSpeed * Time.deltaTime);
            lungeTimer += Time.deltaTime;
            yield return null;
        }

        // Stop trail and cooldown
        if (trailRenderer != null)
            trailRenderer.emitting = false;

        isLunging = false;
        yield return new WaitForSeconds(lungeCooldown);
        canLunge = true;
    }

    // ===== DAMAGE & DEATH =====
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        if (health <= 0) Die();
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;

        TryDropHealthPack();   // Attempt health pack drop

        Destroy(gameObject);         // Remove from scene
    }

    void TryDropHealthPack()
    {
        // ✅ Ensure a prefab is assigned
        if (healthPackPrefab == null)
            return;

        // ✅ Roll the chance
        float roll = Random.value;  // 0.0 → 1.0

        if (roll <= healthPackDropChance)
        {
            Instantiate(healthPackPrefab, transform.position, Quaternion.identity);
        }
    }
}
