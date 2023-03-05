using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class escaperoom1_7_questions : MonoBehaviour
{
    public string filename = "assets/JSON/espaceroom1_7.json";

    private string[] letters;
    private string[] hints;
    private string[] questions;
    private string[] answers;

    void Awake()
    {
        int size = File.ReadAllLines(filename).Length - 2;
        letters = new string[size / 4];
        hints = new string[size / 4];
        questions = new string[size / 4];
        answers = new string[size / 4];

        int i = 0;
        int order = 0;
        foreach (string line in File.ReadAllLines(filename))
        {
            if (line != "{" && line != "}")
            {
                switch(order)
                {
                    case 0:
                        letters[i] = line;
                        break;
                    case 1:
                        hints[i] = line;
                        break;
                    case 2:
                        questions[i] = line;
                        break;
                    case 3:
                        answers[i] = line;
                        i++;
                        order = -1;
                        break;
                }
                order++;
            }
        }
    }

    public string[] GetLetters()
    {
        return letters;
    }

    public string[] GetHints()
    {
        return hints;
    }

    public string[] GetQuestions()
    {
        return questions;
    }

    public string[] GetAnswers()
    {
        return answers;
    }
}
