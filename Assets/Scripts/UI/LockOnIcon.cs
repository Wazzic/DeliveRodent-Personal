using DG.Tweening;
using Spring.Runtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockOnIcon : MonoBehaviour
{
    public bool lockedOn;

    SpriteRenderer spriteRenderer;

    public Camera cam;
    /*
    Tween posTween;
    Tween rotTween;
    Tween scaleTween;
    Tween fadeTween;
    */

    public float missileDistance;
    float farDistance = 250;
    float nearDistance = 20;

    [SerializeField] float startScale;
    [SerializeField] float endScale = 0.15f;
    [SerializeField] Vector3 startPos = new Vector3(0, 5f, -10f);
    [SerializeField] Vector3 endPos = new Vector3(0, 2.95f, -4.55f);
    [SerializeField] float endAlpha = 180f;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //spriteRenderer.color = Color.clear;
        cam = transform.root.GetComponentInChildren<Camera>();

        Reset();

        lockedOn = false;
    }
    private void Update()
    {
        if (lockedOn)
        {
            float missileDistanceLerp = Mathf.InverseLerp(nearDistance, farDistance, missileDistance);
            LerpScale(missileDistanceLerp);
            LerpPosition(missileDistanceLerp);
            LerpSpriteAlpha(missileDistanceLerp);
            this.transform.localRotation = Quaternion.Euler(0, 0, 0.1f * missileDistance);
        }
        else
        {
            spriteRenderer.color = Color.clear;
        }
    }
    private void InitTweens()
    {
        /*
        posTween = rectTransform.DOAnchorPos(-120f * Vector2.up, 3).SetEase(Ease.InQuart);
        rotTween = rectTransform.DOLocalRotate(60f * Vector3.forward, 3f).SetEase(Ease.InQuart);
        scaleTween = transform.DOScale(1.5f * Vector3.one, 3f).SetEase(Ease.InQuart);
        fadeTween = canvasGroup.DOFade(0.6f, 2f).SetEase(Ease.InQuart);
        */
    }
    public void Reset()
    {
        /*
        posTween.Rewind();
        rotTween.Rewind();
        scaleTween.Rewind();
        fadeTween.Rewind();
        */
        missileDistance = 0;
    }   
    public void FadeOut()
    {
        //canvasGroup.DOFade(0f, 0.2f).onComplete = Reset;
    }
    private void LerpScale(float value)
    {
        this.transform.localScale = Vector3.Slerp(endScale * Vector3.one, startScale * Vector3.one, value);
    }
    private void LerpPosition(float value)
    {
        this.transform.localPosition = Vector3.Slerp(startPos, endPos, value);
    }
    private void LerpSpriteAlpha(float value)
    {
        Color color = new Color(1, 1, 1, 1 - value);
        spriteRenderer.color = color;
    }
}
