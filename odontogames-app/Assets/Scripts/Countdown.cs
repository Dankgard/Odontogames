using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Countdown : MonoBehaviour
{
    Text countdownText;
    public float timeLeft = 60.0f;
    bool end = false;

    void Awake()
    {
        countdownText = GetComponent<Text>();
    }

    void Update()
    {
        timeLeft -= Time.deltaTime;

        int minutes = Mathf.FloorToInt(timeLeft / 60);
        int seconds = Mathf.FloorToInt(timeLeft % 60);

        if(timeLeft <= 0)
        {
            countdownText.text = "0:00";
            OutOfTime();
        }
        else
        {
            countdownText.text = minutes.ToString("00") + ":" + seconds.ToString("00");
        }
    }

    void OutOfTime()
    {
        if(!end)
        {
            end = true;
            Debug.Log("You are out of time.");
            MySceneManager.instance.LoadScene("MinigameEnd");
        }
    }

}
