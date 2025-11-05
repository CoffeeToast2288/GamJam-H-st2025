using UnityEngine;

public class Enemy_Script : MonoBehaviour
{
    [Header("Enemy Settings")]
    public float moveSpeed;
    public float health;
    public float damage;
    public float stopDistance;
    public float attackCooldown = 0.5f;
    public float shootCooldown = 1f;
    public float attackRange = 1.2f;
    public float shootrange = 6f;
    public float lungeSpeed = 10f;
    public float lungeDuration = 0.3f;
    public float lungeCooldown = 1.5f;
    public float chargeTime = 0.5f;
    public float lungeRange = 8f;
    public GameObject hitboxObject;

    [Header("References")]
    public Transform player;
    public GameObject tankita;
    private Rigidbody2D rb;
    public Transform spawnPos;
    public GameObject enemyBullet;
    public SpriteRenderer spriteRenderer;
    public TrailRenderer trailRenderer;

    [Header("Type Flags")]
    public bool shooty;
    public bool hitty;
    public bool tanky;
    public bool lungie;

    [Header("Elite Settings")]
    [Tooltip("Whether this enemy is an elite variant")]
    public bool isElite = false;


    [Tooltip("How much stronger elites are (stat multiplier)")]
    public float eliteMultiplier = 1.5f;

    // --- Private control flags ---
    private bool isDead = false;
    private bool canAttack = true;
    private bool canshoot = true;
    private bool isShooty = false;
    private bool isLungie = false;
    private bool isLunging = false;
    private bool canLunge = true;

    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
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

    // --- MAIN UPDATE ---
    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance <= lungeRange && canLunge && isLungie == true)
        {
            StartCoroutine(LungeAttack());
        }
        else if (distance > stopDistance && isLunging == false)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
        else if (distance <= attackRange)
        {
            TryAttack();
        }

        if (distance <= shootrange && isShooty == true)
        {
            TryFire();
            RotateTowardsPlayer();
        }
    }

    // --- ENEMY TYPE SETUPS ---
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

    // --- ELITE HANDLING ---
    public void SetElite(bool eliteStatus)
    {
        isElite = eliteStatus;

        if (isElite)
        {
            // Boost stats
            health *= eliteMultiplier;
            damage *= eliteMultiplier;           
        }
    }

    // --- MOVEMENT + ATTACK CODE BELOW (unchanged) ---
    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle;
    }

    void TryAttack()
    {
        if (!canAttack) return;
        StartCoroutine(AttackRoutine());
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        canAttack = false;
        hitboxObject.SetActive(true);
        yield return new WaitForSeconds(0.4f);
        hitboxObject.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }

    void TryFire()
    {
        if (!canshoot) return;
        StartCoroutine(ShootRoutine());
    }

    System.Collections.IEnumerator ShootRoutine()
    {
        canshoot = false;
        Instantiate(enemyBullet, spawnPos.position, spawnPos.rotation);
        yield return new WaitForSeconds(shootCooldown);
        canshoot = true;
    }

    System.Collections.IEnumerator LungeAttack()
    {
        canLunge = false;
        isLunging = true;

        Vector3 originalPosition = transform.position;
        Vector2 direction = (player.position - transform.position).normalized;

        float chargeTimer = 0f;
        while (chargeTimer < chargeTime)
        {
            chargeTimer += Time.deltaTime;
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * 0.05f);
            yield return null;
        }

        transform.position = originalPosition;

        Vector2 lockedDirection = player.position;
        float lockedAngle = Mathf.Atan2(lockedDirection.y, lockedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lockedAngle - 90f);

        if (trailRenderer != null)
            trailRenderer.emitting = true;

        float lungeTimer = 0f;
        while (lungeTimer < lungeDuration)
        {
            transform.position = Vector2.MoveTowards(transform.position, lockedDirection, lungeSpeed * Time.deltaTime);
            lungeTimer += Time.deltaTime;
            yield return null;
        }

        if (trailRenderer != null)
            trailRenderer.emitting = false;

        isLunging = false;
        yield return new WaitForSeconds(lungeCooldown);
        canLunge = true;
    }

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
        Destroy(gameObject);
    }
}
