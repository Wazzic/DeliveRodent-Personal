using Spring.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class AnalogueClock : MonoBehaviour
{
    public float timer = 0.0f;

    bool isTimer = false;
    bool isFinished = false;
    [SerializeField] public bool infiniteTime;

    public Slider timeSlider;

    CanvasGroup canvasGroup;

    [SerializeField] LowTimeUIManager lowTimeManager;

    [SerializeField] Prompt overTimePrompt;

    private void Start()
    {
        timeSlider = GetComponent<Slider>();
        timeSlider.minValue = 0;
        timeSlider.maxValue = GameManager.instance.playerConfigs.roundTime * 60;

        isFinished = false;

        canvasGroup = GetComponentInParent<CanvasGroup>();

        canvasGroup.alpha = 0.0f;
        
        if (GameManager.instance.playerConfigs.numberOfPlayers == 1)
        {
            canvasGroup.alpha = 0.0f;
            canvasGroup.interactable = false;
        }
        infiniteTime = false;
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
            DeliveryManager.instance.spawningPoints = false;
            lowTimeManager.lowTime = true;

            overTimePrompt.ShowPromptSpringTo();

            lowTimeManager.lowTime = true;
            lowTimeManager.oneFrame = true;
        }  
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
    private IEnumerator WaitThenFindPrompts()
    {
        yield return new WaitForSeconds(1);
    }
}
