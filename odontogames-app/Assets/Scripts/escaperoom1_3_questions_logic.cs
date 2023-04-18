using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;
using System.Text;
using System;

public class escaperoom1_3_questions_logic : MonoBehaviour
{
    public string[] questions;
    public string[] answers;

    public GameObject canvas;

    public InputField answerInputField;
    public Text feedbackText;
    public Text questionText;
    public TextMesh answerText;

    private int index;
    private string wrongAnswerMessage;
    private string correctAnswerMessage;

    private int score = 0;

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

    public void OnSubmit()
    {
        if (checkAnswer(answerInputField.text, index))
        {
            CorrectAnswer();
            Debug.Log("Indice " + index);
        }
        else
        {
            feedbackText.GetComponent<Text>().color = Color.red;
            feedbackText.text = wrongAnswerMessage;
            score--;
        }
        answerInputField.text = "";

        if (index == questions.Length)
        {
            StartCoroutine(EndGame());
        }
    }

    public void CorrectAnswer()
    {
        StartCoroutine(CorrectAnswerCoroutine());
    }

    private IEnumerator CorrectAnswerCoroutine()
    {
        canvas.SetActive(false);
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        yield return new WaitForSeconds(1.5f);
        feedbackText.GetComponent<Text>().color = Color.green;
        feedbackText.text = correctAnswerMessage;
        if (index < questions.Length)
        {
            index++;
            questionText.text = questions[index];
            answerText.text += answers[index - 1][0];
        }
        score++;
        yield return new WaitForSeconds(2.5f);
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        canvas.SetActive(true);
    }

    private IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3.5f);
        SoundManager.instance.PlaySound(2);
        MySceneManager.instance.LoadScene("MinigameEnd");
        yield return new WaitForSeconds(1.5f);
    }

    public bool checkAnswer(string answer, int index)
    {
        answer = NormalizeText(answer);
        string[] correctAnswers = answers[index].Split('|');

        foreach (string correctAnswer in correctAnswers)
        {
            if (answer.Equals(correctAnswer, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }

        return false;
    }

    private string NormalizeText(string text)
    {
        return text = Regex.Replace(text.Normalize(NormalizationForm.FormD), @"[\p{Mn}]", string.Empty);
    }
}
