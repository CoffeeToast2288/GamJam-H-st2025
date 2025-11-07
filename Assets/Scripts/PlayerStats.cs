using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    [Header("Refrences")] // Refrenc to the varius scripts that the stats interact with - Nino
    PlayerHealth HealthScript;
    PlayerAttack AttacScript;
    Melee MeleeScript;
    Bullet_Script Bullet;
    Playermovment PlayermovmentScript;


    [Header("Stats")] // the stats - Nino
    public float damage = 1; 
    public int hp = 4;
    public int speed = 5; 
    public int dash_chargers = 1;
    public float dash_coldown_reduction = 1;
    public float attack_speed = 1;

    public bool upgraded = false;


   public void Update() // Refrences funktions to update the stats in verius scripts - Nino
    {
        if(upgraded == true)
        {
            HealthScript.updatehealth();
            PlayermovmentScript.speedupdate();
            MeleeScript.damageupdate();
            AttacScript.UpdateAtackSpeed();

        }
    }

}
