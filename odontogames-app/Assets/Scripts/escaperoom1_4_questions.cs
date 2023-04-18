using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class escaperoom1_4_questions : MonoBehaviour
{
    public string[] question;
    public string[] answer1;
    public string[] answer2;
    public string[] answer3;
    public string[] answer4;

    public TextMesh password;

    private TextMeshPro line1;
    private TextMesh line2;
    private TextMesh line3;
    private TextMesh line4;
    private TextMesh line5;

    private string[] results = new string[5];

    private int currentQuestion = -1;
    private int score = 0;

    void Awake()
    {
        line1 = gameObject.transform.GetChild(0).GetComponent<TextMeshPro>();
        line2 = gameObject.transform.GetChild(1).GetComponent<TextMesh>();
        line3 = gameObject.transform.GetChild(2).GetComponent<TextMesh>();
        line4 = gameObject.transform.GetChild(3).GetComponent<TextMesh>();
        line5 = gameObject.transform.GetChild(4).GetComponent<TextMesh>();

        NextQuestion();
    }

    void NextQuestion()
    {
        currentQuestion++;
        if (currentQuestion < 5)
        {
            line1.text = question[currentQuestion];
            line2.text = answer1[currentQuestion].Split('|')[0];
            line3.text = answer2[currentQuestion].Split('|')[0];
            line4.text = answer3[currentQuestion].Split('|')[0];
            line5.text = answer4[currentQuestion].Split('|')[0];
        }
        else
        {
            EndGame();
        }
    }

    public void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        SoundManager.instance.PlaySound(2);
        StrapiComponent._instance.UpdatePlayerScore(score);
        yield return new WaitForSeconds(3.5f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }

    public void ChooseAnswer(string answer)
    {
        switch(answer.ToLower())
        {
            case "a":
                if (answer1[currentQuestion].Split('|')[1] == "c")
                    score++;
                else score--;
                break;
            case "b":
                if (answer2[currentQuestion].Split('|')[1] == "c")
                    score++;
                else score--;
                break;
            case "c":
                if (answer3[currentQuestion].Split('|')[1] == "c")
                    score++;
                else score--;
                break;
            case "d":
                if (answer4[currentQuestion].Split('|')[1] == "c")
                    score++;
                else score--;
                break;
        }
        Debug.Log("Puntos " + score);
        StartCoroutine(ChooseAnswerCoroutine(answer));
    }

    private IEnumerator ChooseAnswerCoroutine(string answer)
    {
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        yield return new WaitForSeconds(1.5f);
        password.text += answer;
        yield return new WaitForSeconds(1.5f);
        results[currentQuestion] = answer;
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        GameObject.FindGameObjectWithTag("letter" + (currentQuestion + 1)).GetComponent<TextMesh>().text = answer;
        NextQuestion();
    }
}
