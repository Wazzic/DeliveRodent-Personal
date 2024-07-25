using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ParticleEffectGroup : MonoBehaviour
{
    public List<ParticleSystem> effectGroup;
    bool isPlaying;

    private void Awake()
    {
        effectGroup = new List<ParticleSystem>();
        effectGroup = GetComponentsInChildren<ParticleSystem>().ToList();
    }
    public void Play()
    {
        Stop();
        if (!isPlaying)
        {
            isPlaying = true;
            foreach (ParticleSystem p in effectGroup)
            {
                p.Play();
            }
        }
    }
    public void PlayContinous()
    {
        if (!isPlaying)
        {
            isPlaying = true;
            foreach(ParticleSystem p in effectGroup)
            {
                p.Play();
            }
        }
    }
    public void Stop()
    {
        if (isPlaying)
        {
            isPlaying = false;
            foreach(ParticleSystem p in effectGroup)
            {
                p.Stop();
            }
        }
    }
}
