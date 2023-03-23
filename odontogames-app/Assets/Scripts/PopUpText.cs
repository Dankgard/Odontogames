using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PopUpText : MonoBehaviour
{
    public TextMesh interactText;
    public float interactDistance = 3f;
    public GameObject player;

    [SerializeField]
    public System.Action OnInteract;

    public void Start()
    {
        OnInteract = DoSomething;
    }

    private void UpdateInteractText(bool show)
    {
        if (show)
        {
            interactText.text = "Presiona 'E' para interactuar";
        }
        else
        {
            interactText.text = "";
        }
    }

    private bool IsPlayerNear()
    {
        float distance = Vector3.Distance(transform.position, player.transform.position);
        return distance <= interactDistance;
    }

    private void TryInteract()
    {
        if (IsPlayerNear() && Input.GetKeyDown(KeyCode.E))
        {
            if (OnInteract != null)
            {
                OnInteract();
            }
        }
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

    public void DoSomething()
    {
        Debug.Log("SIRVE!!!!");
    }
}
