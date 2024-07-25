using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinimapButton : MonoBehaviour
{
    [SerializeField] CanvasGroup minimap;
    public bool showMinimap;
    public void HideShowMap()
    {
        showMinimap = !showMinimap;
        if (showMinimap)
        {
            minimap.alpha = 1.0f;
        }
        else
        {
            minimap.alpha = 0.0f;
        }
    }
}
