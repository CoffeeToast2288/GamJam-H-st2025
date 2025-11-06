using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;
using JetBrains.Annotations;
using System.Diagnostics;
using Debug = UnityEngine.Debug;

public class Gambling : MonoBehaviour
{
    public Button spin;
    public Button select;
    public bool hasrolled = false;

    public int upgradetype;
    public int whatupgrade;
    public PlayerAttack attack;
    
    private void Start()
    {
        spin.onClick.AddListener(gamble);
        select.onClick.AddListener(pickskill);
    }
    void gamble()
    {
        upgradetype = Random.Range(1, 4);
        whatupgrade = Random.Range(1, 8);
        hasrolled = true;

    }
    void pickskill()
    {
        if(hasrolled == true)
        {
            if(upgradetype < 3) 
            {
                if(whatupgrade == 1)
                {
                    Debug.Log("1 wepon");
                    
                }
                if (whatupgrade == 2)
                {
                    Debug.Log("2 wepon");

                }
                if (whatupgrade == 3)
                {
                    Debug.Log("3 wepon");

                }
                if (whatupgrade == 4)
                {
                    Debug.Log("4 wepon");

                }
                if (whatupgrade == 5)
                {
                    Debug.Log("5 wepon");

                }
                if (whatupgrade == 6)
                {
                    Debug.Log("6 wepon");

                }
                if (whatupgrade == 7)
                {
                    Debug.Log("7 wepon");

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
                if (whatupgrade == 7)
                {
                    Debug.Log("7 misc");

                }



            }




        }
    }
}
