using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public class MainMenu : MonoBehaviour
{
    
    [SerializeField] TutorialSlideShow slideShow;
    [SerializeField] RectTransform buttonsPanel;
    public List<Button> buttons = new List<Button>();
    public List<UIButtonScaleSpring> buttonScaleSprings = new List<UIButtonScaleSpring>();
    public List<CanvasGroup> canvasGroups = new List<CanvasGroup>();
    int buttonIndex = 0;
    Color unselectedButtonColour;
    Color selectedButtonColour;
    float unselectedAlphaValue = 0.75f;
    private void Awake()
    {
        buttonIndex = 0;

        buttons = buttonsPanel.GetComponentsInChildren<Button>().ToList();
        buttonScaleSprings = buttonsPanel.GetComponentsInChildren<UIButtonScaleSpring>().ToList();

        canvasGroups = buttonsPanel.GetComponentsInChildren<CanvasGroup>().ToList();
        unselectedButtonColour = new Color(0.8f, 0.8f, 0.8f, 1); 
        selectedButtonColour = Color.white;


        //SetCurrentButton(0);
        //AudioManager.instance.LogMusicVolume(PlayerPrefs.GetFloat("SliderMasterVolume"));
        //AudioManager.instance.LogMusicVolume(PlayerPrefs.GetFloat("SliderSFXVolume"));
    }
    private void Start()
    {
        AllButtonsInactive();
        SetCurrentButton(0);

        //AudioManager.instance.SetMusicVolume();
        // AudioManager.instance.SetSFXVolume();        
    }

    public void OnUpButtonPress(InputAction.CallbackContext context)
    {       
        if (context.action.WasPerformedThisFrame())
        {
            DecreaseButtonIndex();
            //Debug.Log("Up");
        }
    }
    public void OnDownButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            IncreaseButtonIndex();
        }
    }
    public void OnLeftButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            slideShow.NextPanel();
        }
    }
    public void OnRightButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            slideShow.PrevPanel();
        }
    }
    public void OnConfirmButtonPress(InputAction.CallbackContext context)
    {
        //Debug.Log("Button Press");
        buttonScaleSprings[buttonIndex].OnButtonPressNudge();
        buttons[buttonIndex].onClick.Invoke();
    }
    public void OnCancelButtonPress(InputAction.CallbackContext context)
    {

    }
    public void IncreaseButtonIndex()
    {
        buttonScaleSprings[buttonIndex].OnButtonHoverExitScale();
        canvasGroups[buttonIndex].alpha = unselectedAlphaValue;
        buttons[buttonIndex].image.color = unselectedButtonColour;
        if (buttonIndex == buttons.Count - 1)
        {
            buttonIndex = 0;
        }
        else
        {
            buttonIndex++;
        }
        //buttonIndex = Mathf.Clamp(buttonIndex, 0, buttons.Count - 1);
        buttonScaleSprings[buttonIndex].OnButtonHoverEnterScale();
        buttons[buttonIndex].image.color = selectedButtonColour;
        canvasGroups[buttonIndex].alpha = 1.0f;
    }

    public void DecreaseButtonIndex()
    {
        buttonScaleSprings[buttonIndex].OnButtonHoverExitScale();
        buttons[buttonIndex].image.color = unselectedButtonColour;
        canvasGroups[buttonIndex].alpha = unselectedAlphaValue;
        if (buttonIndex == 0)
        {
            buttonIndex = buttons.Count - 1;
        }
        else
        {
            buttonIndex--;
        }
        //buttonIndex = Mathf.Clamp(buttonIndex, 0, buttons.Count - 1);
        buttonScaleSprings[buttonIndex].OnButtonHoverEnterScale();
        buttons[buttonIndex].image.color = selectedButtonColour;
        canvasGroups[buttonIndex].alpha = 1.0f;
    }
    
    public void AllButtonsInactive()
    {
        for (int i = 0; i < buttons.Count; i++)
        {
            buttonScaleSprings[i].OnButtonHoverExitScale();
            canvasGroups[i].alpha = unselectedAlphaValue;
            buttons[i].image.color = unselectedButtonColour;

        }
    }
    
    public void SetCurrentButton(int index)
    {
        buttonIndex = index;
        buttonScaleSprings[buttonIndex].OnButtonHoverEnterScale();
        buttons[buttonIndex].image.color = selectedButtonColour;
        canvasGroups[buttonIndex].alpha = 1.0f;
    }
    private void SetButton0()
    {
        //SetCurrentButton(0);
        
    }
}
