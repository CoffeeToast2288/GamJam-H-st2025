using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class upgrades : MonoBehaviour
{

    public string[] upgrade_descriptions;
    PlayerStats stats; // this refrances the player stats
    public TMP_Text text_box;
    public int upgrade_points;
    public AudioSource stat_bonus_sound;
    public TMP_Text[] displayer_text_boxes;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        displayer_text_boxes[0].text = displayer_text_boxes[0].text + stats.damage;
        displayer_text_boxes[1].text = displayer_text_boxes[1].text + stats.hp;
        displayer_text_boxes[2].text = displayer_text_boxes[2].text + stats.speed;
        displayer_text_boxes[3].text = displayer_text_boxes[3].text + stats.dash_chargers;
        displayer_text_boxes[4].text = displayer_text_boxes[4].text + stats.dash_coldown_reduction;
        displayer_text_boxes[5].text = displayer_text_boxes[5].text + stats.attack_speed;




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
                    break;
            }

        }


    }



}

