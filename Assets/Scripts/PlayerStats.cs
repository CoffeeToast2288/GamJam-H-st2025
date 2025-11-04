using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "PlayerStats", menuName = "Scriptable Objects/PlayerStats")]
public class PlayerStats : ScriptableObject
{
    //stats 
    int damage;
    int hp;
    int speed; 
    int dash_chargers;
    float dash_coldown_reduction;
    float attack_speed;

    
}
