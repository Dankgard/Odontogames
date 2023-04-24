using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class escaperoom1_7_circle : MonoBehaviour
{
    public GameObject letter;
    public TextMeshPro hint;
    public TextMeshPro question;
    public GameObject questionsManager;
    public GameObject door;
    public GameObject canvas;
    public Countdown countdown;

    public InputField answerInput;

    private int index;
    private string[] lettersJSON;
    private string[] hints;
    private string[] questions;
    private string[] answers;

    private int numberOfLetters = 26;
    public float radius = 5f;
    string circleLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    GameObject[] letters;

    int activeLetter = 0;

    public Material[] colors;
    public Material activeColor;
    public Material blackColor;
    int[] letterStatus;

    int availableLetters;

    private bool gameEnded = false;

    private void Awake()
    {
        letters = new GameObject[numberOfLetters + 1];
        letterStatus = new int[numberOfLetters + 1];
        availableLetters = numberOfLetters;
    }

    private void Start()
    {
        door.transform.GetComponent<Animator>().enabled = false;
        SpawnCircle();
    }

    private void FixedUpdate()
    {
        if (countdown.GetTimeLeft() <= 0.0f && !gameEnded)
        {
            gameEnded = true;
            EndGame();
        }
    }

    private void SpawnCircle()
    {
        float angleIncrement = 360f / numberOfLetters;

        for(int i=0;i<numberOfLetters;i++)
        {
            float angle = (270f + i * angleIncrement) * Mathf.Deg2Rad;
            Vector3 position = new Vector3(Mathf.Cos(angle), 0f, Mathf.Sin(angle)) * radius;
            letters[i] = Instantiate(letter, transform.position + position, Quaternion.identity);
            letters[i].transform.SetParent(transform);
            letters[i].transform.GetChild(1).GetComponent<TextMesh>().text = circleLetters[i].ToString();
        }

        transform.Rotate(Vector3.right, 90f);
        LoadLetterMaterial(0);
        letters[activeLetter].GetComponent<Renderer>().material = activeColor;

        // Para las preguntas
        index = 0;

        lettersJSON = questionsManager.GetComponent<escaperoom1_7_questions>().GetLetters();
        hints = questionsManager.GetComponent<escaperoom1_7_questions>().GetHints();
        questions = questionsManager.GetComponent<escaperoom1_7_questions>().GetQuestions();
        answers = questionsManager.GetComponent<escaperoom1_7_questions>().GetAnswers();

        hint.text = hints[index];
        question.text = questions[index];
    }

    private void LoadLetterMaterial(int color)
    {
        letters[activeLetter].transform.GetChild(0).GetComponent<Renderer>().material = colors[color];
    }

    private void NextLetter()
    {
        if (activeLetter < numberOfLetters - 1)
        {
            letters[activeLetter].GetComponent<Renderer>().material = blackColor;
            activeLetter++;
            letters[activeLetter].GetComponent<Renderer>().material = activeColor;
        }
        else
        {
            letters[numberOfLetters - 1].GetComponent<Renderer>().material = blackColor;
            activeLetter = 0;
            letters[0].GetComponent<Renderer>().material = activeColor;
            index = 0;
        }

        index++;
        if (letterStatus[activeLetter] == 1 || letterStatus[activeLetter] == 2 || circleLetters[activeLetter].ToString() != lettersJSON[index].Trim('"'))
        {
            availableLetters--;
            index--;
            SkipAnswer();
        }

        hint.text = hints[index];
        question.text = questions[index];
    }

    public void SkipAnswer()
    {
        if (availableLetters > 0)
        {
            NextLetter();
            if (letterStatus[activeLetter] != 1 || letterStatus[activeLetter] != 2)
            {
                LoadLetterMaterial(0);
                letterStatus[activeLetter] = 0;
            }
        }
        else
        {
            EndGame();
        }
    }

    public void CheckAnswer()
    {
        if (answerInput.text.ToLower() == answers[index].Trim('"').ToLower())
        {
            CorrectAnswer();
        }
        else WrongAnswer();

        answerInput.text = "";
    }

    public void CorrectAnswer()
    {
        GameManager.instance.CorrectAnswer();
        SoundManager.instance.PlaySound(5);
        if (availableLetters > 0)
        {
            LoadLetterMaterial(1);
            letterStatus[activeLetter] = 1;
            availableLetters--;
            SkipAnswer();
        }
        else
        {
            EndGame();
        }
    }

    void WrongAnswer()
    {
        GameManager.instance.WrongAnswer();
        Debug.Log(availableLetters);
        if (availableLetters > 0)
        {
            LoadLetterMaterial(2);
            letterStatus[activeLetter] = 2;
            availableLetters--;
            SkipAnswer();
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
        canvas.SetActive(false);
        yield return new WaitForSeconds(1.5f);
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        SoundManager.instance.PlaySound(2);

        for (int i = 0; i < availableLetters - GameManager.instance.GetPoints(); i++)
            GameManager.instance.WrongAnswer();

        GameManager.instance.ReceiveBonusPoints(countdown.GetBonusPoints());
        GameManager.instance.UpdatePlayerScore();

        door.transform.GetComponent<Animator>().enabled = true;
        door.transform.GetComponent<Animator>().Play("door_anim");
        SoundManager.instance.PlaySound(4);
        yield return new WaitForSeconds(1.5f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }
}
