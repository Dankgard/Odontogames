using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Escaperom1_2_logic : MonoBehaviour
{
    public GameObject computer;

    public GameObject playerCamera;
    public GameObject mazeCamera;
    
    public GameObject maze;
    public GameObject player;

    public float interactDistance = 15f;
    public float distanceFromMaze = 15f;
    public float moveSpeed = 5.0f;

    public GameObject timer;

    private System.Action OnInteract;

    // Booleans for gameplay uses
    private bool mazePlaying;

    public void Start()
    {
        OnInteract = PlayMaze;
        mazePlaying = false;

        maze.SetActive(false);

        playerCamera.SetActive(true);
        mazeCamera.SetActive(false);
        timer.SetActive(false);
    }

    private void Update()
    {
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
            if (!mazePlaying)
            {
                if (OnInteract != null)
                {
                    mazePlaying = true;
                    OnInteract();
                    OnInteract = StopPlayingMaze;
                }
            }
            else
            {
                mazePlaying = false;
                OnInteract();
                OnInteract = PlayMaze;
            }
        }
    }

    private void PlayMaze()
    {
        maze.SetActive(true);
        computer.SetActive(false);
        player.GetComponent<PlayerBehavior>().enabled = false;
        Cursor.visible = true;

        playerCamera.SetActive(false);
        mazeCamera.SetActive(true);
        timer.SetActive(true);
    }

    private void StopPlayingMaze()
    {
        maze.SetActive(false);
        computer.SetActive(true);
        player.GetComponent<PlayerBehavior>().enabled = true;
        Cursor.visible = false;

        playerCamera.SetActive(true);
        mazeCamera.SetActive(false);
        timer.SetActive(false);
    }

    private bool IsPlayerNear() {
        float distance = Vector3.Distance(computer.transform.position, player.transform.position);
        return distance <= interactDistance;
    }

    private void UpdateInteractText(bool show)
    {
        if (show)
        {
            computer.transform.GetChild(4).GetComponent<TextMesh>().text = "Presiona 'E' para interactuar";
        }
        else
        {
            computer.transform.GetChild(4).GetComponent<TextMesh>().text = "";
        }
    }
}
