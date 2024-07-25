using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Spring;
using Spring.Runtime;
using System.Linq;

//SINGLETON
public class ScoreManager : MonoBehaviour
{  
    public List<SpringToScale> scaleSprings;

    //PRIVATE
    //Text for the player score;
    [SerializeField]
    private GameObject ScorePanel;
    //Text prefab used for score
    [SerializeField]
    private GameObject TextUIPrefab;
    //
    [SerializeField]
    private List<PlayerScore> playersScores;
    public List<PlayerItemsHandler> playerItemsHandlers;
    //Singleton Variable
    public static ScoreManager instance;
    //sets up the the prefab
    public void SetTextPrefab(GameObject prefab)
    {
        TextUIPrefab = prefab;
    }
    //Sets up singleton
    private void Awake()
    {
        if (!instance)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
        
        ScorePanel = GameObject.Find("ScorePanel");
        playersScores = new List<PlayerScore>();    
        playerItemsHandlers = new List<PlayerItemsHandler>();
    }

    //Returns list of players scores
    public List<float> PlayersScore()
    {
        List<float> scores = new();

        for(int i = 0; i < playersScores.Count; i++)
        {
            scores.Add(playersScores[i].GetScore());
        }

        return scores;
    }

    //Gets the players score script
    public void AddPlayerScoreScript(PlayerScore player)
    {   
        playersScores.Add(player);
    }
    public bool IsPlayerLast(float score)
    {
        foreach (PlayerScore player in playersScores)
        {
            if (score > player.GetScore())
            {
                return false;
            }
        }
        return true;
    }
    public void CheckAllPlayerRanks()
    {
        foreach (PlayerItemsHandler handler in playerItemsHandlers)
        {
            handler.PlayerRank();
        }
    }
}
