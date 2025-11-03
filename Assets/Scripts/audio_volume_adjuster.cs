using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class audio_volume_adjuster : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public audiocontroler audio_stats;
    public float volume;
    public AudioSource self;
    public string sound_type;

    void Start()
    { 
        self = GetComponent<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
       if( sound_type == "monster")
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
        self.volume = volume;

    }
}
