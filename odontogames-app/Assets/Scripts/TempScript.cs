using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempScript : MonoBehaviour
{
    void OnTriggerStay(Collider other)
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            // Agrega aquí la acción que deseas realizar al interactuar con el objeto.
            Debug.Log("El jugador interactuó con el objeto.");
        }
    }
}
