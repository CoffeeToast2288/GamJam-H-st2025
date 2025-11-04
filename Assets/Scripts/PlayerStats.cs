using UnityEngine;
using UnityEngine.Rendering;


[System.Serializable]
public class PlayerStats : MonoBehaviour
{
    static PlayerStats stats;
    static PlayerStats GetStats;
    //stats 
    public float damage; 
    public int hp;
    public int speed; 
    public int dash_chargers;
    public float dash_coldown_reduction;
    public float attack_speed;

    
}
