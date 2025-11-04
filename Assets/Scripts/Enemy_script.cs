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
    public GameObject hitboxObject; // assign the hitbox child here

    [Header("References")]
    public Transform player;
    private Rigidbody2D rb;
    public Transform spawnPos;
    public GameObject enemyBullet;


    private bool isDead = false;
    private bool canAttack = true;
    private bool canshoot = true;
    private bool isShooty = false;

    [Header("Type")]
    public bool shooty;
    public bool hitty;
    public bool tanky;
    public bool lungie;


    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        if (hitty == true)
        {
            Hitty();
        }
        if (shooty == true)
        {
            Shooty();
        }
        if (tanky == true)
        {
            Tanky();
        }
        if (lungie == true)
        {
            Lungie();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (isDead || player == null) return;

        float distance = Vector2.Distance(transform.position, player.position);

        if (distance > stopDistance)
        {
            MoveTowardsPlayer();
            RotateTowardsPlayer();
        }
        else if (distance <= attackRange)
        {
            TryAttack();
        }   

        if(distance <= shootrange && isShooty == true)
        {
            TryFire();
            RotateTowardsPlayer();
        }
             
    }

    public void Hitty()
    {
        moveSpeed = 3f;
        damage = 1f;
        health = 3f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
    }
    public void Shooty()
    {
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
        moveSpeed = 1.5f;
        damage = 2f;
        health = 6f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
    }
    public void Lungie()
    {
        moveSpeed = 4f;
        damage = 1f;
        health = 1.5f;
        stopDistance = 1.1f;
        attackRange = 1.2f;
    }
    void MoveTowardsPlayer()
    {
        transform.position = Vector2.MoveTowards(transform.position, player.position, moveSpeed * Time.deltaTime);
    }

    void RotateTowardsPlayer()
    {
        Vector2 direction = (player.position - transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rb.rotation = angle; // sets rotation around Z-axis in degrees
    }
    void TryAttack()
    {
        if (!canAttack) return;
        StartCoroutine(AttackRoutine());
    }

    System.Collections.IEnumerator AttackRoutine()
    {
        canAttack = false;
        hitboxObject.SetActive(true); // enable hitbox briefly
        yield return new WaitForSeconds(0.4f); // attack duration
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
    public void TakeDamage(float damage)
    {
        if (isDead) return;

        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Health left: {health}");

        if (health <= 0)
            Die();
    }

    void Die()
    {
        isDead = true;
        rb.linearVelocity = Vector2.zero;
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject, 0.5f); // small delay before removal
    }
}
