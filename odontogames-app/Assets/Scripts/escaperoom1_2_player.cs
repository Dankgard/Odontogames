using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_2_player : MonoBehaviour
{
    int currentImage = 1;
    bool finishedGame = false;

    void OnCollisionEnter(Collision col)
    {
        if(col.gameObject.tag == "image" + currentImage)
        {
            currentImage++;
            Destroy(col.gameObject);

            if(currentImage == 11)
            {
                finishedGame = true;
            }
                
        }
        else if(col.gameObject.tag != "wall")
        {
            // penalizar al jugador porque se equivoco de imagen
        }
    }
}
