using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_7_circle : MonoBehaviour
{
    public GameObject letter;
    int numberOfLetters = 26;
    public float radius = 5f;
    string circleLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

    GameObject[] letters;

    int activeLetter = 0;

    public Material[] colors;
    public Material activeColor;
    public Material blackColor;
    int[] letterStatus;

    int availableLetters;

    void Awake()
    {
        letters = new GameObject[numberOfLetters+1];
        letterStatus = new int[numberOfLetters + 1];
        availableLetters = numberOfLetters;
        SpawnCircle();
    }

    void SpawnCircle()
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
    }

    void LoadLetterMaterial(int color)
    {
        letters[activeLetter].transform.GetChild(0).GetComponent<Renderer>().material = colors[color];
    }

    void NextLetter()
    {
        if(activeLetter < numberOfLetters-1)
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
        }

        if(letterStatus[activeLetter] == 1 || letterStatus[activeLetter] == 2)
        {
            SkipAnswer();
        }
    }

    public void SkipAnswer()
    {
        if(availableLetters > 0)
        {
            NextLetter();
            if(letterStatus[activeLetter] != 1 || letterStatus[activeLetter] != 2)
            {
                LoadLetterMaterial(0);
                letterStatus[activeLetter] = 0;
            }
        }
        else
        {
            EndOfGame();
        }
    }

    public void CorrectAnswer()
    {
        if(availableLetters > 0)
        {
            LoadLetterMaterial(1);
            letterStatus[activeLetter] = 1;
            availableLetters--;
            SkipAnswer();
        }
        else
        {
            EndOfGame();
        }
    }

    void WrongAnswer()
    {
        if(availableLetters > 0)
        {
            LoadLetterMaterial(2);
            letterStatus[activeLetter] = 2;
            availableLetters--;
            SkipAnswer();
        }
        else
        {
            EndOfGame();
        }
    }

    void EndOfGame()
    {
        Debug.Log("Game ended");
    }
}
