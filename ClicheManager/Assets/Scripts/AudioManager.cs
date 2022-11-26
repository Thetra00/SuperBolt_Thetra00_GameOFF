using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    private AudioClip savedMusic;
    private void Awake()
    {
     if(instance == null)
        instance = this;
     else
        Destroy(gameObject);

     DontDestroyOnLoad(gameObject);
    }

    public AudioSource Music, BossMusic, SFX;

    private void Update()
    {
        if (GameManager.instance != null)
        {
            if (GameManager.instance.leveltime < 45)
                Music.pitch = 1.3f;
            else
                Music.pitch = 1f;
        }
    }



    public void PlayMusic(AudioClip clip, float delay)
    {
        savedMusic = clip;
        Music.clip = null;
        Music.clip = clip;
        Music.PlayDelayed(delay);
    }
    public void PlayBossMusic(AudioClip clip)
    {
        BossMusic.gameObject.SetActive(true);
        Music.gameObject.SetActive(false);
        BossMusic.clip = null;
        BossMusic.clip = clip;
        BossMusic.PlayDelayed(0);
    }
    public void StopBossMusic()
    {
        Music.gameObject.SetActive(true);
        BossMusic.gameObject.SetActive(false);

        PlayMusic(savedMusic, 0);
    }


    public void PlaySFX(AudioClip clip)
    {
        SFX.PlayOneShot(clip);
    }

    public void ChangeMasterVolume(float value)
    {
        AudioListener.volume = value;
    }


    public void AudioToggle()
    {
        Music.mute = !Music.mute;
        BossMusic.mute = !BossMusic.mute;
        SFX.mute = !SFX.mute;
    }


}
