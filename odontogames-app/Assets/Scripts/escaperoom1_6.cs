using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class escaperoom1_6 : MonoBehaviour
{
    public string answer;
    public GameObject letterBoxPrefab;
    public GameObject boxPrefab;

    private string[] words;
    private GameObject[,] boxes;

    private bool movingLetters;

    void Start()
    {
        movingLetters = false;
        words = answer.Split(' ');

        int numLetters = 0;
        for (int i = 0; i < words.Length; i++)
        {
            numLetters += words[i].Length;
        }

        int wordLength = 0;
        int posY = Screen.height / 2;
        int posX = Screen.width / 2;
        int cubeSize = 35;
        List<int> indexes;

        boxes = new GameObject[words.Length, numLetters + 1];

        for (int i = 0; i < words.Length; i++)
        {
            wordLength = words[i].Length;

            int cont = (wordLength / 2) * -1;
            indexes = new List<int>(wordLength);
            indexes.Add(-1);

            for (int j = 0; j < wordLength; j++)
            {
                GameObject box = Instantiate(boxPrefab, new Vector3(posX + cubeSize * cont, posY, 0), Quaternion.identity);
                boxes[i, j] = box;

                GameObject cube = Instantiate(letterBoxPrefab, new Vector3(posX + cubeSize * cont, posY, 0), Quaternion.identity);
                int randomLetterIndex = -1;
                do
                {
                    randomLetterIndex = Random.Range(0, wordLength);
                    
                } while (indexes.Contains(randomLetterIndex));
                indexes.Add(randomLetterIndex);

                cube.transform.GetChild(0).GetComponent<TextMesh>().text = words[i][randomLetterIndex].ToString();
                cont++;
            }
            posY -= cubeSize * 2;
        }
    }

    public void MovingLetters()
    {
        movingLetters = !movingLetters;

        if (!movingLetters)
            for (int i = 0; i < boxes.GetLength(0); i++)
            {
                for (int j = 0; j < words[i].Length; j++)
                {
                    boxes[i, j].transform.GetComponent<escaperoom1_6_box_logic>().LettersMoved();
                }
            }
    }

    public void SubmitAnswer()
    {
        string answer = " ";
        for (int i = 0; i < boxes.GetLength(0); i++)
        {
            for (int j = 0; j < words[i].Length; j++)
            {
                answer += boxes[i, j].transform.GetComponent<escaperoom1_6_box_logic>().GetLetter();
            }
            answer += " ";
        }

        Debug.Log("La respuesta es: " + answer);
    }
}
