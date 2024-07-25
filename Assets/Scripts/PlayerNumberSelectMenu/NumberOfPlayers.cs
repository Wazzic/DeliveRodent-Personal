using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class NumberOfPlayers : MonoBehaviour
{
    [SerializeField] private PlayerConfigs playerConfigs;
    [SerializeField] private UIButtonScaleSpring textSpring;

    private List<Button> buttons;
    
    int playerNumber;
    TextMeshProUGUI playerNumberText;

    private void Awake()
    {
        buttons = new List<Button>();
        buttons = this.GetComponentsInChildren<Button>().ToList();

        GameObject temp = GameObject.Find("PlayerNumberPanel");
        playerNumberText = temp.GetComponentInChildren<TextMeshProUGUI>();

        playerNumber = 2;
        UpdatePlayerNumber();
    }
    public void IncreasePlayerNumber()
    {
        playerNumber++;
        UpdatePlayerNumber();        
    }
    public void DecreasePlayerNumber()
    {
        playerNumber--;
        UpdatePlayerNumber();        
    }
    private void UpdatePlayerNumber()
    {
        playerNumber = Mathf.Clamp(playerNumber, 1, 4);
        playerNumberText.text = playerNumber.ToString();
        playerConfigs.numberOfPlayers = playerNumber;
        textSpring.OnButtonPressNudge();
    }     
}
