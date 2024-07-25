using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;


public class NewsTicker : MonoBehaviour
{
    RectTransform scrollBar;
    [SerializeField] GameObject scrollingText;
    public List<RectTransform> texts;

    [Range(50, 150)]
    public float scrollSpeed;

    private float scrollBarWidth;

    private bool isPlaying;

    private void Awake()
    {
        texts = new List<RectTransform>();

        scrollBar = GetComponent<RectTransform>();   
        scrollBarWidth = scrollBar.rect.width; 

        isPlaying = false;           
    }
    public void StartOvertime()
    {
        if (!isPlaying)
        {
            isPlaying = true;

            ShowScrollingBars();
            scrollBar.DOAnchorPosY(-(Mathf.MoveTowards(scrollBar.anchoredPosition.y, 0, 1)), 1, true);
        }
    }
    public void StopOverTime()
    {

    }
    private void ShowScrollingBars()
    {
        float scrollingBarWidth = scrollingText.GetComponent<RectTransform>().rect.width;
        int numberOfScrollingTexts = Mathf.FloorToInt(scrollBarWidth / scrollingBarWidth);
            //Mathf.RoundToInt(scrollBarWidth / scrollingBarWidth);
        for (int i = 0; i < numberOfScrollingTexts; i++)
        {
            RectTransform newText = Instantiate(scrollingText, this.transform).GetComponent<RectTransform>();
            texts.Add(newText);
            newText.anchoredPosition = new Vector2(scrollBar.rect.width - (i * newText.rect.width), newText.anchoredPosition.y);
            newText.DOAnchorPosX(scrollingBarWidth / 2, (newText.anchoredPosition.x / scrollSpeed), true)
                .SetEase(Ease.Linear).OnComplete(()=>RestartScrollingText(newText));
        }
    }
    private void RestartScrollingText(RectTransform scrollText)
    {
        float startPosX = scrollBarWidth - (scrollText.rect.width / 2);
        scrollText.anchoredPosition = new Vector2(startPosX, scrollText.anchoredPosition.y);
        scrollText.DOAnchorPosX(scrollText.rect.width / 2, startPosX / scrollSpeed, true)
        .SetEase(Ease.Linear).OnComplete(() => RestartScrollingText(scrollText));
    }
}
