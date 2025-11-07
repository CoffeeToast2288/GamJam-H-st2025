using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class upgrades : MonoBehaviour
{

    public string[] upgrade_descriptions;
    public PlayerStats stats; // this refrances the player stats
    public TMP_Text text_box, number_of_points;
    public int upgrade_points;
    public AudioSource stat_bonus_sound;
    public TMP_Text[] displayer_text_boxes;
    public bool has_upgrade_open;
    public GameObject upgrade_screen;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        displayer_text_boxes[0].text = "damage= " + stats.damage;
        displayer_text_boxes[1].text = "HP= " + stats.hp;
        displayer_text_boxes[2].text = "Speed= " + stats.speed;
        displayer_text_boxes[3].text = "Dash Charges= " + stats.dash_chargers;
        displayer_text_boxes[4].text = "Dash Cooldown= " + stats.dash_coldown_reduction;
        displayer_text_boxes[5].text = "Attack Speed= " + stats.attack_speed;
        number_of_points.text = "Skill point= " + number_of_points;

        if (!has_upgrade_open && Input.GetKeyDown(KeyCode.F))
        {
            open();

        }
        else if (has_upgrade_open && Input.GetKeyDown(KeyCode.F))
        {
            close();

        }


    }
    public void open()
    {
        has_upgrade_open = true;
        upgrade_screen.SetActive(true);

    }
  
    public void close()
    {
        has_upgrade_open = false;
        upgrade_screen.SetActive(false);

    }
    


    public void OnButtonClicked(int id)
    {
        
        if (upgrade_points > 0)
        {

            switch (id)
            {
                case 0:// damage boost 
                    stats.damage += 1;
                    
                    break;

                case 1: // health boost
                    stats.hp += 1;

                    break;

                case 2:// speed boost
                    stats.speed += 1;

                    break;

                case 3:// dash charge boost
                    stats.dash_chargers += 1;

                    break;

                case 4:// dash cooldown boost 
                    stats.dash_coldown_reduction += 1;
                    break;

                case 5:// attack speed
                    stats.attack_speed += 1;

                    break;

                default:
                    upgrade_points -= 1;
                    text_box.text = upgrade_descriptions[id];
                    stat_bonus_sound.Play();
                    break;
            }

        }


    }



}

