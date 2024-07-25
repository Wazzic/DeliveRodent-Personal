using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMapVisibilty : MonoBehaviour
{
    CanvasGroup minimapGroup;
    void Start()
    {
        minimapGroup = GetComponent<CanvasGroup>();
        if (TestScene01Manager.instance.PlayerAmount != 1)
        {
            minimapGroup.alpha = 0.9f;
        }
        else
        {
            minimapGroup.alpha = 0.0f;
        }
    }
}
