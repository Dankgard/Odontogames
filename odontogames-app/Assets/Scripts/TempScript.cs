using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Agrega aqu� la acci�n que deseas realizar al interactuar con el objeto.
            Debug.Log("El jugador interactu� con el objeto.");
        }
    }
}
