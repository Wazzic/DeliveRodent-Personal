using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;
using DG.Tweening;

public class DeliverySpring : MonoBehaviour
{
    [SerializeField] private Vector3 nudgeAmount;

    SpringToScale scaleSpring;

    ParticleEffectGroup particleEffectGroup;
    void Awake()
    {
        scaleSpring = GetComponent<SpringToScale>();

        particleEffectGroup = GetComponent<ParticleEffectGroup>();
    }
    //void OnEnable()
    //{
    //    //StartCoroutine(WaitFrames());
    //    //Debug.Log("Delivery Enabled");
    //}
    private IEnumerator WaitFrames()
    {
        yield return new WaitForEndOfFrame();
        scaleSpring.Nudge(nudgeAmount);
    }
    public void NudgeDeliveryScale()
    {
        scaleSpring.Nudge(nudgeAmount);
    }
    public void DeliveryRanOut()
    {
        particleEffectGroup.Play();
        transform.DOScale(Vector3.zero, 1.5f).SetEase(Ease.InOutQuart).onComplete = SetInActive;
    }
    private void SetInActive()
    {
        this.gameObject.SetActive(false);
        transform.localScale = Vector3.one * 2.5f;
    }
}
