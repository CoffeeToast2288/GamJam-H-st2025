using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Setting")]
    public PlayerStats stats;

    [Header("Upgrade Variables")]
    public bool SideAttacks = false;
    public bool BackAttack = false;
    public bool Shotgun = false;
    public bool doubleshoot;
    public bool dashattack;
    public bool bulletExplosion = false;
    public bool piercingUpgrade = false;  // ✅ NEW



    [Header("References")]
    public Transform spawnPos;
    public Transform spawnPosBack;
    public Transform spawnPosLeft;
    public Transform spawnPosRight;
    public Transform spawnPosShotgunFront1;
    public Transform spawnPosShotgunFront2;
    public Transform spawnPosShotgunBack1;
    public Transform spawnPosShotgunBack2;
    public GameObject bullet;
    public GameObject doublebullets;

    [Header("Attack Logic")]
    float attack_duration = 0.3f;
    float attack_timer;
    public float colldown;
    public float colldown_max;
    public bool colldown_active = false;
    public bool Isattacking = false;


    public Animator animator;
    void Start()
    {
        colldown = 0;
    }

    void Update()
    {
        if (doubleshoot)
            bullet = doublebullets;

        if ((Input.GetKey(KeyCode.E) || Input.GetMouseButton(0)) && Isattacking == false)
        {       
            StartCoroutine(Shoot());                        
        }

        if (colldown_active)
            colldown -= Time.deltaTime;

        if (colldown < 0)
            colldown = 0;

        if (colldown <= 0)
            colldown_active = false;
    }

    IEnumerator Shoot()
    {
        Isattacking = true;
        animator.SetTrigger("Shoots");
        FireBullet(spawnPos);

        if (Shotgun)
        {
            FireBullet(spawnPosShotgunFront1);
            FireBullet(spawnPosShotgunFront2);
        }
        if (SideAttacks)
        {
            FireBullet(spawnPosLeft);
            FireBullet(spawnPosRight);
        }
        if (BackAttack)
        {
            FireBullet(spawnPosBack);

            if (Shotgun)
            {
                FireBullet(spawnPosShotgunBack1);
                FireBullet(spawnPosShotgunBack2);
            }
        }
        yield return new WaitForSeconds(colldown_max);
        Isattacking = false;
    }


    // ✅ Applies upgrades to bullets
    void FireBullet(Transform t)
    {
        GameObject obj = Instantiate(bullet, t.position, t.rotation);

        Bullet_Script bs = obj.GetComponent<Bullet_Script>();
        if (bs != null)
        {
            bs.explosionEnabled = bulletExplosion;
            bs.piercing = piercingUpgrade;
        }
    }
    public void UpdateAtackSpeed()
    {
        colldown_max = Mathf.Max(0f, stats.baseAttackSpeed - stats.attack_speed);
        animator.SetFloat("ShootSpeed", 1f / Mathf.Min(1f, colldown_max));
    }
}
