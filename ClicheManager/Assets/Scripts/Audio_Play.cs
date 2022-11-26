using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio_Play : MonoBehaviour
{
   public AudioClip clip;
   public bool isMusic;
    public float delay;

    private void Start()
    {
        if(isMusic)
            AudioManager.instance.PlayMusic(clip, delay);
        else
            AudioManager.instance.PlaySFX(clip);
    }


}
