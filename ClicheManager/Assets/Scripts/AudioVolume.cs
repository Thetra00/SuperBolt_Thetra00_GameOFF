using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioVolume : MonoBehaviour
{
    [SerializeField] Slider _slider;

    void Start()
    {
        _slider.value = AudioListener.volume;

        AudioManager.instance.ChangeMasterVolume(_slider.value);
     
        _slider.onValueChanged.AddListener(Val => AudioManager.instance.ChangeMasterVolume(Val));
    }
}
