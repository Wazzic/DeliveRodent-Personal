using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spring;
using Spring.Runtime;

public class UIButtonScaleSpring : MonoBehaviour
{
    [SerializeField] private SpringToScale scaleSpring;

    Vector3 initScale;

    private void Awake()
    {
        initScale = transform.localScale;
    }

    public void OnButtonPressNudge()
    {
        scaleSpring.Nudge(new Vector3(2, 2, 2));
    }
    public void OnButtonHoverEnterScale()
    {
        scaleSpring.SpringTo(new Vector3(1.2f * initScale.x, 1.1f * initScale.y, 1.1f * initScale.z));
    }
    public void OnButtonHoverExitScale()
    {
        scaleSpring.SpringTo(initScale);
    }
}
