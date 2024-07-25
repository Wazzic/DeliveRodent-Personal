using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class DigitalClock : MonoBehaviour
{
    //Text to display timer
    [SerializeField]
    private TMP_Text textTimer;

    //Initial Start time for timer
    [SerializeField]
    private float startTime;

    //Timer
    private float timer = 0.0f;
    //Checks if timer is enabled
    private bool isTimer = false;

    void Awake()
    {
        timer = startTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimer();
    }
    //Gets the current time
    public float GetTimer()
    {
        return timer;
    }

    //Set the timer to a time
    public void SetTime(float t)
    {
        timer = t;
        startTime = t;
        DisplayTime();
    }
    //Set the timer to a time
    public void SetTime(int minute, int seconds)
    {
        startTime = minute * 60 * seconds;
        timer = startTime;
        DisplayTime();
    }

    //Starts the timer
    public void StartTimer()
    {
        isTimer = true;
    }
    //Stops the timer
    public void StopTimer()
    {
        isTimer = false;
    }
    //Resets the timer to the start time
    public void ResetTimer()
    {
        timer = startTime;
    }

    //Updates the display for the timer
    private void DisplayTime()
    {
        int minutes = Mathf.FloorToInt(timer / 60f);
        int seconds = Mathf.FloorToInt(timer - minutes * 60f);

        //textTimer.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
    //Updates the timer
    private void UpdateTimer()
    {
        if (isTimer)
        {
            timer += Time.deltaTime;
            DisplayTime();
        }
    }
}
