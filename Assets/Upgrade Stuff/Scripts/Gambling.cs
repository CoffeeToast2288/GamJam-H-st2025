using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Gambling : MonoBehaviour
{
    public Button spin; // randomises the numbers
    public Button select; // select the perk
    public bool hasrolled = false;
    public Text FlavorText;
    public Text Name;

    
    public int whatupgrade; // number of the upgrade (6 wepon 4 misc curently implamented.)
    public PlayerAttack attack; // i think you know what this is 

    [Header("Refrences")]
    public Melee mellescript;
    public PlayerAttack PlayerAttackScript;
    public PlayerHealth HealtScript;
    public Bullet_Script Bullet;
    public Playermovment MovmentScript;
    public LightUpgrad light;
    public Tekniikare stinky;
    public HealthPack HelthPackScript;
    private void Start()
    {
        spin.onClick.AddListener(gamble); 
        select.onClick.AddListener(pickskill);
    }
    void gamble() // randomise the numbers
    {
       
        whatupgrade = Random.Range(1, 11);
        hasrolled = true; // activates the butons to select shit

    }
    void pickskill() // void to determin withc buton corisponds to what skill and selecting them 
    {
        if(hasrolled == true)
        {
           
            
                if(whatupgrade == 1)
                {
                    PlayerAttackScript.SideAttacks = true;
                    Name.text = "Side Attack";
                    FlavorText.text = "This attack upgrade allows your attacks to hit enemies to your sides";
                }
                if (whatupgrade == 2)
                {
                    PlayerAttackScript.BackAttack = true;
                    Name.text = "Back Attack";
                    FlavorText.text = "This attack upgrade allows your attacks to hit enemies behind you";
            }
                if (whatupgrade == 3)
                {
                    if(PlayerAttackScript.sword == true)
                    {
                        mellescript.kancrit = true;
                    Name.text = "Crit";
                    FlavorText.text = "This attack upgrade alows your sword strikes";

                }
                    else
                    {

                        PlayerAttackScript.Shotgun = true;
                    }
                }
                if (whatupgrade == 4)
                {
                    if(PlayerAttackScript.sword == true)
                    {
                        PlayerAttackScript.Upgradeattackwidth();
                    }
                    else
                    {
                        Bullet.piercing = true;

                    }
                }
                if (whatupgrade == 5)
                {
                    if(PlayerAttackScript.sword == true)
                    {

                        MovmentScript.dashattack = true;
                    }
                    else
                    {

                        PlayerAttackScript.doubleshoot = true;
                    }

                }
                if (whatupgrade == 6)
                {
                   if(PlayerAttackScript.sword == true)
                    {
                        PlayerAttackScript.Upgradeattackrange();

                    }
                    else
                    {


                    }
                }
                
            

            
            
                if (whatupgrade == 7)
                {
                    HealtScript.revive = true;

                }
                if (whatupgrade == 8)
                {
                    HelthPackScript.healupgrade = true;

                }
                if (whatupgrade == 9)
                {
                stinky.unlocked = true;

                }
                if (whatupgrade == 10)
                {
                    Debug.Log("4 misc");

                }
               
               



         }




        
    }
}
