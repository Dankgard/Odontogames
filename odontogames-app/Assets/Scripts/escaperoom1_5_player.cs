using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using UnityEngine.UI;

[System.Serializable]
public class escaperoom1_5_player : MonoBehaviour
{
    public int numQuestions;

    public GameObject room;
    public GameObject fadeToBlackPanel;

    public Texture[] textures;

    public string filename = "assets/JSON/escaperoom_1_5.json";

    public GameObject doorsButton, textButton, imageButton;

    private int score = 0;

    private int currentQuestion = 0;

    private string[] questions;
    private string[] answers;
    private string[] correctAnswer;

    void Awake()
    {
        questions = new string[numQuestions];
        correctAnswer = new string[numQuestions];

        // This array holds ALL the answers, and is 3 times bigger than the number of questions
        // since each question has 3 possible answers
        answers = new string[numQuestions * 3];

        // We read the json, which follows the following structure:
        // The first line is the question
        // The second line is the correct answer
        // The 2nd, 3rd and 4th lines are the answers
        // To do this we use order to indicate the line we are reading in a [0, 2] range
        // and two indexes. Index i we use for questions and correctAnswers array and
        // each iteration its value adds 1. Index j we use only for answers array, and
        // updates its value by 3 each iteration
        int i = 0;
        int j = 0;
        int order = 0;
        foreach (string line in File.ReadAllLines(filename))
        {
            if (line != "{" && line != "}")
            {
                switch (order)
                {
                    case 0:
                        questions[i] = line;
                        order++;
                        break;
                    case 1:
                        correctAnswer[i] = line;
                        answers[j] = line;
                        order++;
                        i++;
                        j++;
                        break;
                    case 2:
                        answers[j] = line;
                        j++;
                        order++;
                        break;
                    case 3:
                        answers[j] = line;
                        j++;
                        order = 0;
                        break;
                }
            }
        }

        fadeToBlackPanel.SetActive(false);
    }

    void Start()
    {
        if (textures.Length == 0 || textures.Length != numQuestions)
        {
            Debug.LogError("Textures array size must be equal to number of questions");
        }

        int answersIndex = currentQuestion * 3;
        room.transform.GetChild(0).GetChild(0).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex]);
        room.transform.GetChild(0).GetChild(1).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex + 1]);
        room.transform.GetChild(0).GetChild(2).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex + 2]);

        room.transform.GetChild(6).GetComponent<Renderer>().material.mainTexture = textures[currentQuestion];
        room.transform.GetChild(7).GetChild(0).GetComponent<TextMeshPro>().text = questions[currentQuestion];

        // Child 0 of this GO must always be an empty GO
        room.transform.parent = transform.GetChild(0);

        doorsButton.SetActive(false);
    }

    private void SetRoom(int currentIndex)
    {
        StartCoroutine(SetRoomCoroutine(currentIndex));
    }

    private IEnumerator SetRoomCoroutine(int currentIndex)
    {
        yield return FadeToBlack(1.5f);
        int answersIndex = currentIndex * 3;
        room.transform.GetChild(0).GetChild(0).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex]);
        room.transform.GetChild(0).GetChild(1).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex + 1]);
        room.transform.GetChild(0).GetChild(2).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex + 2]);

        room.transform.GetChild(6).GetComponent<Renderer>().material.mainTexture = textures[currentIndex];
        room.transform.GetChild(7).GetChild(0).GetComponent<TextMeshPro>().text = questions[currentIndex];

        // Child 0 of this GO must always be an empty GO
        room.transform.parent = transform.GetChild(0);
    }

    private IEnumerator FadeToBlack(float time)
    {
        fadeToBlackPanel.SetActive(true);

        fadeToBlackPanel.GetComponent<Image>().color = new Color(0, 0, 0, 0);

        // Gradualmente incrementar la opacidad del color de la imagen del panel
        float timer = 0;
        while (timer < time)
        {
            float alpha = Mathf.Lerp(0, 1, timer / time);
            fadeToBlackPanel.GetComponent<Image>().color = new Color(0, 0, 0, alpha);
            timer += Time.deltaTime;
            yield return null;
        }

        fadeToBlackPanel.SetActive(false);
    }

    public void CheckAnswer(string answer)
    {
        if (answer == answers[currentQuestion])
        {
            Debug.Log("Correcto");
            score++;
        }
        else {
            Debug.Log("Incorrecto");
            score--;
        }


        if (currentQuestion < questions.Length - 1)
        {
            currentQuestion++;
            SetRoom(currentQuestion);
        }
        else
        {
            EndGame();
        }
    }

    private void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        SoundManager.instance.PlaySound(2);
        StrapiComponent._instance.UpdatePlayerScore(score);
        yield return new WaitForSeconds(1.5f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }

    public void OnInspectTextPressed()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        textButton.SetActive(false);
        imageButton.SetActive(true);
        doorsButton.SetActive(true);
    }
    public void OnInspectImagePressed()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(2);
        textButton.SetActive(true);
        imageButton.SetActive(false);
        doorsButton.SetActive(true);
    }

    public void OnInspectAnswersPressed()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        textButton.SetActive(true);
        imageButton.SetActive(true);
        doorsButton.SetActive(false);
    }

    private void StartTransitionBetweenRooms()
    {
        //player.transform.GetComponent<PlayerBehavior>().enabled = false;
    }

    private void EndTransitionBetweenRooms()
    {
        //player.transform.GetComponent<PlayerBehavior>().enabled = true;
    }
}
