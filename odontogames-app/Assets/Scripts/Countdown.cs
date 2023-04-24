using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    private Text countdownText;
    public float timeLeft = 60.0f;

    private void Awake()
    {
        countdownText = GetComponent<Text>();
    }

    private void Update()
    {
        timeLeft -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        if(timeLeft <= 0)
        {
            countdownText.text = "0:00";
        }
        else
        {
            countdownText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    public float GetTimeLeft()
    {
        return timeLeft;
    }

    public int GetBonusPoints()
    {
        int extra = 0;
        if (timeLeft >= 240) extra = 3;
        else if (timeLeft >= 120) extra = 2;
        else if (timeLeft >= 60) extra = 1;

        return extra;
    }
}
