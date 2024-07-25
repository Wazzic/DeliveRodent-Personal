using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetRoundTime : MonoBehaviour
{
    [SerializeField] private PlayerConfigs playerConfigs;
    [SerializeField] private UIButtonScaleSpring textSpring;

    private List<Button> buttons;
    [SerializeField]
    int roundTime;
    TextMeshProUGUI roundTimeText;

    private void Start()
    {
        buttons = new List<Button>();
        buttons = this.GetComponentsInChildren<Button>().ToList();

        GameObject temp = GameObject.Find("RoundTimePanel");
        roundTimeText = temp.GetComponentInChildren<TextMeshProUGUI>();

        roundTime = 5;
        UpdateRoundTime();
    }
    public void IncreaseRoundTime()
    {
        roundTime++;
        UpdateRoundTime();
    }
    public void DecreaseRoundTime()
    {
        roundTime--;
        UpdateRoundTime();
    }
    private void UpdateRoundTime()
    {
        roundTime = Mathf.Clamp(roundTime, 3, 10);
        roundTimeText.text = roundTime.ToString();
        playerConfigs.roundTime = roundTime;
        textSpring.OnButtonPressNudge();
    }
}
