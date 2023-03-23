using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowImage : MonoBehaviour
{
    private GameObject panel = null; // Referencia al objeto de panel que se mostrara u ocultara
    private string textureName;

    // Para el texto de interaccion
    public TextMesh interactText;
    public float interactDistance = 3f;
    private GameObject player = null;

    private System.Action OnInteract;

    private bool disableController;

    public void Start()
    {
        OnInteract = ShowPanel;
    }

    private void Awake()
    {
        disableController = false;
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

    private void UpdateInteractText(bool show)
    {
        if (show)
        {
            interactText.text = "Pulsa 'G' para ver la imagen";
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
        if (IsPlayerNear() && Input.GetKeyDown("g"))
        {
            if (OnInteract != null)
            {
                OnInteract();
            }
        }
    }

    public void SetPanel(GameObject panel)
    {

        // Asegurarse de que se haya asignado un objeto de panel
        if (panel == null)
        {
            Debug.LogError("El objeto de panel no esta asignado en " + name);
            enabled = false;
            return;
        }


        this.panel = panel;
        // Asegurarse de que el objeto de panel este desactivado al inicio
        this.panel.SetActive(false);
    }

    public void SetPlayerCharacter(GameObject player)
    {
        // Asegurarse de que se haya asignado un objeto de panel
        if (player == null)
        {
            Debug.LogError("El objeto de panel no esta asignado en " + name);
            enabled = false;
            return;
        }


        this.player = player;
    }

    public void SetTexture(Texture2D texturaPanel)
    {
        // Asignar la textura al componente RawImage del panel
        if (texturaPanel != null)
        {
            panel.GetComponent<Image>().material.mainTexture = texturaPanel; // Asigna la textura a la propiedad "Texture" del objeto "Panel"
            // Guardamos el nombre de la textura y eliminamos el ultimo caracter porque es un numero que no
            // nos interesa
            textureName = panel.GetComponent<Image>().material.mainTexture.name;
            textureName = textureName.Remove(textureName.Length - 1);
        }
        else
        {
            Debug.LogWarning("No se encontro una Textura 2D para el panel " + panel.name);
        }
    }

    public void ShowPanel()
    {
        // Cambiar el estado activo del objeto de panel en cada clic
        panel.SetActive(!panel.activeSelf);
        disableController = !disableController;
    }

    public bool ControllerIsDisabled()
    {
        return disableController;
    }

    public string GetTextureName()
    {
        return textureName;
    }
}
