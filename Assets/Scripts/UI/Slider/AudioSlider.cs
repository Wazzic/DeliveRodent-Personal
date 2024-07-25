using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioSlider : MonoBehaviour
{
    [SerializeField] Slider slider;
    private enum SliderType
    {
        Master,
        Music,
        SFX
    }
    [SerializeField] private SliderType sliderType;

    private void Awake()
    {
        if(slider == null)
        {
            slider = GetComponent<Slider>();
        }

        switch (sliderType)
        {
            case SliderType.Master:
                slider.value = PlayerPrefs.GetFloat("SliderMasterVolume");
                break;
            case SliderType.Music:
                slider.value = PlayerPrefs.GetFloat("SliderMusicVolume");
                break;
            case SliderType.SFX:
                slider.value = PlayerPrefs.GetFloat("SliderSFXVolume");
                break;
        }
    }
    public void OnChangeSlider(float Value)
    {
        switch(sliderType)
        {
            case SliderType.Master:
                AudioManager.instance.LogMasterVolume(Value);
                break;
            case SliderType.Music:
                AudioManager.instance.LogMusicVolume(Value);
                break;
            case SliderType.SFX:
                AudioManager.instance.LogSFXVolume(Value);
                break;
        }
    }
}
