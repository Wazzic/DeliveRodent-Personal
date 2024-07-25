using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spring.Runtime;
using DG.Tweening;

public class Prompt : MonoBehaviour
{
    CanvasGroup canvasGroup;
    RectTransform rectTransform;

    SpringToScale scaleSpring;

    public bool interruptToken;
    bool isShowing;

    public TextMeshProUGUI promptText;

    [SerializeField] private float delay = 3.5f;
    
    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        

        rectTransform = GetComponent<RectTransform>();

        scaleSpring = GetComponent<SpringToScale>();

        interruptToken = false;
        isShowing = false;

        promptText = GetComponent<TextMeshProUGUI>();
    }
    private void Start()
    {
        canvasGroup.interactable = false;
        canvasGroup.alpha = 0.0f;

        rectTransform.localScale = 0.1f * Vector3.one;
    }
    public void ShowPromptSpringTo()
    {
        if (!isShowing)
        {
            isShowing = true;
            
            //rectTransform.localScale = 0.01f * Vector3.one;
            
            scaleSpring.SpringTo(Vector3.one);        
            //scaleSpring.Nudge(Vector3.one * 10);

            canvasGroup.DOFade(1.0f, 1); 
                //Can add onComplete here

            if (!interruptToken)
            {
                StartCoroutine(TimeDelay());
            }
        }
    }
    //Tweening RectTrans needs DORewind to work. use ShowPromptSpringTo instead!!
    public void ShowPrompt()
    {
        if (!isShowing)
        {
            rectTransform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutElastic);
            canvasGroup.DOFade(1.0f, 0.5f);

            if (!interruptToken)
            {
                StartCoroutine(TimeDelay());
            }

            isShowing = true;
        }
    }
    public void HidePrompt()
    {
        if (isShowing)
        {
            scaleSpring.SpringTo(0.05f * Vector3.one);
            canvasGroup.DOFade(0.0f, 0.3f);    //Add onComplete here

            isShowing = false;
        }
    }
    private IEnumerator TimeDelay()
    {
        yield return new WaitForSeconds(delay);
        HidePrompt();
        
    }
    public void ChangeText(string newText)
    {
        if (promptText != null)
        {
            promptText.text = newText;
        }
    }
}
