using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class escaperoom1_3_questions_logic : MonoBehaviour
{
    public string[] questions;
    public string[] answers;

    public InputField answerInputField;
    public Text feedbackText;
    public Text questionText;
    public Text answerText;

    private int index;
    private string wrongAnswerMessage;
    private string correctAnswerMessage;

    void Start()
    {
        if (questions.Length == 0 || answers.Length == 0 || (questions.Length != answers.Length))
        {
            Debug.LogError("Error en el campo de preguntas y respuestas");
            return;
        }

        wrongAnswerMessage = "¡Intenta otra vez!";
        correctAnswerMessage = "¡Excelente!";

        index = 0;

        questionText.text = questions[index];
    }

    void Update()
    {
        
    }

    public void OnSubmit()
    {
        string answer = answerInputField.text;
        if (checkAnswer(answerInputField.text, index))
        {
            feedbackText.text = correctAnswerMessage;
            answerText.text += answers[index][0];
            index++;
            questionText.text = questions[index];
        }
        else
        {
            feedbackText.text = wrongAnswerMessage;
        }
        answerInputField.text = "";

        if (index == questions.Length)
        {
            SoundManager.instance.PlaySound(2);
            MySceneManager.instance.LoadScene("MinigameEnd");
        }
    }

    public bool checkAnswer(string answer, int index)
    {
        return answer == answers[index];
    }
}
