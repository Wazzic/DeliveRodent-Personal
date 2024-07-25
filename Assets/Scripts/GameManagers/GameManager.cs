using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] public bool StartTimer;
    private List<string> sceneNames;
    [SerializeField] GameManagerConfig config;
    public GameHandler gameHandler { get; private set; }

    [SerializeField] GameObject fadeOut;
    [SerializeField] GameObject fadeIn;
    public int numberOfPlayers { get; private set; }

    [SerializeField] public PlayerConfigs playerConfigs;
    
    //[SerializeField] private SceneNames sceneNames;

    public static GameManager instance;

    bool changingScene = false;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }

        sceneNames = config.sceneNames;

        gameHandler = GetComponent<GameHandler>();        
    }

    private void Start()
    {
        /*
        if (SceneManager.GetActiveScene().name == sceneNames[5])
        {
            //to lock in the centre of window
            Cursor.lockState = CursorLockMode.Locked;
            //to hide the curser
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        */

        if (SceneManager.GetActiveScene().name == sceneNames[1] || SceneManager.GetActiveScene().name == sceneNames[2] || SceneManager.GetActiveScene().name == sceneNames[3])
        {
            fadeIn.SetActive(false);
        }
        else
        {
            fadeIn.SetActive(true);
        }
    }

    public void SetGameHandlerActive()
    {
        gameHandler.gameObject.SetActive(true);
    }
    public void ChangeScene(int sceneIndex)
    {

        if(changingScene == true)
        {
            return;
        }
        changingScene = true;

        if(sceneIndex ==1 || sceneIndex ==2 || sceneIndex == 3)
        {
            ChangeMusic(sceneIndex);
            SceneManager.LoadScene(sceneNames[sceneIndex]);
        }
        else
        {

           
             StartCoroutine(test(sceneIndex));
        }
       
    }

    IEnumerator test(int sceneID)
    {
        //yield return null;
        ChangeMusic(sceneID);
        fadeOut.SetActive(true);

        AsyncOperation asyncOperation =  SceneManager.LoadSceneAsync(sceneNames[sceneID]);
        asyncOperation.allowSceneActivation = false;

        while (!asyncOperation.isDone)
        {
            if(fadeOut.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f)
            {
                asyncOperation.allowSceneActivation = true;
            }
            yield return null;
        }
    }

    public void QuitAppllication()
    {
        Application.Quit();
    }
    private void ChangeMusic(int index)
    {
        switch (index)
        {
            //Changing to Game Scene
            case 5:
                if (DontDestroyOnLoadMusic.instance != null)
                {
                    DontDestroyOnLoadMusic.instance.ChangeMusicInGame();

                }
                break;
            //Changing to main menu scene
            case 1:
                //Checks to see if its switching from the Game scene or End Game Scene
                if(SceneManager.GetActiveScene().name != sceneNames[5] && SceneManager.GetActiveScene().name != sceneNames[7])
                {
                    break;
                }
                DontDestroyOnLoadMusic.instance.ChangeMusicLobby();
                break;
            //Changing to end Game Scene
            case 6:
                DontDestroyOnLoadMusic.instance.DisableMusic();
                break;
        }
    }
}
