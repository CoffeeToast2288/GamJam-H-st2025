using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class audio_volume_adjuster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public audiocontroler audio_stats; // this refrances the audiocontroler script wich controls volume 
    public float volume; // this is the volume for the audio 
    public AudioSource self; // this is the audiosorce on the same object as this script 
    public string sound_type; // this determis what volume type it uses 

    void Start()
    { 
        self = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
       if( sound_type == "monster") // finds the matching type of volume it needs and adjusts it 
        {
            volume = audio_stats.master_volume * audio_stats.monster_volume;
           

        }
       else if (sound_type == "player")
        {
            volume = audio_stats.master_volume * audio_stats.player_volume;

        }
       else if (sound_type == "level")
        {
            volume = audio_stats.master_volume * audio_stats.level_volume;
        }
        else
        {
            volume = audio_stats.master_volume * audio_stats.effect_volume;

        }
        self.volume = volume; // sets the volume of the audiosorce

    }
}
