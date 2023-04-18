using UnityEngine;

public class EscapeRoom1_3_logic : MonoBehaviour
{
    public GameObject computer;

    public GameObject playerCamera;
    public GameObject screenCamera;

    public GameObject minigameComponents;
    public GameObject scenery;
    public GameObject player;

    public float interactDistance = 15f;

    public GameObject timer;

    private System.Action OnInteract;

    // Booleans for gameplay uses
    private bool playingMinigame;

    public void Start()
    {
        OnInteract = PlayMinigame;
        playingMinigame = false;

        minigameComponents.SetActive(false);

        playerCamera.SetActive(true);
        screenCamera.SetActive(false);
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
            if (!playingMinigame)
            {
                if (OnInteract != null)
                {
                    playingMinigame = true;
                    OnInteract();
                    OnInteract = ReturnToEscapeRoom;
                }
            }
            else
            {
                playingMinigame = false;
                OnInteract();
                OnInteract = PlayMinigame;
            }
        }
    }

    private void PlayMinigame()
    {
        minigameComponents.SetActive(true);
        computer.SetActive(false);
        scenery.SetActive(false);
        player.GetComponent<PlayerBehavior>().enabled = false;
        Cursor.visible = true;

        playerCamera.SetActive(false);
        screenCamera.SetActive(true);
        timer.SetActive(true);
    }

    private void ReturnToEscapeRoom()
    {
        minigameComponents.SetActive(false);
        computer.SetActive(true);
        scenery.SetActive(true);
        player.GetComponent<PlayerBehavior>().enabled = true;
        Cursor.visible = false;

        playerCamera.SetActive(true);
        screenCamera.SetActive(false);
        timer.SetActive(false);
    }

    private bool IsPlayerNear()
    {
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
