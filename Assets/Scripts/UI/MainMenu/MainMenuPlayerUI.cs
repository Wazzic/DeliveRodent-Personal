using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MainMenuPlayerUI : MonoBehaviour
{
    MainMenu mainMenu;
    private void Start()
    {
        InputManager.instance.OnNewInput(this.GetComponent<PlayerInput>());
    }

    public void OnUpArrow(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            Debug.Log("Nutton");
            mainMenu.DecreaseButtonIndex();
        }
    }
    public void OnDownButtonPress(InputAction.CallbackContext context)
    {

    }
    public void OnLeftButtonPress(InputAction.CallbackContext context)
    {

    }
    public void OnRightButtonPress(InputAction.CallbackContext context)
    {

    }
    public void OnConfirmButtonPress(InputAction.CallbackContext context)
    {

    }
    public void OnCancelButtonPress(InputAction.CallbackContext context)
    {

    }

}
