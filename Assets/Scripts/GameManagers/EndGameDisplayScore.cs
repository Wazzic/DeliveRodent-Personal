using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class EndGameDisplayScore : MonoBehaviour
{
    //Location to spawn the text in
    [SerializeField]
    GameObject ScoreBoard;

    //Text prefab use for the score
    [SerializeField]
    private GameObject TextUIPrefab;

    //To store the list of player scores
    private List<float> PlayerScores;

    //Currently Does the basics of displaying the players scores: NEEDS UPDATED TO SHOW PLAYERS IN SCORE ORDER
    void Start()
    {
        if (PlayerStats.Scores.Count>0)
        {
            PlayerScores = PlayerStats.Scores;

            for (int i = 0; i < PlayerScores.Count; i++)
            {
                TextMeshProUGUI text = Instantiate(TextUIPrefab, ScoreBoard.transform).GetComponent<TextMeshProUGUI>();
                text.text = ((i + 1) + ". Player " + i + " Score: " + PlayerScores[i]);
            }
        }     
    }

}
