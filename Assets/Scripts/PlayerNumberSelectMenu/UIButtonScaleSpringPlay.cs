using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;

public class UIButtonScaleSpringPlay : MonoBehaviour
{
    [SerializeField] private SpringToScale scaleSpring;

    public void OnButtonPressNudge()
    {
        scaleSpring.Nudge(new Vector3(2, 2, 2));
    }
    public void OnButtonHoverEnterScale()
    {
        scaleSpring.SpringTo(new Vector3(1.6f, 1.4f, 1.2f));
    }
    public void OnButtonHoverExitScale()
    {
        scaleSpring.SpringTo(new Vector3(1.2f, 1f, 1f));
    }
}
