using UnityEngine;
using UnityEngine.Audio;

public class music_controler : MonoBehaviour
{
    public AudioSource[] fighty_music;
    public AudioSource[] safe_music;
    public int play_audio;
    AudioSource current_audio;
    public bool is_in_fighty;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        is_in_fighty = true;
        current_audio = fighty_music[0];
    }

    // Update is called once per frame
    void Update()
    {

        if (is_in_fighty && !current_audio.isPlaying)
        { 
            if (play_audio != 3)
            {
                play_audio += 1;

            }
            else
            {
                play_audio = 0;
            }

            current_audio = fighty_music[play_audio];
            current_audio.Play();

        }
    }

    public void fighty_music_play()
    {
        

        

    }



}
