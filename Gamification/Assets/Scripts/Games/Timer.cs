using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    private bool isTimerOn;
    private float seconds;
    private int minutes;
    private int hours;
    private TMP_Text timerText;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        StartTimer();
    }

    void Update()
    {
		if (isTimerOn)
		{
            seconds += Time.deltaTime;
            if(seconds >= 60)
			{
                minutes++;
                seconds = 0;
                if(minutes >= 60)
				{
                    minutes = 0;
                    hours++;
				}
			}
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
        seconds = 0;
	}

    public void RestartTimer()
	{
        ResetTimer();
        StartTimer();
	}

    private void DisplayTime()
	{
        if(hours > 0)
		{
            timerText.text = hours.ToString() + "h " + minutes.ToString() + "m " + seconds.ToString("F0") + "s";
        }
        else if(minutes > 0)
		{
            timerText.text = minutes.ToString() + "m " + seconds.ToString("F0") + "s";
        }
		else
		{
            timerText.text = seconds.ToString("F1") + "s";
        }
        
	}
}
