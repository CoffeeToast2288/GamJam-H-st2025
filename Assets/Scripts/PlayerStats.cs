using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    [Header("Refrences")]
    PlayerHealth HealthScript;
    PlayerAttack AttacScript;
    Melee MeleeScript;
    Bullet_Script Bullet;
    Playermovment PlayermovmentScript;


    [Header("Stats")]
    public float damage = 1; 
    public int hp = 4;
    public int speed = 5; 
    public int dash_chargers = 1;
    public float dash_coldown_reduction = 1;
    public float attack_speed = 1;

    public bool upgraded = false;


   public void Update()
    {
        if(upgraded == true)
        {
            HealthScript.updatehealth();
            PlayermovmentScript.speedupdate();
            MeleeScript.damageupdate();

        }
    }

}
