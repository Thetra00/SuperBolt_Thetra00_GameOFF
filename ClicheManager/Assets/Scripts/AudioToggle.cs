using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioToggle : MonoBehaviour
{
   
    [SerializeField] GameObject _toggleImage;
    private bool _togglebool = true;
    public void AudioToggled()
    {
        _togglebool = !_togglebool;
        AudioManager.instance.AudioToggle();
       
        _toggleImage.SetActive(_togglebool);
    }



}
