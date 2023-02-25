using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_5_player : MonoBehaviour
{
    public string[] answers;
    int currentQuestion = 0;

    public void ChooseAnswer(string answer)
    {
        answers[currentQuestion] = answer;
        currentQuestion++;
    }
}
