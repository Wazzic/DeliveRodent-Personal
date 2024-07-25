using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Rendering;

public class CarAudioHandler : MonoBehaviour
{
    ArcadeVehicleController arcadeVehicleController;
    SceneAudioManager audioManager;
    //SmellTrailController smellTrail;

    [Header("Audio Sources")]
    [SerializeField] private AudioSource oneTimeAudioSource;
    [Header("Clips")]
    [SerializeField] private AudioClip bumpClip;
    [SerializeField] private AudioClip Bounce;
    [SerializeField] private AudioClip Swoosh;
    [SerializeField] private AudioClip StealVoice;
    [SerializeField] private AudioClip PickUp;
    [SerializeField] private AudioClip DropOff;
    [SerializeField] private AudioClip SlipStream;
    [SerializeField] private AudioClip CarHonk;
    [SerializeField] private AudioClip Yay;
    [SerializeField] private AudioClip Woah;
    [SerializeField] private AudioClip Horn;

    [SerializeField] private AudioMixer mixer;

    void Awake()
    {
        audioManager = FindFirstObjectByType<SceneAudioManager>();
        arcadeVehicleController = GetComponentInParent<ArcadeVehicleController>();
        //smellTrail = GetComponentInChildren<SmellTrailController>();
        //skidAudioSource.mute = true;
        audioManager.AddPlayerAudio(arcadeVehicleController);
    }
    //void Update()
    //{
    //    //ManageCarSounds();
    //    //SmellTrailSound();
    //    //PlayLampPostBumpSound();
    //}
    public void ManageCarSounds()
    {
    }
    public void PlayObstacleSound(AudioClip clip)
    {
        oneTimeAudioSource.pitch = Random.Range(0.90f, 1.1f);
        oneTimeAudioSource.PlayOneShot(clip, 0.2f);
    }
    public void PlayPedestrianCarSound()
    {
        // oneTimeAudioSource.PlayOneShot(pedestrianCarClip, 1f);
        oneTimeAudioSource.pitch = Random.Range(0.90f, 1.1f);
        oneTimeAudioSource.PlayOneShot(bumpClip, 0.2f);

    }
    public void PlayOtherPlayerCarSound()
    {
        oneTimeAudioSource.pitch = Random.Range(0.90f, 1.1f);
        oneTimeAudioSource.PlayOneShot(bumpClip, 0.2f);
    }
    public void PlayDefaultCrashSound() // wall or house
    {
        oneTimeAudioSource.pitch = Random.Range(0.90f, 1.1f);
        oneTimeAudioSource.PlayOneShot(bumpClip, 0.2f);

    }
    public void PlaySwoosh()
    {
        oneTimeAudioSource.PlayOneShot(Swoosh, 1f);
    }
    public void PlayAttackNotSteal()
    {
        oneTimeAudioSource.PlayOneShot(bumpClip, 0.2f);
    }
    public void PlayStealAudio()
    {
        oneTimeAudioSource.PlayOneShot(StealVoice, 1f);
    }
    public void PlayPickUpAudio()
    {
        oneTimeAudioSource.PlayOneShot(PickUp, 1f);
    }
    public void PlayDropOffAudio()
    {
        oneTimeAudioSource.PlayOneShot(DropOff, 1.5f);
        //oneTimeAudioSource.PlayOneShot(Yay, 1f); //yay has audio pop/peak rn
    }
    public void PlayHornSound()
    {
        oneTimeAudioSource.PlayOneShot(Horn, 1f);
    }
    /*public void SmellTrailSound()
    {
        if(smellTrail.trailRenderer.enabled)
        {
            Debug.Log("CrossPlaysound");
            oneTimeAudioSource.PlayOneShot(SlipStream);
        }
    }*/
}