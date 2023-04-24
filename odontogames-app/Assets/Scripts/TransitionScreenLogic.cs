using UnityEngine;
using TMPro;

public class TransitionScreenLogic : MonoBehaviour
{
    public GameObject headers;
    public GameObject notes;

    public GameObject finalScore;

    // Start is called before the first frame update
    private void Start()
    {
        GameManager.Minigame_Score score = GameManager.instance.GetMinigameScore();

        headers.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "Aciertos: ";
        headers.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "Fallos: ";
        headers.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = "Bonus por tiempo: ";

        notes.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = score.correctAnswers.ToString();
        notes.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = score.wrongAnswers.ToString();
        notes.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = score.bonusPoints.ToString();

        finalScore.GetComponent<TextMeshProUGUI>().text = "Puntuacion final: " + (score.correctAnswers - score.wrongAnswers + score.bonusPoints).ToString();
    }
}
