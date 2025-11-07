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

    [Header("Bullet Upgrades")]
    public bool bulletExplosion = false;   // ✅ NEW

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

    // ✅ Applies upgrades to bullets
    void ApplyBulletUpgrades(GameObject bulletObj)
    {
        Bullet_Script bs = bulletObj.GetComponent<Bullet_Script>();
        if (bs != null)
        {
            bs.explosionEnabled = bulletExplosion;
        }
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
            player_animator.CrossFade(play_animations[4], 0.2f);
            StartCoroutine(player_animations_reset());

            // Front shot
            GameObject b = Instantiate(bullet, spawnPos.position, spawnPos.rotation);
            ApplyBulletUpgrades(b);

            Isattacking = true;
            colldown_active = true;
            colldown = colldown_max;

            // Shotgun
            if (Shotgun)
            {
                GameObject s1 = Instantiate(bullet, spawnPosShotgunFront1.position, spawnPosShotgunFront1.rotation);
                ApplyBulletUpgrades(s1);

                GameObject s2 = Instantiate(bullet, spawnPosShotgunFront2.position, spawnPosShotgunFront2.rotation);
                ApplyBulletUpgrades(s2);
            }

            // Sides
            if (SideAttacks)
            {
                GameObject l = Instantiate(bullet, spawnPosLeft.position, spawnPosLeft.rotation);
                ApplyBulletUpgrades(l);

                GameObject r = Instantiate(bullet, spawnPosRight.position, spawnPosRight.rotation);
                ApplyBulletUpgrades(r);
            }

            // Back
            if (BackAttack)
            {
                GameObject back = Instantiate(bullet, spawnPosBack.position, spawnPosBack.rotation);
                ApplyBulletUpgrades(back);

                if (Shotgun)
                {
                    GameObject b1 = Instantiate(bullet, spawnPosShotgunBack1.position, spawnPosShotgunBack1.rotation);
                    ApplyBulletUpgrades(b1);

                    GameObject b2 = Instantiate(bullet, spawnPosShotgunBack2.position, spawnPosShotgunBack2.rotation);
                    ApplyBulletUpgrades(b2);
                }
            }
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
