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
    public float chargeTime = 0.5f;       // how long to charge before lunging
    public float lungeRange = 8f;
    public GameObject hitboxObject;

    [Header("References")]
    public Transform player;
    public GameObject tankita;
    private Rigidbody2D rb;
    public Transform spawnPos;
    public GameObject enemyBullet;
    public SpriteRenderer spriteRenderer;    // assign your enemy sprite here



    private bool isDead = false;
    private bool canAttack = true;
    private bool canshoot = true;
    private bool isShooty = false;
    private bool isLungie = false;
    private bool isLunging = false;
    private bool canLunge = true;
    private int type;

    [Header("Type")]
    public bool shooty;
    public bool hitty;
    public bool tanky;
    public bool lungie;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        type = Random.Range( 1, 5);

        if (type == 1)
        {
            Hitty();
        }
        if (type == 2)
        {
            Shooty();
        }
        if (type == 3)
        {
            Tanky();
        }
        if (type == 4)
        {
            Lungie();
        }
    }

    // Update is called once per frame
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

    public void Hitty()
    {
        hitty = true;
        moveSpeed = 3f;
        damage = 1f;
        health = 3f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
    }
    public void Shooty()
    {
        shooty = true;
        moveSpeed = 3f;
        damage = 1f;
        health = 2f;
        stopDistance = 6f;
        shootrange = 10f;
        attackRange = 1.2f;
        isShooty = true;
    }
    public void Tanky()
    {
        tanky = true;
        tankita.gameObject.transform.localScale += new Vector3(1.2f, 1.2f, 1.2f);
        moveSpeed = 1.5f;
        damage = 2f;
        health = 6f;
        stopDistance = 1.8f;
        attackRange = 1.9f;
    }
    public void Lungie()
    {
        lungie = true;
        moveSpeed = 4f;
        damage = 1f;
        health = 2f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
        isLungie = true;
    }    

    

    
    void MoveTowardsPlayer() 
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer() // looks at the player
    {
        Vector2 direction = (player.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle; 
    }
    void TryAttack() // makes the attack have a cooldown
    {
        if (!canAttack) return;
        StartCoroutine(AttackRoutine());
    }

    System.Collections.IEnumerator AttackRoutine() // attack script
    {
        canAttack = false;
        hitboxObject.SetActive(true); // enable hitbox briefly
        yield return new WaitForSeconds(0.4f); // attack duration
        hitboxObject.SetActive(false);
        yield return new WaitForSeconds(attackCooldown); 
        canAttack = true;
    }
    void TryFire() // Makes the shooting have a cooldown
    {
        if (!canshoot) return;
        StartCoroutine(ShootRoutine());
    }

    System.Collections.IEnumerator ShootRoutine() // shooting script
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

        // --- Charge Phase ---
        Vector3 originalPosition = transform.position;
        Vector2 direction = (player.position - transform.position).normalized;

        // Flash red to warn player
        float chargeTimer = 0f;
        while (chargeTimer < chargeTime)
        {
            chargeTimer += Time.deltaTime;
            transform.position = originalPosition + (Vector3)(Random.insideUnitCircle * 0.05f);

            yield return null;
        }


        transform.position = originalPosition;

        // --- Lock Direction ---
        Vector2 lockedDirection = player.position;
        float lockedAngle = Mathf.Atan2(lockedDirection.y, lockedDirection.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, lockedAngle - 90f);

        // --- Lunge Phase ---
        float lungeTimer = 0f;
        while (lungeTimer < lungeDuration)
        {
            transform.position = Vector2.MoveTowards(transform.position, lockedDirection, lungeSpeed * Time.deltaTime);
            lungeTimer += Time.deltaTime;
            yield return null;
        }

        // --- Cooldown Phase ---
        isLunging = false;
        yield return new WaitForSeconds(lungeCooldown);
        canLunge = true;
    }
    public void TakeDamage(float damage) // 
    {
        if (isDead) return;

        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health left: {health}");

        if (health <= 0)
            Die();
    }


    void Die() // dying script
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject); // small delay before removal
    }
}
