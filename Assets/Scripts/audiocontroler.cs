using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class audiocontroler : MonoBehaviour
{
    public List<AudioSource> audio_list = new List<AudioSource>();
    public float master_volume, monster_volume, player_volume, level_volume, effect_volume;
    public Scrollbar master_scroll, monster_scroll, player_scroll, level_scroll, effect_scroll;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
     if (master_scroll.value != master_volume)
        {
            master_volume_change();
        }
     if  (monster_volume != monster_scroll.value)
        {
            monster_volume_change();
        }
     if (player_volume != player_scroll.value)
        {
            player_volume_change();
        }
     if (level_volume != level_scroll.value)
        {
            level_volume_change();
        }
     if (effect_volume != effect_scroll.value)
        {
            effect_volume_change();
        }


    }

    public void master_volume_change()
    {
        master_volume = master_scroll.value;


    }
    public void monster_volume_change()
    {
        monster_volume = monster_scroll.value;


    }
    public void player_volume_change()
    {
        player_volume = player_scroll.value;


    }
    public void level_volume_change()
    {
        level_volume = level_scroll.value;


    }
    public void effect_volume_change()
    {
        effect_volume = effect_scroll.value;

    }



}
