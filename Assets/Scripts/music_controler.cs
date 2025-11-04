using UnityEngine;
using UnityEngine.Audio;

public class music_controler : MonoBehaviour
{
    public AudioSource[] fighty_music; // the songs for in fighty areana  there are 4 songs
    public AudioSource[] safe_music; // the songs for the safe zone there are 2 songs 
    public int play_audio; // this decides what audio in the array is played 
    AudioSource current_audio; // this is the current audio being played
    public bool is_in_fighty, is_playing_fighty; // is_in_fighty shows if the player is in the fighting area or not and is_playing_fighty shows what areana the music being played is for 

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        is_in_fighty = true;
        current_audio = fighty_music[0];
    }

    // Update is called once per frame
    void Update()
    {

        if (is_in_fighty) // this checks if the player is in the fighty arena 
        {
            if (!current_audio.isPlaying && is_playing_fighty) // this checks if the audio is playing and if the correct music is playing 
            {
                if (play_audio != 3)// this if and else statment runs through the audio cycle and resets at three
                {
                    play_audio += 1;

                }
                else
                {
                    play_audio = 0;
                }

                current_audio = fighty_music[play_audio];// changes the current audio to match the one in the cycle 
                current_audio.Play(); // plays the audio
                is_playing_fighty = true; // shows that the music playing is for the fighty areana 
            }
            else if (current_audio.isPlaying && !is_playing_fighty) // this is identical to the one above but checks if the music playing is the correct one for the areana 
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
                is_playing_fighty = true;
            }

        }
        else if (!is_in_fighty) // this checks if the player is in the safe arena it is the same as the one above but uses the safe areana music 
        {
            if (!current_audio.isPlaying && !is_playing_fighty)
            {
                if (play_audio != 1)
                {
                    play_audio += 1;

                }
                else
                {
                    play_audio = 0;
                }

                current_audio = safe_music[play_audio];
                current_audio.Play();
                is_playing_fighty = false;
            }
            else if (current_audio.isPlaying && is_playing_fighty)
            {
                if (play_audio != 1)
                {
                    play_audio += 1;

                }
                else
                {
                    play_audio = 0;
                }

                current_audio = safe_music[play_audio];
                current_audio.Play();
                is_playing_fighty = false;

            }
        }
    }

    


}
