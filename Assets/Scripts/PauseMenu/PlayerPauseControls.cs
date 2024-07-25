using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerPauseControls : MonoBehaviour
{
    [SerializeField]
    PauseMenu pauseMenu;
    [SerializeField]
    MainMenu pauseControl;
    [SerializeField]
    SettingsScreen settings;
    private void Start()
    {
        pauseMenu = Resources.FindObjectsOfTypeAll<PauseMenu>()[0];
        pauseControl = Resources.FindObjectsOfTypeAll<MainMenu>()[0];
        settings = Resources.FindObjectsOfTypeAll<SettingsScreen>()[0];
    }
    public void OnConfirmButtonPress(InputAction.CallbackContext context)
    {
        /*
        foreach(MainMenu menu in pauseControl)
        {
            if (menu.gameObject.activeSelf && context.action.WasPerformedThisFrame())
            {
                menu.OnConfirmButtonPress(context);
                break;
            }       
        }
        */
        if (pauseControl.gameObject.activeSelf && context.action.WasPerformedThisFrame())
        {
            pauseControl.OnConfirmButtonPress(context);
        }
    }

    public void OnCancelButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            pauseMenu.OnPauseButtonPress();
        }
    }

    public void OnUpButtonPress(InputAction.CallbackContext context)
    {
        /*
        foreach (MainMenu menu in pauseControl)
        {
            if (menu.gameObject.activeSelf)
            {
                menu.OnUpButtonPress(context);
                break;
            }
        }

        if (settings.gameObject.activeSelf)
        {
            settings.OnUpButtonPress(context);
        }
        */
        if (pauseControl.gameObject.activeSelf)
        {
            pauseControl.OnUpButtonPress(context);
        }
        else if (settings.gameObject.activeSelf)
        {
            settings.OnUpButtonPress(context);
        }
    }

    public void OnDownButtonPress(InputAction.CallbackContext context)
    {
        /*
        foreach (MainMenu menu in pauseControl)
        {
            if (menu.gameObject.activeSelf)
            {
                menu.OnDownButtonPress(context);
                break;
            }
        }

        if (settings.gameObject.activeSelf)
        {
            settings.OnDownButtonPress(context);
        }
        */
        if (pauseControl.gameObject.activeSelf)
        {
            pauseControl.OnDownButtonPress(context);
        }
        else if (settings.gameObject.activeSelf)
        {
            settings.OnDownButtonPress(context);
        }
    }

    public void OnRightButtonPress(InputAction.CallbackContext context)
    {
        if (settings.gameObject.activeSelf)
        {
            settings.OnRightButtonPress(context);
        }
    }

    public void OnLeftButtonPress(InputAction.CallbackContext context)
    {
        if (settings.gameObject.activeSelf)
        {
            settings.OnLeftButtonPress(context);
        }
    }

    public void OnPauseButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            pauseMenu.OnPauseButtonPress();
        }       
    }
}
