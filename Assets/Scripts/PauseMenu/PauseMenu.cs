using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject optionsScreen;
    public GameObject pauseScreen;
    //Used to be called in other scripts to disable certain functionality when paused
    public static bool isPaused;
    [SerializeField] CanvasGroup alarmClock;

    public PlayerInput[] p;

    //Used to call functions in other classes without the need to find or create the object in another script
    public static PauseMenu instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != null)
        {
            Destroy(this);
        }      
    }

    private void Start()
    {
        ResumeGame();
        optionsScreen.SetActive(false);     
    }


    //Pauses the game
    public void PauseGame()
    {
        //Debug.Log("PauseGame Triggered");
        foreach(PlayerInput i in InputManager.instance.playerInput)
        {
            i.SwitchCurrentActionMap("Menu");
        }

        pauseMenu.SetActive(true);
        MainMenu mm = pauseMenu.GetComponentInChildren<MainMenu>();
        mm.AllButtonsInactive();
        mm.SetCurrentButton(0);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 0f;
        isPaused = true;
        //alarmClock.alpha = 0.0f;
    }
    //Resumes the Game
    public void ResumeGame()
    {
        foreach (PlayerInput i in InputManager.instance.playerInput)
        {
            i.SwitchCurrentActionMap("CarController");
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        pauseMenu.SetActive(false);

        Time.timeScale = 1f;
        isPaused = false;
        if (GameManager.instance.playerConfigs.numberOfPlayers > 1)
        {
            alarmClock.alpha = 1.0f;
        }
    }

    public void OnPauseButtonPress()
    {
        if (isPaused)
        {
            if (optionsScreen.activeSelf)
            {
                OptionsReturn();               
            }
            else
            {
                ResumeGame();
            }           
        }
        else
        {
            PauseGame();
        }
    }

    //Sends the user to the options menu
    public void Options()
    {
        optionsScreen.SetActive(true);
        pauseScreen.SetActive(false);
    }

    public void OptionsReturn()
    {       
        pauseScreen.SetActive(true);
        optionsScreen.SetActive(false);
    }

    //Returns to the main menu
    public void MainMenu()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        GameManager.instance.ChangeScene(1);
    }
    //Exits the game
    public void QuitGame()
    {
        Application.Quit();
    }
}
