using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System.Linq;

public class ShieldEffect : MonoBehaviour
{
    //Tween rotation;
    List<MeshRenderer> renderers;

    private void Awake()
    {
        renderers = GetComponentsInChildren<MeshRenderer>().ToList();

        //rotation = transform.DOLocalRotate(transform.localEulerAngles + new Vector3(0f, 135f, 0f), 2, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo);
        HideShields();
    }
    public void PlayShield()
    {
        transform.DOLocalRotate(transform.localEulerAngles + new Vector3(0f, 235f, 0f), 2f, RotateMode.FastBeyond360).onComplete = HideShields;
        foreach(MeshRenderer renderer in renderers)
        {
            //renderer.enabled = true;
            renderer.gameObject.SetActive(true);
        }
        //rotation.Play().onComplete = ResetTween;        
    }
    private void HideShields()
    {
        foreach(MeshRenderer renderer in renderers)
        {
            renderer.gameObject.SetActive(false);
        }
        //rotation.Rewind();
    }
}
