using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_target : MonoBehaviour
{
    bool completed = false;

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "robot" && !completed)
        {
            completed = true;
            Debug.Log("Prueba completada");
        }
    }
}
