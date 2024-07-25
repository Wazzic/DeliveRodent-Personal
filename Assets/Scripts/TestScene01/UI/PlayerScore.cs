using UnityEngine;
using System.Collections;
using TMPro;
using Spring.Runtime;
public class PlayerScore : MonoBehaviour
{
    //PRIVATE
    [SerializeField]
    public float score { get;  private set; }
    
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI plus3Text;    
    [SerializeField] Color blueColour;
    [SerializeField] Color greenColour;
    SpringToScale scoreTextSpringToScale;
    Animator plusScoreAnimator;
    [SerializeField] AnimationClip plusScoreAnimClip;

    private void Start()
    {
        plusScoreAnimator = plus3Text.GetComponent<Animator>();
        scoreTextSpringToScale = scoreText.GetComponent<SpringToScale>();        
        plus3Text.gameObject.SetActive(false);
        // plus3initialPosition = plus3Text.transform.localPosition;
        UpdateScoreText();
    }
    //Return the player score
    public float GetScore()
    {
        return score;
    }
    
    public void AddScore(float value)
    {
        //plus3Text.gameObject.SetActive(true);
        plus3Text.gameObject.SetActive(true);
        if (value == 1)
        {
            plus3Text.color = blueColour;
        }
        else if (value == 3)
        {
            plus3Text.color = greenColour;
        }
        else
        {
            Debug.LogWarning("a score value that wasnt 1 or 3 was passed through");
        }
        plus3Text.text = "£+" + value;
        plusScoreAnimator.Play("PlusScore");
        Invoke("UpdateScoreText", plusScoreAnimClip.length - 0.2f);
        //Score gets added instanly
        score += value;
    }

    public void SetScore(float value)
    {
        score = value;
    }

    private void UpdateScoreText()
    {
        //scoreText.SetText("£{0:2}",score);
        scoreText.SetText("£"+score);
        plus3Text.gameObject.SetActive(false);
        scoreTextSpringToScale.Nudge(new Vector3(20, 20, 20));
    }
}