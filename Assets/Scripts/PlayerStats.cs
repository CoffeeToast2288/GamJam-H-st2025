using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    //stats 
    public float damage;
    public int hp;
    public int speed; 
    public int dash_chargers;
    public float dash_coldown_reduction;
    public float attack_speed;

    
}
