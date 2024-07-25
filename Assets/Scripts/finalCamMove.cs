using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class finalCamMove : MonoBehaviour
{ 
    [SerializeField] CanvasGroup buttonGroup;
    Vector3 targetVector = new Vector3(-1291f, -332f, 357.1f);
    Quaternion targetRot = Quaternion.Euler(0f, 86.735f, 0f);
    private float startTime;
    public float journeyTime = 4.0f;

    [SerializeField] ShowScores checkCompletion;
    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        buttonGroup.alpha = 0.0f;
    }

    // Update is called once per frame
    void Update()
    {
        float fracComplete = (Time.time - startTime) / journeyTime;
        
        transform.position = Vector3.Slerp(transform.position, targetVector, fracComplete * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, fracComplete * Time.deltaTime);

        if(checkCompletion.scoresComplete == true)
        {
            if(buttonGroup.alpha < 1.0f)
            {
                buttonGroup.alpha += 0.01f;
            }
            else
            {
                return;
            }
        }
    }
}
