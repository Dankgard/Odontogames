using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Collections;

public class escaperoom1_6 : MonoBehaviour
{
    public string prompt;
    private string answer;

    public int separationVal = 35;
    public GameObject boxPrefab;
    public GameObject door;
    public Countdown countdown;

    private string[] words;
    private List<GameObject> boxes;

    public Material correctAnswer;
    public Material wrongAnswer;

    private bool gameEnded = false;
    private bool doorTransition = false;

    // Door animation
    public GameObject bars;
    public GameObject top;
    private int numBars = 6;
    private int barIndex = 5;
    private int answerLenght;

    void Start()
    {
        door.transform.GetComponent<Animator>().enabled = false;

        answer = prompt.Replace(" ", "");
        answerLenght = answer.Length;
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

    private void FixedUpdate()
    {
        if ((boxes.Count <= 0 || countdown.GetTimeLeft() <= 0.0f) && !gameEnded && !doorTransition)
        {
            gameEnded = true;
            EndGame();
        }
    }

    private void EndGame()
    {
        StartCoroutine(EndGameCoroutine());
    }

    private IEnumerator EndGameCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        SoundManager.instance.PlaySound(2);

        for (int i = 0; i < boxes.Count; i++)
            GameManager.instance.WrongAnswer();

        GameManager.instance.ReceiveBonusPoints(countdown.GetBonusPoints());
        GameManager.instance.UpdatePlayerScore();

        door.transform.GetComponent<Animator>().enabled = true;
        door.transform.GetComponent<Animator>().Play("door_anim");
        SoundManager.instance.PlaySound(4);
        yield return new WaitForSeconds(1.5f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }

    public void SubmitAnswer()
    {
        doorTransition = true;
        var sortedBoxes = boxes.OrderByDescending(box => box.transform.position.y)
                       .ThenBy(box => box.transform.position.x)
                       .ToList();

        int index = 0;
        int boxIndex = 0;
        int n = 0;
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
                GameManager.instance.CorrectAnswer();
                n++;
            }
            else
            {
                sortedBoxes[boxIndex].GetComponent<Renderer>().material = wrongAnswer;
                index++;
                boxIndex++;
                GameManager.instance.WrongAnswer();
            }
        }
        StartCoroutine(CorrectAnswer(n));
    }

    private IEnumerator CorrectAnswer(int n)
    {
        CamerasManager.camerasManagerInstance.SwapCamera(1);
        yield return new WaitForSeconds(1.5f);
        for (int i = 0; i < (n * numBars) / answerLenght; i++)
        {
            StartCoroutine(RaiseBar());
            barIndex--;
        }
        yield return new WaitForSeconds(1.5f);
        CamerasManager.camerasManagerInstance.SwapCamera(0);
        doorTransition = false;
    }

    private IEnumerator RaiseBar()
    {
        Vector3 barPos = bars.transform.GetChild(barIndex).transform.position;
        Vector3 newPos = new Vector3(barPos.x, top.transform.position.y, barPos.z);
        bars.transform.GetChild(barIndex).transform.position = Vector3.MoveTowards(barPos, newPos, 35f);
        SoundManager.instance.PlaySound(4);
        yield return new WaitForSeconds(1f);
    }
}
