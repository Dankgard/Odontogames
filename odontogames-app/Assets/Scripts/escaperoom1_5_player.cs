using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class escaperoom1_5_player : MonoBehaviour
{
    public GameObject player;

    public int numQuestions;
    private string[] questions;
    private string[] answers;
    private string[] correctAnswer;

    private GameObject[] rooms;

    public Texture[] textures;

    int currentQuestion = 0;

    public GameObject roomPrefab;
    public float spaceBetweenRooms = 2.0f;

    public string filename = "assets/JSON/escaperoom_1_5.json";

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
    }

    void Start()
    {
        if (textures.Length == 0 || textures.Length != numQuestions)
        {
            Debug.LogError("Textures array size must be equal to number of questions");
        }

        rooms = new GameObject[numQuestions];

        for (int i = 0; i < numQuestions; i++)
        {
            Vector3 pos = transform.position + (transform.forward * spaceBetweenRooms * i);
            GameObject instance = Instantiate(roomPrefab, pos, Quaternion.identity);

            int answersIndex = i * 3;
            instance.transform.GetChild(0).GetChild(0).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex]);
            instance.transform.GetChild(0).GetChild(0).GetComponent<escaperoom1_5_door>().SetGameLogicManagerReference(transform.GetComponent<escaperoom1_5_player>());
            instance.transform.GetChild(0).GetChild(0).GetComponent<escaperoom1_5_door>().SetPlayerReference(player);

            instance.transform.GetChild(0).GetChild(1).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex + 1]);
            instance.transform.GetChild(0).GetChild(1).GetComponent<escaperoom1_5_door>().SetGameLogicManagerReference(transform.GetComponent<escaperoom1_5_player>());
            instance.transform.GetChild(0).GetChild(1).GetComponent<escaperoom1_5_door>().SetPlayerReference(player);

            instance.transform.GetChild(0).GetChild(2).GetComponent<escaperoom1_5_door>().SetAnswer(answers[answersIndex + 2]);
            instance.transform.GetChild(0).GetChild(2).GetComponent<escaperoom1_5_door>().SetGameLogicManagerReference(transform.GetComponent<escaperoom1_5_player>());
            instance.transform.GetChild(0).GetChild(2).GetComponent<escaperoom1_5_door>().SetPlayerReference(player);

            instance.transform.GetChild(6).GetComponent<Renderer>().material.mainTexture = textures[i];
            instance.transform.GetChild(7).GetChild(0).GetComponent<TextMesh>().text = questions[i];

            // Child 0 of this GO must always be an empty GO
            instance.transform.parent = transform.GetChild(0);

            rooms[i] = instance;
        }
    }

    public void CheckAnswer(string answer)
    {
        if (answer == answers[currentQuestion])
        {
            Debug.Log("Correcto");
        }
        else Debug.Log("Incorrecto");


        if (currentQuestion < rooms.Length - 1)
        {
            rooms[currentQuestion].transform.GetChild(0).GetChild(0).GetComponent<escaperoom1_5_door>().StopWorking();
            rooms[currentQuestion].transform.GetChild(0).GetChild(1).GetComponent<escaperoom1_5_door>().StopWorking();
            rooms[currentQuestion].transform.GetChild(0).GetChild(2).GetComponent<escaperoom1_5_door>().StopWorking();

            currentQuestion++;
        }
        else
        {
            Debug.Log("Juego terminado");
        }
    }

    private void StartTransitionBetweenRooms()
    {
        player.transform.GetComponent<PlayerBehavior>().enabled = false;
    }

    private void EndTransitionBetweenRooms()
    {
        player.transform.GetComponent<PlayerBehavior>().enabled = true;
    }
}
