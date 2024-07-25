using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;
using System.Linq;

public class LowTimeUIManager : MonoBehaviour
{
    [SerializeField] List<SpringToRotation> rotationSprings;
    //SpringToScale scaleSpring;
    //CanvasGroup lowTimeGroup;

    [SerializeField] List<NewsTicker> overTimeTickers;
    int numberOfFrames;
    [SerializeField] int delay;

    public bool lowTime;
    public bool oneFrame;

    private void Awake()
    {
        //scaleSpring = GetComponent<SpringToScale>();
        //lowTimeGroup = GetComponent<CanvasGroup>();
        //SetGroupAlpha(0.0f);
        numberOfFrames = delay - 1;
        //rotationSprings = GetComponentsInChildren<SpringToRotation>().ToList();

        oneFrame = false;
    }
    private void Update()
    {
        if (lowTime)
        {
            if (numberOfFrames > delay)
            {
                //ScaleSpringNudge();
                RotationSpringNudge();
                numberOfFrames = 0;
            }
            else
            {
                numberOfFrames++;
            }
            if (oneFrame)
            {
                oneFrame = false;
                foreach (NewsTicker ticker in overTimeTickers)
                {
                    ticker.StartOvertime();
                    //Debug.Log("Overtime Started");
                }
            }
        }
    }
    public void RotationSpringNudge()
    {
        foreach (SpringToRotation spring in rotationSprings)
        {
            spring.Nudge(Random.Range(1, 4) * 175 * Vector3.one);
        }
    }
}

