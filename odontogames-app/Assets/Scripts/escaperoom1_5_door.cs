using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class escaperoom1_5_door : MonoBehaviour
{
    public escaperoom1_5_player gameLogic;

    private string answer;

    private void OnMouseDown()
    {
        SoundManager.instance.PlaySound(1);
        gameLogic.CheckAnswer(answer);
    }

    public void SetAnswer(string answer)
    {
        this.answer = answer;
        transform.GetChild(0).GetComponent<TextMeshPro>().text = answer;
    }
}
