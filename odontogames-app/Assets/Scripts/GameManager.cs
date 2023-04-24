using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public struct Minigame_Score
    {
        public int correctAnswers;
        public int wrongAnswers;
        public int bonusPoints;
    }

    public static GameManager instance;

    private Dictionary<int, Minigame_Score> gamePoints;

    private int currentMinigame = 1;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if (instance != this)
            {
                Destroy(gameObject);
            }
        }

        gamePoints = new Dictionary<int, Minigame_Score>();
    }

    public void CorrectAnswer()
    {
        if (!gamePoints.ContainsKey(currentMinigame))
        {
            gamePoints[currentMinigame] = new Minigame_Score();
        }
        Minigame_Score currentScore = gamePoints[currentMinigame];
        currentScore.correctAnswers++;

        gamePoints[currentMinigame] = currentScore;
    }

    public void WrongAnswer()
    {
        if (!gamePoints.ContainsKey(currentMinigame))
        {
            gamePoints[currentMinigame] = new Minigame_Score();
        }
        Minigame_Score currentScore = gamePoints[currentMinigame];
        currentScore.wrongAnswers++;

        gamePoints[currentMinigame] = currentScore;
    }

    public void ReceiveBonusPoints(int points)
    {
        if (!gamePoints.ContainsKey(currentMinigame))
        {
            gamePoints[currentMinigame] = new Minigame_Score();
        }
        Minigame_Score currentScore = gamePoints[currentMinigame];
        currentScore.bonusPoints = points;

        gamePoints[currentMinigame] = currentScore;
    }

    public void UpdatePlayerScore()
    {
        string currentMinigameName = "firstgamescore";
        switch (currentMinigame)
        {
            case 1:
                currentMinigameName = "firstgamescore";
                break;
            case 2:
                currentMinigameName = "secondgamescore";
                break;
            case 3:
                currentMinigameName = "thirdgamescore";
                break;
            case 4:
                currentMinigameName = "fourthgamescore";
                break;
            case 5:
                currentMinigameName = "fifthgamescore";
                break;
            case 6:
                currentMinigameName = "sixthgamescore";
                break;
            case 7:
                currentMinigameName = "seventhgamescore";
                break;
        }

        if (StrapiComponent._instance != null) 
            StrapiComponent._instance.UpdatePlayerScore(gamePoints[currentMinigame], currentMinigameName);
    }

    public void NextMinigame()
    {
        if (currentMinigame == 7)
        {
            EndGame();
        }
        else
        {
            currentMinigame++;
            MySceneManager.instance.LoadScene("escaperoom1_" + currentMinigame);
        }
        
    }

    private void EndGame()
    {
        UpdatePlayerScore();
        MySceneManager.instance.LoadScene("MainMenu");
    }

    public void SetCurrentMinigame(int current)
    {
        currentMinigame = current;
    }

    public int GetPoints()
    {
        return gamePoints[currentMinigame].correctAnswers;
    }

    public Minigame_Score GetMinigameScore()
    {
        return gamePoints[currentMinigame];
    }
}
