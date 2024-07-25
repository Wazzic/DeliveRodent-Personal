using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DisplayPlayerScore : MonoBehaviour
{
    private PlayerScore playerScore;
    [SerializeField]
    private TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
       playerScore = transform.root.GetComponent<PlayerScore>();
       text =  GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.SetText(playerScore.GetScore().ToString());
    }
}
