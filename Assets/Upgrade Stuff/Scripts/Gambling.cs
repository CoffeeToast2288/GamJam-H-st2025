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

    public int upgradetype;// misc or wepon
    public int whatupgrade; // number of the upgrade (6 wepon 4 misc curently implamented.)
    public PlayerAttack attack; // i think you know what this is 

    [Header("Refrences")]
    public Melee mellescript;
    public PlayerAttack PlayerAttackScript;
    public PlayerHealth HealtScript;
    public Bullet_Script Bullet;
    public Playermovment MovmentScript;
    public LightUpgrad light;
    public Tekniikare sttinky;
    public HealthPack HelthPackScript;
    private void Start()
    {
        spin.onClick.AddListener(gamble); 
        select.onClick.AddListener(pickskill);
    }
    void gamble() // randomise the numbers
    {
        upgradetype = Random.Range(1, 4);
        whatupgrade = Random.Range(1, 7);
        hasrolled = true; // activates the butons to select shit

    }
    void pickskill() // void to determin withc buton corisponds to what skill and selecting them 
    {
        if(hasrolled == true)
        {
            if(upgradetype < 3) 
            {
                if(whatupgrade == 1)
                {
                    PlayerAttackScript.SideAttacks = true;
                    
                }
                if (whatupgrade == 2)
                {
                    PlayerAttackScript.BackAttack = true;

                }
                if (whatupgrade == 3)
                {
                    if(PlayerAttackScript.sword == true)
                    {
                       

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


                    }
                    else
                    {


                    }
                }
                
            }

            if(upgradetype >= 3)
            {
                if (whatupgrade == 1)
                {
                    Debug.Log("1 misc");

                }
                if (whatupgrade == 2)
                {
                    Debug.Log("2 misc");

                }
                if (whatupgrade == 3)
                {
                    Debug.Log("3 misc");

                }
                if (whatupgrade == 4)
                {
                    Debug.Log("4 misc");

                }
                if (whatupgrade == 5)
                {
                    Debug.Log("5 misc");

                }
                if (whatupgrade == 6)
                {
                    Debug.Log("6 misc");

                }
               



            }




        }
    }
}
