using System.Collections;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [Header("Attack Setting")]
    public PlayerStats stats;

    [Header("Directional Melee Attack Objects")]
    public GameObject Attack;
    public GameObject AttackBack;
    public GameObject AttackLeft;
    public GameObject AttackRight;

    [Header("Upgrade Variables")]
    public bool SideAttacks = false;
    public bool BackAttack = false;
    public bool Shotgun = false;
    public bool doubleshoot;
    public bool dashattack;
    public bool Pierce;
    public bool bulletExplosion = false;
    public bool piercingUpgrade = false;  // ✅ NEW

    [Header("Melee Visual Cue")]
    public SpriteRenderer[] meleeFlashes;
    public float flashFadeIn = 0.05f;
    public float flashStay = 0.05f;
    public float flashFadeOut = 0.2f;
    public float flashScaleMultiplier = 1.3f;

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
    public float colldown_max = 5f;
    public bool sword;
    public bool colldown_active = false;
    public bool Isattacking = false;


    public Animator player_animator;
    public string[] play_animations;

    void Start()
    {
        colldown = 0;
    }

    void Update()
    {
        if (doubleshoot)
            bullet = doublebullets;

        CheckTimer();

        if ((Input.GetKey(KeyCode.E) || Input.GetMouseButton(0)) && colldown == 0)
        {
            if (sword)
                Attacking();
            else
                Shoot();
        }

        if (colldown_active)
            colldown -= Time.deltaTime;

        if (colldown < 0)
            colldown = 0;

        if (colldown <= 0)
            colldown_active = false;

        if (dashattack)
            Attacking();
    }



    public IEnumerator player_animations_reset()
    {
        AnimatorClipInfo[] clipInfo = player_animator.GetCurrentAnimatorClipInfo(0);
        float clipLength = clipInfo[0].clip.length;
        Debug.Log("clip length " + clipLength);
        yield return new WaitForSeconds(clipLength);
        player_animator.CrossFade(play_animations[0], 0.2f);
    }


    void Attacking()
    {
        if (!Isattacking)
        {
            player_animator.CrossFade(play_animations[2], 0.2f);
            StartCoroutine(player_animations_reset());

            Attack.SetActive(true);
            Isattacking = true;

            colldown_active = true;
            colldown = colldown_max;

            if (SideAttacks)
            {
                AttackLeft.SetActive(true);
                AttackRight.SetActive(true);
            }

            if (BackAttack)
                AttackBack.SetActive(true);

            dashattack = false;

            StartCoroutine(MeleeFlashEffect());
        }
    }

    private IEnumerator MeleeFlashEffect()
    {
        if (meleeFlashes == null || meleeFlashes.Length == 0)
            yield break;

        // store original scales
        Vector3[] originalScales = new Vector3[meleeFlashes.Length];
        Vector3[] bigScales = new Vector3[meleeFlashes.Length];

        for (int i = 0; i < meleeFlashes.Length; i++)
        {
            if (meleeFlashes[i] == null) continue;

            // reset alpha
            Color c = meleeFlashes[i].color;
            meleeFlashes[i].color = new Color(c.r, c.g, c.b, 0f);

            originalScales[i] = meleeFlashes[i].transform.localScale;
            bigScales[i] = originalScales[i] * flashScaleMultiplier;
        }

        // ---------- FADE IN ----------
        float t = 0;
        while (t < flashFadeIn)
        {
            t += Time.deltaTime;
            float a = t / flashFadeIn;

            for (int i = 0; i < meleeFlashes.Length; i++)
            {
                if (meleeFlashes[i] == null) continue;

                Color c = meleeFlashes[i].color;
                meleeFlashes[i].color = new Color(c.r, c.g, c.b, a);

                meleeFlashes[i].transform.localScale =
                    Vector3.Lerp(originalScales[i], bigScales[i], a);
            }

            yield return null;
        }

        // ---------- HOLD ----------
        yield return new WaitForSeconds(flashStay);

        // ---------- FADE OUT ----------
        t = 0;
        while (t < flashFadeOut)
        {
            t += Time.deltaTime;
            float a = 1 - (t / flashFadeOut);

            for (int i = 0; i < meleeFlashes.Length; i++)
            {
                if (meleeFlashes[i] == null) continue;

                Color c = meleeFlashes[i].color;
                meleeFlashes[i].color = new Color(c.r, c.g, c.b, a);

                meleeFlashes[i].transform.localScale =
                    Vector3.Lerp(bigScales[i], originalScales[i], 1 - a);
            }

            yield return null;
        }

        // reset to original
        for (int i = 0; i < meleeFlashes.Length; i++)
        {
            if (meleeFlashes[i] == null) continue;

            Color c = meleeFlashes[i].color;
            meleeFlashes[i].color = new Color(c.r, c.g, c.b, 0f);
            meleeFlashes[i].transform.localScale = originalScales[i];
        }
    }


    void Shoot()
    {
        if (!Isattacking)
        {
            FireBullet(spawnPos);

            Isattacking = true;
            colldown_active = true;
            colldown = colldown_max;

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
        }
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


    void CheckTimer()
    {
        if (Isattacking)
        {
            attack_timer += Time.deltaTime;

            if (attack_timer > attack_duration)
            {
                Isattacking = false;
                attack_timer = 0;

                Attack.SetActive(false);
                AttackBack.SetActive(false);
                AttackLeft.SetActive(false);
                AttackRight.SetActive(false);
            }
        }
    }


    // melee upgrades
    public void Upgradeattackwidth()
    {
        Attack.transform.localScale += new Vector3(1, 0, 0);
        AttackBack.transform.localScale += new Vector3(1, 0, 0);
        AttackLeft.transform.localScale += new Vector3(1, 0, 0);
        AttackRight.transform.localScale += new Vector3(1, 0, 0);
    }
    public void Upgradeattackrange()
    {
        Attack.transform.localScale += new Vector3(0, 1, 0);
        AttackBack.transform.localScale += new Vector3(0, 1, 0);
        AttackLeft.transform.localScale += new Vector3(0, 1, 0);
        AttackRight.transform.localScale += new Vector3(0, 1, 0);
    }
    public void UpdateAtackSpeed()
    {
        colldown_max = 5f;
        if(colldown_max == 5f)
        {

            colldown_max /= stats.attack_speed;

        }


    }
    
}
