using Spring.Runtime;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class StunEffect : MonoBehaviour
{
    SpringToScale effectSpring;
    List<Transform> stars = new List<Transform>();
    List<TrailRenderer> trails = new List<TrailRenderer>();
    List<MeshRenderer> renderers = new List<MeshRenderer>();
    
    public float rotationSpeed = 10;
    
    private void Awake()
    {
        stars = GetComponentsInChildren<Transform>().ToList();
        stars.RemoveAt(0);

        foreach (Transform t in stars)
        {
            trails.Add(t.GetComponent<TrailRenderer>());
            renderers.Add(t.GetComponent<MeshRenderer>());
        }
        /*
        particles = GetComponentsInChildren<ParticleEffectGroup>().ToList();
        //stars.RemoveAt(0);
        rot = 0.0f;
        */
        effectSpring = GetComponent<SpringToScale>();
        DeactiveTrails();
        DeactivateRenderers();
    }
    public void PlayStunEffect()
    {
        ActivateRenderers();
        Grow();
        RotateStars();
        transform.DOLocalRotate(transform.localEulerAngles + new Vector3(0, 235, 0), 1.8f, RotateMode.FastBeyond360).SetEase(Ease.InOutExpo).onComplete = Shrink;
    }
    private void Grow()
        //change this to a tween with callback on finish to set trail emitting to true!!
            //Can then remove the spring comp
    {
        this.transform.localScale = 0.01f * Vector3.one;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBounce); //.onComplete = ActivateTrails;
        effectSpring.SpringTo(Vector3.one);
        
    }
    private void Shrink()
    {
        //effectSpring.SpringTo(0.01f * Vector3.one);
        transform.DOScale(Vector3.one * 0.01f, 0.4f).SetEase(Ease.InQuart).onComplete = DeactivateRenderers;
        /*
        foreach (TrailRenderer t in trails)
        {
            t.emitting = false;
        }
        */
    }
    private void RotateStars()
    {
        foreach (Transform star in stars)
        {
            star.transform.DOLocalRotate(new Vector3(0, -135, 0), 2, RotateMode.FastBeyond360);
        }     
    }
    private void ActivateTrails()
    {
        //effectSpring.Nudge(100 * Vector3.one);
        foreach (TrailRenderer t in trails)
        {
            t.emitting = true;
        }
    }
    private void DeactiveTrails()
    {
        foreach (TrailRenderer t in trails)
        {
            t.emitting = false;
        }
    }
    private void ActivateRenderers()
    {
        foreach (MeshRenderer r in renderers)
        {
            r.enabled = true;
        }
    }
    private void DeactivateRenderers()
    {
        foreach (MeshRenderer r in renderers)
        {
            r.enabled = false;
        }
    }
    
}
