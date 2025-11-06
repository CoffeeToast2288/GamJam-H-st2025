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
                    FlavorText.text = "This attack upgrade alows your sword to criticaly stike enemies, dealing 1.5x more damage. Crit strike scales with dash charges and you gaine 25% for etch charg";

                }
                    else
                    {

                        PlayerAttackScript.Shotgun = true;
                    Name.text = "Shotgun";
                    FlavorText.text = "You now have a shotgun. Figure it out´.";

                }
                }
                if (whatupgrade == 4)
                {
                    if(PlayerAttackScript.sword == true)
                    {
                        PlayerAttackScript.Upgradeattackwidth();
                    Name.text = "Swipe";
                    FlavorText.text = "Your sword now swipes in wider arches, making it kapeble of hiting more enemies";
                    }
                    else
                    {
                        Bullet.piercing = true;
                    Name.text = "Piercing";
                    FlavorText.text = "Your bullets now pierce enemies, making them kapeble of hitting multiple enemies in a row";

                    }
                }
                if (whatupgrade == 5)
                {
                    if(PlayerAttackScript.sword == true)
                    {

                        MovmentScript.dashattack = true;
                    Name.text = "Dash attack";
                    FlavorText.text = "You now attack with your sword whenever you dash. This ignores the coldown on attacking from attack speed.";
                         
                    }
                    else
                    {

                        PlayerAttackScript.doubleshoot = true;
                    Name.text = "Double shoot";
                    FlavorText.text = "You now shoot twice as mutch bulet for the price of one";
                    }

                }
                if (whatupgrade == 6)
                {
                   if(PlayerAttackScript.sword == true)
                    {
                        PlayerAttackScript.Upgradeattackrange();
                    Name.text = "Lunge";
                    FlavorText.text = "Your sword swings now have more range, alowing you to hit enemies that are further away";
                    }
                    else
                    {

                    Name.text = "Exploding Bullet";
                    FlavorText.text = "Kaboom";
                    }
                }
                
            

            
            
                if (whatupgrade == 7)
                {
                    HealtScript.revive = true;
                Name.text = "Revive";
                FlavorText.text = "You may defy death. But only once.";
                }
                if (whatupgrade == 8)
                {
                    HelthPackScript.healupgrade = true;
                Name.text = "Better healthpack";
                FlavorText.text = "Eat the health pack!";

                }
                if (whatupgrade == 9)
                {
                stinky.unlocked = true;
                Name.text = "Teknikare";
                FlavorText.text = "Eftersom att plugar teknik så är det extrenmt osanolikt att du dushar . You stink so bad that enemies die";

                }
              
               
               



         }




        
    }
}
