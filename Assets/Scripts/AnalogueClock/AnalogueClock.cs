using Spring.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnalogueClock : MonoBehaviour
{
    //float startTime;

    public float timer = 0.0f;
    //float springTimer = 0.0f;

    bool isTimer = false;
    bool isFinished = false;
    [SerializeField] public bool infiniteTime;

    public Slider timeSlider;

    CanvasGroup canvasGroup;

    //SpringToScale scaleSpring;
    [SerializeField] LowTimeUIManager lowTimeManager;

    //public List<PlayerTimePromptController> timePrompts = new List<PlayerTimePromptController>();
    [SerializeField] Prompt overTimePrompt;

    private void Start()
    {
        //overTimePrompt = GetComponentInChildren<Prompt>();
            //If this isnt the only prompt in children, this will probs create error
        
        //timer = startTime;

        timeSlider = GetComponent<Slider>();
        timeSlider.minValue = 0;
        timeSlider.maxValue = GameManager.instance.playerConfigs.roundTime * 60;

        isFinished = false;

        canvasGroup = GetComponentInParent<CanvasGroup>();
        /*
        if (GameManager.instance.playerConfigs.numberOfPlayers == 1)
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
        }
        else
        {
            canvasGroup.alpha = 1.0f;
            canvasGroup.interactable = false;
        }
        */
        canvasGroup.alpha = 0.0f;

        //scaleSpring = GetComponent<SpringToScale>();
        
        if (GameManager.instance.playerConfigs.numberOfPlayers == 1)
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
        }
        infiniteTime = false;

        //StartCoroutine(WaitThenFindPrompts());
        
    }

    private void Update()
    {
        if (isTimer)
        {
            timer += Time.deltaTime;
        }
        timeSlider.value = timer;
    }
    private void LateUpdate()
    {
        if (timeSlider.normalizedValue == 1 && !infiniteTime)
        {
            isFinished = true;
            //Debug.Log("Timer has run out");
            DeliveryManager.instance.spawningPoints = false;
            lowTimeManager.lowTime = true;

            overTimePrompt.ShowPromptSpringTo();

            lowTimeManager.lowTime = true;
            lowTimeManager.oneFrame = true;
            /*
            foreach (PlayerTimePromptController promptController in timePrompts)
            {
                promptController.ShowOvertime();

            }
            */
            /*
            if(DeliveryManager.instance.ActiveAmount > 1)
            {
                GameManager.instance.gameHandler.EndGame();
            }
            else if (!DeliveryManager.instance.spawningPoints)
            {
                GameManager.instance.gameHandler.EndGame();
            }
            */
        }  
        /*
        else if (Mathf.Abs(timeSlider.maxValue - timeSlider.value) < 60 && !infiniteTime)
        {
            foreach(PlayerTimePromptController promptController in timePrompts)
            {

            }
        }
        */
    }
    public void StartTimer()
    {
        isTimer = true;
    }
    public void CheckForEndTime()
    {
        if (timeSlider.normalizedValue == 1 && !infiniteTime)
        {

        }
    }
    /*
    private void SpringTimer()
    {
        if (springTimer > 0.5)
        {
            springTimer = 0.0f;
            scaleSpring.Nudge(new Vector3(5, 0.2f, 5));
            alarmBellRotSpring[0].Nudge(new Vector3(0, 0, 180));
            alarmBellRotSpring[1].Nudge(new Vector3(0, 0, 180));
        }
        else
        {
            springTimer += Time.deltaTime;
        }
    }
    private void StartAlarmScaleSpring()
    {
        scaleSpring.Nudge(new Vector3(5, 0.2f, 5));
    }
    */
    private IEnumerator WaitThenFindPrompts()
    {
        yield return new WaitForSeconds(1);
        //timePrompts = FindObjectsOfType<PlayerTimePromptController>().ToList();
    }
}
