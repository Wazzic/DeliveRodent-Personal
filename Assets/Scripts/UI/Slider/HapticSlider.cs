using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HapticSlider : MonoBehaviour
{
    Slider slider;
    private void Awake()
    {
        slider = GetComponent<Slider>();
        slider.value = GameManager.instance.playerConfigs.hapticLevel;        
    }
    public void OnChangeSlider(float value)
    {
        GameManager.instance.playerConfigs.hapticLevel = value;
    }
}
