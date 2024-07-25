using UnityEngine;
using System;
using UnityEngine.Audio;

public class AudioManagerV2 : MonoBehaviour
{

    public Sound[] sounds;

    public static AudioManagerV2 instance;

    void Awake()
    {
        #region set up singelton and ensure persistency across scenes
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        DontDestroyOnLoad(gameObject);
        #endregion

        #region loop through and initialise sound characteristics
        foreach (Sound sound in sounds)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.pitch = sound.pitch;
            sound.source.loop = sound.loop;
            sound.source.spatialBlend = sound.spatialBlend;
            //sound.source.dopplerLevel = sound.dopplerLevel;
        }
        #endregion

        instance.Play("lol");
    }

    public void Play(string fileName)
    {
        Sound currSound = Array.Find(sounds, sound => sound.name == fileName);
        if(currSound == null)
        {
            Debug.LogWarning("Incorrect file name - please check the spelling for: " + fileName);
            return;
        }
        currSound.source.Play();
    }
}
   
