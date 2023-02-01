using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_4_answer : MonoBehaviour
{
    public string answer;
    escaperoom1_4_questions questions;

    void Awake()
    {
        questions = GameObject.FindGameObjectWithTag("questions").GetComponent<escaperoom1_4_questions>();
    }

    void OnMouseDown()
    {
        questions.ChooseAnswer(answer);
    }
}
