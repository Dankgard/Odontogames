using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int[] gamePoints;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            if(instance != this)
            {
                Destroy(gameObject);
            }
        }

        gamePoints = new int[7];
    }

    public void ReceiveGamePoints(int minigame, int points)
    {
        gamePoints[minigame] = points;
    }
}
