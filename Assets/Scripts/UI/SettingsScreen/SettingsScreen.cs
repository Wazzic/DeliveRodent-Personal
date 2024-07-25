using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class SettingsScreen : MonoBehaviour
{
    public List<Slider> sliders;
    int sliderIndex;

    public List<UIButtonScaleSpring> sliderScaleSprings;
    [SerializeField] List<CanvasGroup> canvasGroups;

    public bool isPressed;

    private void Awake()
    {
        sliderIndex = 0;

        sliders = new List<Slider>();
        sliders = GetComponentsInChildren<Slider>().ToList();
        /*
        foreach (Slider slider in sliders)
        {
            slider.value = 1.0f;
        }*/

        sliderScaleSprings = new List<UIButtonScaleSpring>();
        sliderScaleSprings = GetComponentsInChildren<UIButtonScaleSpring>().ToList();
        sliderScaleSprings[sliderIndex].OnButtonHoverEnterScale();

        /*
        canvasGroups = new List<CanvasGroup>();
        canvasGroups = GetComponentsInChildren<CanvasGroup>().ToList();
        */

        foreach (CanvasGroup canvas in canvasGroups)
        {
            canvas.alpha = 0.8f;
        }
        canvasGroups[0].alpha = 1.0f;
    }
    public void Start()
    {
        sliderScaleSprings[0].OnButtonHoverEnterScale();
        canvasGroups[0].alpha = 1.0f;
    }

    public void OnUpButtonPress(InputAction.CallbackContext context)
    {       
        if (context.action.WasPerformedThisFrame())
        {
            //Debug.Log("Up Button");
            DecreaseSliderIndex();
        }
    }
    public void OnDownButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            IncreaseSliderIndex();
        }
    }
    public void OnLeftButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            DecreaseSliderValue();
        } 
        if (context.action.IsPressed())
        {
            isPressed = true;
            //DecreaseSliderValue();
        }
        else
        {
            isPressed = false;
        }
    }
    public void OnRightButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            IncreaseSliderValue();
        }
    }
    public void OnConfirmButtonPress(InputAction.CallbackContext context)
    {
        /*
        Debug.Log("Button Press");
        buttonScaleSprings[buttonIndex].OnButtonPressNudge();
        buttons[buttonIndex].onClick.Invoke();
        */
    }
    public void OnCancelButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame())
        {
            //Debug.Log("Cancel Button Press");
            PlayerPrefs.SetFloat("SliderMusicVolume", sliders[0].value);
            PlayerPrefs.SetFloat("SliderSFXVolume", sliders[1].value);
            GameManager.instance.ChangeScene(1);
        }
    }
    private void IncreaseSliderIndex()
    {        
        sliderScaleSprings[sliderIndex].OnButtonHoverExitScale();
        canvasGroups[sliderIndex].alpha = 0.8f;
        if (sliderIndex == sliders.Count - 1)
        {
            sliderIndex = 0;
        }
        else
        {
            sliderIndex++;
        }
        //buttonIndex = Mathf.Clamp(buttonIndex, 0, buttons.Count - 1);
        sliderScaleSprings[sliderIndex].OnButtonHoverEnterScale();
        canvasGroups[sliderIndex].alpha = 1.0f;
    }
    private void DecreaseSliderIndex()
    {        
        sliderScaleSprings[sliderIndex].OnButtonHoverExitScale();
        canvasGroups[sliderIndex].alpha = 0.8f;
        if (sliderIndex == 0)
        {
            sliderIndex = sliders.Count - 1;
        }
        else
        {
            sliderIndex--;
        }
        //buttonIndex = Mathf.Clamp(buttonIndex, 0, buttons.Count - 1);
        sliderScaleSprings[sliderIndex].OnButtonHoverEnterScale();
        canvasGroups[sliderIndex].alpha = 1.0f;
        
    }
    private void IncreaseSliderValue()
    {
        sliders[sliderIndex].value += 0.1f;
        sliderScaleSprings[sliderIndex].OnButtonPressNudge();
    }
    private void DecreaseSliderValue()
    {
        Debug.Log("i have been pressed!");
        sliders[sliderIndex].value -= 0.1f;
        sliderScaleSprings[sliderIndex].OnButtonPressNudge();
    }
}
