using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Blackout : MonoBehaviour
{
    RectTransform panel;
    CanvasGroup group;
    [SerializeField] PlayerConfigs configs;

    private void Start()
    {
        //panel = GetComponent<RectTransform>();
        group = GetComponent<CanvasGroup>();
        //Vector2 screenSize = new Vector2(Screen.width, Screen.height);
        if (configs.numberOfPlayers == 3)
        {
            //panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.width / 2);
            //panel.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height / 2);
            group.alpha = 1.0f;
        }
        else
        {
            group.alpha = 0.0f;
        }
    }
}
