using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SetActiveZones : MonoBehaviour
{
    [SerializeField] private PlayerConfigs playerConfigs;
    [SerializeField] private UIButtonScaleSpring textSpring;

    private List<Button> buttons;
    [SerializeField]
    int activeZones;
    TextMeshProUGUI activeZonesText;

    private void Start()
    {
        buttons = new List<Button>();
        buttons = this.GetComponentsInChildren<Button>().ToList();

        GameObject temp = GameObject.Find("ActiveZonesPanel");
        activeZonesText = temp.GetComponentInChildren<TextMeshProUGUI>();

        activeZones = 1;
        UpdateActiveZones();
    }
    public void IncreaseActiveZones()
    {
        activeZones++;
        UpdateActiveZones();
    }
    public void DecreaseActiveZones()
    {
        activeZones--;
        UpdateActiveZones();
    }
    private void UpdateActiveZones()
    {
        activeZones = Mathf.Clamp(activeZones, 1, 6);
        activeZonesText.text = activeZones.ToString();
        playerConfigs.activeZones = activeZones;
        textSpring.OnButtonPressNudge();
    }
}
