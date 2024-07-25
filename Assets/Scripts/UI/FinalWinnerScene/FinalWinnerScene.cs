using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using System.Linq;

public class FinalWinnerScene : MonoBehaviour
{
    private int buttonIndex;
    public List<UIButtonScaleSpring> scaleSprings = new List<UIButtonScaleSpring>();
    public List<Button> buttons = new List<Button>();
    [SerializeField] ShowScores checkCompletion;

    float time;

    private void Start()
    {
        buttonIndex = 0;
        scaleSprings = GetComponentsInChildren<UIButtonScaleSpring>().ToList();
        buttons = GetComponentsInChildren<Button>().ToList();

        IncreaseSelected();

        time = 0.0f;
    }
    private void Update()
    {
        if (time < 2.0f)
        {
            time += Time.deltaTime;
        }
        else
        {
            InputManager.instance.controlsEnabled = true;
        }
    }
    public void OnLeftButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame() && InputManager.instance.controlsEnabled)
        {
            DecreaseButtonIndex();
        }
    }
    public void OnRightButtonPress(InputAction.CallbackContext context)
    {
        if (context.action.WasPerformedThisFrame() && InputManager.instance.controlsEnabled)
        {
            IncreaseButtonIndex();
        }
    }
    public void OnConfirmButtonPress(InputAction.CallbackContext context)
    {
        
        if (context.action.WasPerformedThisFrame() && InputManager.instance.controlsEnabled)
        {
            if(checkCompletion.scoresComplete == true)
            {
                buttons[buttonIndex].onClick.Invoke();
            }
            else
            {
                return;
            }
        }
    }
    private void DecreaseButtonIndex()
    {
        DecreaseAll();
        buttonIndex--;
        if (buttonIndex < 0)
        {
            buttonIndex = 1;
        }
        IncreaseSelected();
    }
    private void IncreaseButtonIndex()
    {
        DecreaseAll();
        buttonIndex++;
        if (buttonIndex > 1)
        {
            buttonIndex = 0;
        }
        IncreaseSelected();
    }
    private void DecreaseAll()
    {
        foreach (UIButtonScaleSpring spring in scaleSprings)
        {
            spring.OnButtonHoverExitScale();
        }
    }
    private void IncreaseSelected()
    {
        scaleSprings[buttonIndex].OnButtonHoverEnterScale();
    }
}
