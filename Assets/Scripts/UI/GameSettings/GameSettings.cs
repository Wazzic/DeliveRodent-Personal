using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class GameSettings : MonoBehaviour
{
    public int buttonIndex = 0;

    [SerializeField] SetRoundTime setRoundTime;
    //[SerializeField] SetActiveZones setActiveZones;
    //[SerializeField] Toggle muteToggle;

    public List<UIButtonScaleSpring> scaleSprings = new List<UIButtonScaleSpring>();
    public List<CanvasGroup> canvasGroups = new List<CanvasGroup>();

    private void Start()
    {
        scaleSprings = GetComponentsInChildren<UIButtonScaleSpring>().ToList();
        canvasGroups = GetComponentsInChildren<CanvasGroup>().ToList();

        DecreaseAll();
        DecreaseAlphaAll();

        IncreaseSelected();
        IncreaseAlphaSelected();
    }
    public void OnUpButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            DecreaseButtonIndex();
            Debug.Log("ButtonPress");
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
            if (buttonIndex == 0)
            {
                setRoundTime.DecreaseRoundTime();
            }
            else if (buttonIndex == 1)
            {
                //setActiveZones.DecreaseActiveZones();
            }
            else if (buttonIndex == 2)
            {
                //muteToggle.isOn = !muteToggle.isOn;
            }
        }
    }
    public void OnRightButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            if (buttonIndex == 0)
            {
                setRoundTime.IncreaseRoundTime();
            }
            else if (buttonIndex == 1)
            {
                //setActiveZones.IncreaseActiveZones();
            }
            else if (buttonIndex == 2)
            {
                //muteToggle.isOn = !muteToggle.isOn;
            }
        }
    }
    public void OnConfirmButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            GameManager.instance.ChangeScene(4);
        }
    }
    public void OnCancelButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            GameManager.instance.ChangeScene(1);
        }
    }
    private void IncreaseButtonIndex()
    {
        //buttonScaleSprings[buttonIndex].OnButtonHoverExitScale();
        DecreaseAll();
        DecreaseAlphaAll();
        if (buttonIndex >= canvasGroups.Count-1)
        {
            buttonIndex = 0;
        }
        else
        {
            buttonIndex++;
        }
        //buttonIndex = Mathf.Clamp(buttonIndex, 0, buttons.Count - 1);
        //buttonScaleSprings[buttonIndex].OnButtonHoverEnterScale();
        IncreaseSelected();
        IncreaseAlphaSelected();
    }
    private void DecreaseButtonIndex()
    {
        //buttonScaleSprings[buttonIndex].OnButtonHoverExitScale();
        DecreaseAll();
        DecreaseAlphaAll();
        if (buttonIndex <= 0)
        {
            buttonIndex = canvasGroups.Count-1;
        }
        else
        {
            buttonIndex--;
        }
        //buttonIndex = Mathf.Clamp(buttonIndex, 0, buttons.Count - 1);
        //buttonScaleSprings[buttonIndex].OnButtonHoverEnterScale();
        IncreaseSelected();
        IncreaseAlphaSelected();
    }
    private void DecreaseAll()
    {
        foreach (UIButtonScaleSpring spring in scaleSprings)
        {
            spring.OnButtonHoverExitScale();            
        }
    }
    private void DecreaseAlphaAll()
    {
        foreach (CanvasGroup canvas in canvasGroups)
        {
            canvas.alpha = 0.8f;
        }
    }
    private void IncreaseSelected()
    {
        if (buttonIndex < 2)
        {
            scaleSprings[3 * buttonIndex].OnButtonHoverEnterScale();
            scaleSprings[(3 * buttonIndex) + 1].OnButtonHoverEnterScale();
            //scaleSprings[(3 * buttonIndex) + 2].OnButtonHoverEnterScale();
        }
        else
        {
            scaleSprings[buttonIndex + 4].OnButtonHoverEnterScale();
        }
    }
    private void IncreaseAlphaSelected()
    {
        canvasGroups[buttonIndex].alpha = 1.0f;
    }
}
