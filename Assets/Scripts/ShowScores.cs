using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class ShowScores : MonoBehaviour
{
    [SerializeField]TextMeshPro[] scoreUI;
    [SerializeField] Transform[] scorePos;
    [SerializeField] PlayerConfigs playerConfigsSO;

    [SerializeField] List<Transform> spawnPoints;

    List<int> positions;

    private ParticleEffectGroup fireworks;

    public bool scoresComplete = true;
    public struct scores
    {
        public int index;
        public float score;
    }

    public List<scores> finalScores;

    List<scores> bubbleSort(List<scores> scoresToSort)
    {
        int n = scoresToSort.Count;
        for (int i = 0; i < n - 1; i++)
        {
            for (int j = 0; j < n - i - 1; j++)
            {
                if (scoresToSort[j].score > scoresToSort[j + 1].score)
                {
                    scores temp = scoresToSort[j];
                    scoresToSort[j] = scoresToSort[j + 1];
                    scoresToSort[j + 1] = temp;
                }
            }
        }        
        return scoresToSort;
    }
    [SerializeField] AudioClip yayClip;

    IEnumerator scoreDelay()
    {
        for (int i = 0; i < PlayerStats.Scores.Count; i++)
        {
            yield return new WaitForSeconds(1.5f);
            if (i == playerConfigsSO.numberOfPlayers - 1)
            {
              
                fireworks.transform.position = scorePos[finalScores[i].index].position;
                yield return new WaitForSeconds(1f);
                GetComponent<AudioSource>().Stop();
                GetComponent<AudioSource>().PlayOneShot(yayClip, 1);
                yield return new WaitForSeconds(1.0f);
                scoresComplete = true;
            }
            else if (i == 0)
            {
                yield return new WaitForSeconds(3.5f);
                GetComponent<AudioSource>().Play();
            }
            else
            {
                yield return new WaitForSeconds(1);
            }
            int players2 = 0;
            if(playerConfigsSO.numberOfPlayers == 2)
            {
                players2 = 1;
            }
            scoreUI[finalScores[i].index + players2].SetText("£{0:2}", finalScores[i].score);
            scoreUI[finalScores[i].index + players2].color = playerConfigsSO.carMaterials[finalScores[i].index].GetColor("_ColorDim");
            scoreUI[finalScores[i].index + players2].GetComponent<UIButtonScaleSpring>().OnButtonPressNudge();
            //scoreUI[finalScores[i].index + players2].transform.parent = spawnPoints[finalScores[i].index + players2];
            PodiumPosition(finalScores[i].index, i);
        }

        if (PlayerStats.Scores.Count == 0)
        {
            scoresComplete = true;
        }
    }
    private void PodiumPosition(int playerNumber, int playerPosition)
    {
        int player2 = 0;
        if (playerConfigsSO.numberOfPlayers == 2)
        {
            player2 = 1;
        }
        spawnPoints[playerNumber + player2].DOLocalMoveY(Mathf.Pow(((float)playerPosition + player2), 2) + spawnPoints[playerNumber + player2].localPosition.y , 1.05f).SetEase(Ease.InOutQuart);
            //+= Mathf.Pow((float)playerScorePosition, 2) * Vector3.up;
        
    }

    // Start is called before the first frame update
    void Start()
    {
        scoresComplete = false;
        fireworks = GetComponentInChildren<ParticleEffectGroup>();
        finalScores = new List<scores>();
        scores tempScore = new scores();

        for (int i = 0; i < PlayerStats.Scores.Count; i++)
        {
            
            tempScore.score = PlayerStats.Scores[i];
            tempScore.index = i;

            finalScores.Add(new scores());

            finalScores[i] = tempScore;
        }

        finalScores = bubbleSort(finalScores);

        StartCoroutine(scoreDelay());
    }
}
