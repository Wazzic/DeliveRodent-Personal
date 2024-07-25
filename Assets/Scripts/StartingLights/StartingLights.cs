using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Spring;
using Spring.Runtime;
using System.Threading;

public class StartingLights : MonoBehaviour
{
    [SerializeField] AnalogueClock analogueClock;
    //[SerializeField] DigitalClock digitalClock;
    private SpringToScale scaleSpring;
    private CanvasGroup startingLightsGroup;
    //[SerializeField] private CanvasGroup scoreGroup;
    private TextMeshProUGUI countdownText;
    private int countdown;
    
    // Start is called before the first frame update
    void Start()
    {
        //scoreGroup.alpha = 0.0f;
        startingLightsGroup = GetComponent<CanvasGroup>();
        countdownText = GetComponentInChildren<TextMeshProUGUI>();
        scaleSpring = GetComponent<SpringToScale>();
        countdown = 5;

        /*
        #if UNITY_EDITOR
            if (GameManager.instance.StartTimer)
            {
            //StartCoroutine(StartLights());
                Invoke("DelayThenStart", 1f);
            }
            else
            {
                GiveControl();
            }
        #else
             //StartCoroutine(StartLights());
             Invoke("DelayThenStart, 1f);
        #endif
        */
        Invoke("DelayThenStart", 1.5f);
    }
    private void GiveControl()
    {
        InputManager.instance.controlsEnabled = true;
        startingLightsGroup.alpha = 0.0f;
        startingLightsGroup.interactable = false;
        analogueClock.StartTimer();
    }
    private void DelayThenStart()
    {
        StartCoroutine(StartLights());
    }
    private IEnumerator StartLights()
    {
        for (int i = 3; i > -1; i--)
        {
            yield return new WaitForSeconds(1.0f);
            countdown = i;
            UpdateCountdownText();
        }
        InputManager.instance.controlsEnabled = true;
        scaleSpring.SpringTo(0.5f * Vector3.one);
        yield return new WaitForSeconds(1.0f);
        startingLightsGroup.alpha = 0.0f;
        startingLightsGroup.interactable = false;
        //scoreGroup.alpha = 1.0f;
        analogueClock.StartTimer();
    }
    private void UpdateCountdownText()
    {
        countdownText.text = countdown.ToString();
        NudgeScale();
    }
    private void NudgeScale()
    {
        scaleSpring.Nudge(new Vector3(15, 10, 10));
    }
}
