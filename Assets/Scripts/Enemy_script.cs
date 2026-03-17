using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    // Isak has only been here

    // ===== ENEMY SETTINGS =====
    [Header("Enemy Settings")]
    public float moveSpeed;               // Movement speed
    public float health;                  // Enemy HP
    public float damage;                  // Base damage dealt to player
    public float stopDistance;            // Distance to stop before reaching the player
    public float attackCooldown = 0.5f;   // Delay between melee attacks
    public float attackRange = 1.2f;      // Range for melee attacks
    public float shootCooldown = 1f;      // Delay between ranged shots
    public float healthPackDropChance = 0.1f;  // 10% chance
    [Header("Shooty")]
    public float shootrange = 6f;         // Range for shooting
    [Header("Lungie stats")]
    public float lungeSpeed = 10f;        // Speed when lunging
    public float lungeDuration = 0.3f;    // How long the lunge lasts
    public float lungeCooldown = 1.5f;    // Time before the next lunge
    public float chargeTime = 0.5f;       // Charging time before lunging
    public float lungeRange = 8f;         // Distance within which the enemy can lunge

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
    public GameObject enemyPrefab;
    public GameObject warningLight;


    // ===== TYPE FLAGS =====
    [Header("Type Flags")]
    public bool shooty;                   // Marks this enemy as a shooter
    public bool hitty;                    // Marks this enemy as melee
    public bool tanky;                    // Marks this enemy as tank type
    public bool lungie;                   // Marks this enemy as lunging type
    public bool smoll;                    // Marks this enemy as Smoll
    public bool crawler;
    public bool flyer;

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
    private bool isTanky = false;
    // ====== ANIMATIONS=========
    public Animator animator;
    public audiocontroler audiocontrol;

    private List<GameObject> activeEnemies = new();  // Keeps track of alive enemies in current wave


    // ===== INITIALIZATION =====
    public void Start()
    {
        GameObject audioObj = GameObject.FindGameObjectWithTag("Audiocontrol");
        if (audiocontrol == null)
        {
            audiocontrol = audioObj.GetComponent<audiocontroler>();
           
        }
        else
        {
            Debug.LogWarning("Enemy_Script could not find Audiocontrol in scene!");
        }

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

        // Automatically assign behavior based on type flag and sets the walking animation to start
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
        else if (smoll)
        {
            Smoll();
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
        if (distance <= lungeRange && canLunge && isLungie && !isLunging)
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
            animator.SetBool("Hitting", true);
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
    // Adjust stats depending on enemy type and sets thier animations
    public void Hitty()
    {
        int num = UnityEngine.Random.Range(0, 10);

        if (num > 3 && num != 6) 
        {
            moveSpeed += 3f;
            damage += 1f;
            health += 3f;
            stopDistance = 1.1f;
            attackRange = 1.2f;
            animator.SetBool("IsHitty", true);
        }
        else if (num <= 3)
        {
            moveSpeed += 1.5f;
            damage += 1f;
            health += 4f;
            stopDistance = 1.1f;
            attackRange = 1.2f;
            crawler = true;
            animator.SetBool("IsCrawling", true);
            animator.SetBool("IsHitty", false);
        }
        else if (num == 6)
        {
            moveSpeed += 4f;
            damage += 1f;
            health += 2f;
            stopDistance = 1.1f;
            attackRange = 1.2f;
            flyer = true;
            animator.SetBool("IsFlying", true);
            animator.SetBool("IsCrawling", false);
            animator.SetBool("IsHitty", false);
        }

    }

    public void Shooty()
    {
        moveSpeed = 3f;
        damage += 1f;
        health += 2f;
        stopDistance = 6f;
        shootrange += 7f;
        attackRange = 1.2f;
        isShooty = true;
        animator.SetBool("IsShooty", true);
    }

    public void Tanky()
    {
        // Make it visually bigger
        tankita.transform.localScale += new Vector3(1.1f, 1.1f, 1.1f);
        moveSpeed += 1.5f;
        damage += 2f;
        health += 6f;
        stopDistance = 1.8f;
        attackRange = 1.9f;
        animator.SetBool("IsTanky", true);
        isTanky = true;
    }

    public void Lungie()
    {
        moveSpeed = 4f;
        damage += 1f;
        health += 2f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
        isLungie = true;
        animator.SetBool("IsFast", true);
    }
    
    public void Smoll()
    {
        moveSpeed = 5f;
        damage = 1f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
        tankita.transform.localScale += new Vector3(0.6f, 0.6f, 0.6f);
        animator.SetBool("IsSmoll", true);
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

        // animation controler 
       
    }

    void RotateTowardsPlayer()
    {
        // Rotate enemy to face player
        Vector2 direction = (player.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle - 90;
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
        animator.SetBool("Hitting", false);
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

        animator.SetTrigger("Shoot");

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
        animator.CrossFade("fast lunge", 0.2f);
        audiocontrol.audio_list[7].Play();
        // Save starting point
        Vector3 originalPosition = transform.position;

        // Find direction toward player
        Vector2 direction = (player.position - transform.position).normalized;

        // Charge-up phase with shake
        float chargeTimer = 0f;
        while (chargeTimer < chargeTime)
        {
            warningLight.SetActive(true);
            chargeTimer += Time.deltaTime;
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * 0.05f);            
            yield return null;            
        }

        warningLight.SetActive(false);

        // Reset position after charge
        transform.position = originalPosition;

        // Lock target position at moment of lunge
        Vector2 lockedTarget = player.position;

        // Calculate REAL direction
        Vector2 lockedDirection = (lockedTarget - (Vector2)transform.position).normalized;

        // Face the correct direction
        float lockedAngle = Mathf.Atan2(lockedDirection.y, lockedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lockedAngle - 90f);

        // Enable trail
        if (trailRenderer != null)
            trailRenderer.emitting = true;

        // Actually lunge forward in that direction
        float lungeTimer = 0f;
        while (lungeTimer < lungeDuration)
        {
            transform.position += (Vector3)(lockedDirection * lungeSpeed * Time.deltaTime);
            lungeTimer += Time.deltaTime;
            yield return null;
        }

        // Stop trail and cooldown
        if (trailRenderer != null)
            trailRenderer.emitting = false;

        isLunging = false;
        animator.CrossFade("fast", 0.15f);
        yield return new WaitForSeconds(lungeCooldown);
        canLunge = true;
    }

    // ===== DAMAGE & DEATH =====
    public void TakeDamage(float damage)
    {
        if (isDead) return;


        health -= damage;
        if (health <= 0 && !isTanky)
        {
            if (hitty || crawler || flyer) StartCoroutine(Die(1f));
            else StartCoroutine(Die(0.5f));

        }
        else if (isTanky && health <= 0)
        {
            int num = UnityEngine.Random.Range(2, 5);
            StartCoroutine(TankyDie(num));
        }
    }

    IEnumerator Die(float time)
    {
        animator.SetTrigger("Dead");
        isDead = true;
        yield return new WaitForSeconds(time);
        TryDropHealthPack();   // Attempt health pack drop
        Destroy(gameObject);         // Remove from scene
    }

    IEnumerator TankyDie(int count)
    {
        StartCoroutine(Die(2f));
        yield return new WaitForSeconds(1f);
        for (int i = 0; i < count; i++)
        {
            GameObject enemy = Instantiate(enemyPrefab, tankita.transform.position, Quaternion.identity);
            Enemy_Script enemyScript = enemy.GetComponent<Enemy_Script>();
            enemyScript.player = GameObject.FindGameObjectWithTag("Player").transform;
            Debug.Log("what?");
            ApplySmoll(enemyScript);

            // Track active smoll
            activeEnemies.Add(enemy);
            StartCoroutine(RemoveOnDestroy(enemy));
            yield return new WaitForSeconds(0.4f);
        }
    }
    void ApplySmoll(Enemy_Script enemy)
    {
        // Reset all flags first
        enemy.hitty = false;
        enemy.shooty = false;
        enemy.tanky = false;
        enemy.lungie = false;

        // Make Smoll flag true
        enemy.smoll = true;
    }
    IEnumerator RemoveOnDestroy(GameObject enemy)
    {
        while (enemy != null)
            yield return null;

        activeEnemies.RemoveAll(e => e == null);
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
        else
        {
            return;
        }
    }


}
