using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyOnLoadMusic : MonoBehaviour
{
    public static AudioSource audioSource;
    public static DontDestroyOnLoadMusic instance;
    [SerializeField] AudioClip lobbyMusic, inGameMusic;
    private void Awake()
    {
        if (instance == null)
        {
            audioSource = GetComponent<AudioSource>();
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        DontDestroyOnLoad(this.gameObject);
    }
    public void ChangeMusicLobby()
    {
        audioSource.clip = lobbyMusic;
        audioSource.Play();
    }
    public void ChangeMusicInGame()
    {
        audioSource.clip = inGameMusic;
        audioSource.Play();
    }
    public void DisableMusic()
    {
        audioSource.Stop();
    }
}
