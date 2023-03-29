using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_5_door : MonoBehaviour
{
    private GameObject player;
    private escaperoom1_5_player gameLogic;

    private string answer;
    private bool correctAnswer = false;

    public float interactDistance = 0.1f;

    private bool working = true;

    private void Update()
    {
        if (player != null && gameLogic != null)
            if (working)
                if (IsPlayerNear())
                {
                    UpdateInteractText(true);
                    TryInteract();
                }
                else
                {
                    UpdateInteractText(false);
                }
    }

    private void TryInteract()
    {
        if (IsPlayerNear() && Input.GetKeyDown(KeyCode.E))
        {
            PlayerChoosesDoor();
        }
    }

    private bool IsPlayerNear()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= interactDistance;
    }

    private void UpdateInteractText(bool show)
    {
        if (show)
        {
            transform.GetChild(1).GetComponent<TextMesh>().text = "Pulsa E para interactuar";
        }
        else
        {
            transform.GetChild(1).GetComponent<TextMesh>().text = "";
        }
    }

    private void PlayerChoosesDoor()
    {
        gameLogic.CheckAnswer(answer);
        working = false;
        gameObject.GetComponent<Renderer>().enabled = false;
        gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.parent.gameObject.GetComponent<BoxCollider>().enabled = false;
        transform.GetChild(2).gameObject.SetActive(true);
    }

    public void SetAnswer(string answer)
    {
        this.answer = answer;
        transform.GetChild(0).GetComponent<TextMesh>().text = answer;
    }

    public void SetGameLogicManagerReference(escaperoom1_5_player logic)
    {
        gameLogic = logic;
    }

    public void SetPlayerReference(GameObject player)
    {
        this.player = player;
    }

    public void StopWorking()
    {
        working = false;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(false);
    }
}
