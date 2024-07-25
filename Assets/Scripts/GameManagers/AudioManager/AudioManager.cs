using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;
    private List<AudioMixerGroup> mGroups = new List<AudioMixerGroup>();

    public float masterValue;
    public float musicValue;
    public float sFXValue;
    public static AudioManager instance;
    private void Start()
    {
       
        LogMasterVolume(PlayerPrefs.GetFloat("SliderMasterVolume", 1f)); //set default value to 1
        LogMusicVolume(PlayerPrefs.GetFloat("SliderMusicVolume", 1f)); //set default value to 1
        LogSFXVolume(PlayerPrefs.GetFloat("SliderSFXVolume", 1f)); //set default value to 1

        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }
    }
    public void LogMasterVolume(float value)
    {
        float newValue = Mathf.Log10(value) * 20f;
        mixer.SetFloat("MasterVolume", newValue);
        PlayerPrefs.SetFloat("SliderMasterVolume", value);
        masterValue = value;
    }
    
    public void LogMusicVolume(float value)
    {
        float newValue = Mathf.Log10(value) * 20f;
        mixer.SetFloat("MusicVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SliderMusicVolume", value);
        musicValue = value;
    }
    public void LogSFXVolume(float value)
    {
        float newValue = Mathf.Log10(value) * 20f;
        mixer.SetFloat("SFXVolume", Mathf.Log10(value) * 20);
        PlayerPrefs.SetFloat("SliderSFXVolume", value);
        sFXValue = value;
    }
    public void SetMasterVolume()
    {
        LogMasterVolume(masterValue);
    }
    public void SetMusicVolume()
    {
        LogMusicVolume(musicValue);
    }
    public void SetSFXVolume()
    {
        LogSFXVolume(sFXValue);
    }
}
