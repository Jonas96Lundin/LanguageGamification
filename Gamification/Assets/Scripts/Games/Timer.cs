using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class Timer : MonoBehaviour
{
    private bool isTimerOn;
    private float totalTime;
    private int hours;
    private int minutes;
    private int seconds;
    private float milliSeconds;
    private TMP_Text timerText;

    public float TotalTime { get { return totalTime; } }


    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        //StartTimer();
    }

    void Update()
    {
		if (isTimerOn)
		{
            totalTime += Time.deltaTime;
            DisplayTime();
        }
    }

    public void StartTimer()
	{
        isTimerOn = true;
	}

    public void StopTimer()
	{
        isTimerOn = false;
    }

    public void ResetTimer()
	{
        StopTimer();
        totalTime = 0;
	}

    public void RestartTimer()
	{
        ResetTimer();
        StartTimer();
	}

    private void CalculateTime()
	{
        hours = TimeSpan.FromSeconds(totalTime).Hours;
        minutes = TimeSpan.FromSeconds(totalTime).Minutes;
        seconds = TimeSpan.FromSeconds(totalTime).Seconds;
        milliSeconds = (totalTime - seconds) * 10;
        if (milliSeconds > 9)
            milliSeconds = 0;
    }

    private void DisplayTime()
	{
        CalculateTime();

        if(hours > 0)
		{
            timerText.text = hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString() + "s";
        }
        else if(minutes > 0)
		{
            timerText.text = minutes.ToString() + "m " + seconds.ToString() + "s";
        }
		else
		{
            timerText.text = seconds.ToString() + /*"." + milliSeconds.ToString("F0") + */"s";
        }
	}

    public string GetVictoryTime()
    {
        CalculateTime();

        if (hours > 0)
        {
            return "Heure de la victoire: " + hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString() + "s";
        }
        else if (minutes > 0)
        {
            return "Heure de la victoire: " + minutes.ToString() + "m " + seconds.ToString() + "s";
        }
        else
        {
            return "Heure de la victoire: " + seconds.ToString() + "." + milliSeconds.ToString("F0") + "s";
        }
    }
}
