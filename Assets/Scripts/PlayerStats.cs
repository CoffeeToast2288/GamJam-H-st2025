using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    [Header("Refrences")] // Refrenc to the varius scripts that the stats interact with - Nino
    [SerializeField] PlayerHealth HealthScript;
    [SerializeField] PlayerAttack AttacScript;
    [SerializeField] Melee MeleeScript;
    [SerializeField] Bullet_Script Bullet;
    [SerializeField] Playermovment PlayermovmentScript;

    public float dash_chargers = 1;
    public float dash_coldown_reduction = 1;

    [Header("Base Stats")]
    public float baseDamage = 1;
    public float baseHP = 4;
    public float baseSpeed = 5;
    public float baseAttackSpeed = 5;

    [Header("Runtime Stats")]
    public float damage;
    public float hp;
    public float speed;
    public float attack_speed;

    [Header("Upgrades")]
    public bool pierce;
    public float pierceAmount;


    public void ResetStats()
    {
        damage = baseDamage;
        hp = baseHP;
        speed = baseSpeed;
        attack_speed = baseAttackSpeed;
    }

    public void Start()
    {
        ApplyStats();
    }

    public void ApplyStats()
    {
        Debug.Log("Health: " + HealthScript);
        Debug.Log("Movement: " + PlayermovmentScript);
        Debug.Log("Melee: " + MeleeScript);
        Debug.Log("Attack: " + AttacScript);
        Debug.Log("Bullet: " + Bullet);

        HealthScript?.updatehealth();
        PlayermovmentScript?.speedupdate();
        MeleeScript?.damageupdate();
        AttacScript?.UpdateAtackSpeed();
        Bullet?.damageupdate();
    }
}
