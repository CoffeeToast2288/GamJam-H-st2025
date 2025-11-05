using System.Collections;
using System.Runtime.ConstrainedExecution;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public PlayerStats stats;
    public bool sword;

    //directional melee attack objets
    public GameObject Attack;
    public GameObject AttackBack;
    public GameObject AttackLeft;
    public GameObject AttackRight;

    // upgrade veriables
    public bool SideAttacks = false;
    public bool BackAttack = false;
    public bool Shotgun = false;
    public bool doubleshoot;
    
    

    


 
    public bool Isattacking = false;

    // shooting attack spawn points + bullet refrenc
    public Transform spawnPos;
    public Transform spawnPosBack;
    public Transform spawnPosLeft;
    public Transform spawnPosRight;
    public Transform spawnPosShotgunFront1;
    public Transform spawnPosShotgunFront2;
    public Transform spawnPosShotgunBack1;
    public Transform spawnPosShotgunBack2;
    public GameObject bullet;
    public GameObject doublebullets;
    
    // attack duration + coldown
    float attack_duratin = 0.3f;
    float attack_timer;
    public float colldown;
    public float colldown_max = 5;
    public bool colldown_active = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colldown = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if(doubleshoot == true)
        {
            bullet = doublebullets;

        }
        
        CheckTimer();
        if (Input.GetKey(KeyCode.E) || Input.GetMouseButton(0) && colldown == 0) // attack when buton is presed and no colldown 
        {
            if (sword == true)
            {
                Attacking();
            }
            else
            {
                Shoot();
                
            }

        }
        if (colldown_active) //starts colldown
        {
            colldown -= Time.deltaTime;
        }
        if(colldown < 0)
        {
            colldown = 0;
        }
        if(colldown <= 0)
        {
     
            colldown_active = false;
           
        }
        
    }


    void Attacking() //atack funktion
    {

        if (!Isattacking)
        {
            Attack.SetActive(true);
            Isattacking = true;
            // Put animator script hear
            colldown_active = true;
            colldown = colldown_max;
            if(SideAttacks == true)
            {
                AttackLeft.SetActive(true);
                AttackRight.SetActive(true);

            }
            if(BackAttack == true)
            {
                AttackBack.SetActive(true);


            }
        }


    }
    void Shoot() // shoot function
    {
        if (!Isattacking)
        {
            Instantiate(bullet, spawnPos.position, spawnPos.rotation);
            Isattacking = true;
            // Put animator script hear
            colldown_active = true;
            colldown = colldown_max;
            if(Shotgun == true)
            {
                Instantiate(bullet, spawnPosShotgunFront1.position, spawnPosShotgunFront1.rotation);
                Instantiate(bullet, spawnPosShotgunFront2.position, spawnPosShotgunFront2.rotation);
            }
            if(SideAttacks == true)
            {
                Instantiate(bullet, spawnPosLeft.position, spawnPosLeft.rotation);
                Instantiate(bullet, spawnPosRight.position, spawnPosRight.rotation);
            }
            if(BackAttack == true)
            {
                Instantiate(bullet, spawnPosBack.position, spawnPosBack.rotation);
                if(Shotgun == true)
                {
                    Instantiate(bullet, spawnPosShotgunBack1.position, spawnPosShotgunBack1.rotation);
                    Instantiate(bullet, spawnPosShotgunBack2.position, spawnPosShotgunBack2.rotation);
                }
            }
        }

    }

    void CheckTimer() // checks attack duration
    {

        if (Isattacking)
        {
            attack_timer += Time.deltaTime;
            if(attack_timer > attack_duratin)
            {
                Isattacking = false;
                Attack.SetActive(false);
                attack_timer = 0;
                AttackBack.SetActive(false);
                AttackLeft.SetActive(false);
                AttackRight.SetActive(false);

            }
        }


    }
    // melee upgrades
    void Upgradeattackwidth()
    {
        Attack.transform.localScale += new Vector3(1, 0, 0);
        AttackBack.transform.localScale += new Vector3(1, 0, 0);
        AttackLeft.transform.localScale += new Vector3(1, 0, 0);
        AttackRight.transform.localScale += new Vector3(1, 0, 0);
    }
    void Upgradeattackrange()
    {
        Attack.transform.localScale += new Vector3(0, 1, 0);
        AttackBack.transform.localScale += new Vector3(0, 1, 0);
        AttackLeft.transform.localScale += new Vector3(0, 1, 0);
        AttackRight.transform.localScale += new Vector3(0, 1, 0);
    }

    
}
