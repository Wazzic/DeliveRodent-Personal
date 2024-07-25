using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadePanel : MonoBehaviour
{
    [SerializeField] private CanvasGroup fadePanel;
    public bool isFinished = false;

    private void Start()
    {
        isFinished = false;
    }

    public void FadeIn(float fadeTime)
    {
        StartCoroutine(TimerDown(fadeTime));
    }
    
    private IEnumerator TimerDown(float fadeTime)
    {
        isFinished = false;
        float time = 0.0f;
        if (time < 1)
        {
            time += Time.deltaTime / fadeTime;
            fadePanel.alpha = time;
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            isFinished = true;
        }
    }
    public void FadeOut(float fadeTime, int sceneNumber)
    {
        TimerUp(fadeTime);
        GameManager.instance.ChangeScene(sceneNumber);
    }
    private IEnumerator TimerUp(float fadeTime)
    {
        float time = 0.0f;
        if (time < 1)
        {
            time += Time.deltaTime / fadeTime;
            fadePanel.alpha = time;
            yield return new WaitForSeconds(0.1f);
        }
        else
        {
            isFinished = true;
        }
    }
}
