using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_4_questions : MonoBehaviour
{
    public string[] question;
    public string[] answer1;
    public string[] answer2;
    public string[] answer3;
    public string[] answer4;

    TextMesh line1;
    TextMesh line2;
    TextMesh line3;
    TextMesh line4;
    TextMesh line5;

    int currentQuestion = -1;

    void Awake()
    {
        line1 = gameObject.transform.GetChild(0).GetComponent<TextMesh>();
        line2 = gameObject.transform.GetChild(1).GetComponent<TextMesh>();
        line3 = gameObject.transform.GetChild(2).GetComponent<TextMesh>();
        line4 = gameObject.transform.GetChild(3).GetComponent<TextMesh>();
        line5 = gameObject.transform.GetChild(4).GetComponent<TextMesh>();

        NextQuestion();
    }

    void NextQuestion()
    {
        currentQuestion++;
        if(currentQuestion < 5)
        {
            line1.text = question[currentQuestion];
            line2.text = answer1[currentQuestion];
            line3.text = answer2[currentQuestion];
            line4.text = answer3[currentQuestion];
            line5.text = answer4[currentQuestion];
        }
    }
}
