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


    [Header("Stats")] // the stats - Nino
    public float damage = 1; 
    public float hp = 4;
    public float speed = 5; 
    public float dash_chargers = 1;
    public float dash_coldown_reduction = 1;
    public float attack_speed = 1;

    public bool upgraded = false;

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

        upgraded = false;
    }

}
