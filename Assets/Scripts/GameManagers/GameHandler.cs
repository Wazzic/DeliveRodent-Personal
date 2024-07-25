using UnityEngine;
using UnityEngine.Audio;
//using Lofelt.NiceVibrations;

public class GameHandler : MonoBehaviour
{
    /*
    //Stores the digital clock in the handler for use of game ending
    [SerializeField]
    DigitalClock digitalClock;
    */
    //Stores the audio source in the handler for use of game ending 9speeding music up)
    AudioSource audioSource;
    [SerializeField] AudioMixer mixer;
    //[SerializeField] CanvasGroup lowTimePanel;
    [SerializeField] public LowTimeUIManager lowTimeUIManager;

    //Start time for the game
    /*
    [SerializeField]
    float StartTime = 540f;

    //End time for the game
    [SerializeField]
    float EndTime = 1020f;
    */

    [SerializeField]
    private int sceneNumber = 0;
    //Functionality for ending game

    private int numberOfFrames;
    [SerializeField] int delay;

    [SerializeField] private FadePanel fadePanel;

    public void StartClock()
    {
        //digitalClock.StartTimer();
    }

    public void EndGame()
    {
        //fadePanel.FadeOut(2f, 7);
        for (int i = 0; i < GameManager.instance.numberOfPlayers; i++)
        {
            //GamepadRumbler.SetCurrentGamepad(i);
            //HapticController.Stop();
        }
        PlayerStats.Scores = ScoreManager.instance.PlayersScore();

        //AnalyticsManager.instance.SendEndGameAnalytics();
        //AnalyticsManager.instance.SendEndGameItemAnalytics();

        GameManager.instance.ChangeScene(7);
        //gameObject.SetActive(false);
    }
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
       // AudioManager.instance.LogMusicVolume(PlayerPrefs.GetFloat("SliderMusicVolume"));
       // AudioManager.instance.LogSFXVolume(PlayerPrefs.GetFloat("SliderSFXVolume"));
        //EndTime = StartTime + 60 * GameManager.instance.playerConfigs.roundTime;
        //digitalClock.SetTime(StartTime);
        //fadePanel.FadeIn(2);
    }

    //// Update is called once per frame
    //void Update()
    //{
        
    //    /*
    //    //check if there are 60 seconds left
    //    if (digitalClock.GetTimer() + 60 > EndTime)
    //    {
    //        lowTimeUIManager.StartLowTime();
    //        //pitch the music up
    //        audioSource.pitch = 1.25f;
    //    }
    //    //Checks to see if time has reached the end point
    //    if (digitalClock.GetTimer() >= EndTime)
    //    {
    //        EndGame();
    //    }
    //    */
    //}
}