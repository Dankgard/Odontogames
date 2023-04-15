using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class escaperoom1_6 : MonoBehaviour
{
    public string prompt;
    private string answer;

    public int separationVal = 35;
    public GameObject boxPrefab;

    private string[] words;
    private List<GameObject> boxes;

    public Material correctAnswer;
    public Material wrongAnswer;

    void Start()
    {
        answer = prompt.Replace(" ", "");
        words = prompt.Split(' ');

        int numLetters = words.Sum(w => w.Length);
        int posY = Screen.height / 2;
        int posX = Screen.width / 2;
        int offset = separationVal / 2;

        boxes = new List<GameObject>();

        int totalWordLength = words.Sum(w => w.Length);
        int startX = posX - (totalWordLength / 2) * separationVal + offset;

        int totalWordHeight = words.Length * separationVal * 2;
        int startY = posY + (totalWordHeight / 2) - separationVal - offset;


        for (int i = 0; i < words.Length; i++)
        {
            int wordLength = words[i].Length;
            int wordStartX = startX + (totalWordLength - words[i].Length) / 2 * separationVal;
            float wordStartY = startY - i * separationVal * 2 + (words.Length - 1 - i) * (Screen.height / 2 - startY) / (words.Length - 1);

            // Convertimos la palabra en una lista de caracteres para poder eliminar las letras utilizadas
            List<char> letters = words[i].ToList();

            for (int j = 0; j < wordLength; j++)
            {
                GameObject box = Instantiate(boxPrefab, new Vector3(wordStartX + j * separationVal, wordStartY, 0), Quaternion.identity, transform.GetChild(0).transform);

                // Seleccionamos aleatoriamente una letra de la lista y la eliminamos
                int randomIndex = Random.Range(0, letters.Count);
                char letter = letters[randomIndex];
                letters.RemoveAt(randomIndex);

                box.transform.GetChild(0).GetComponent<TextMesh>().text = letter.ToString();
                boxes.Add(box);
            }
        }
    }

    public void SubmitAnswer()
    {
        var sortedBoxes = boxes.OrderByDescending(box => box.transform.position.y)
                       .ThenBy(box => box.transform.position.x)
                       .ToList();

        int index = 0;
        int boxIndex = 0;
        while (index < answer.Length && sortedBoxes.Count > 0)
        {
            // Obtenemos la letra de la caja
            char letter = sortedBoxes[boxIndex].transform.GetChild(0).GetComponent<TextMesh>().text[0];

            if (letter == answer[index])
            {
                sortedBoxes[boxIndex].GetComponent<Renderer>().material = correctAnswer;
                boxes.Remove(sortedBoxes[boxIndex]);
                sortedBoxes.RemoveAt(boxIndex);
                answer = answer.Remove(index, 1);
            }
            else
            {
                sortedBoxes[boxIndex].GetComponent<Renderer>().material = wrongAnswer;
                index++;
                boxIndex++;
            }
        }
    }
}
