using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Escaperoom1_5_zoom_in_text : MonoBehaviour
{
    public TextMesh text;
    public float normalSize = 25f;
    public float zoomSize = 100f;

    public float angleThreshold = 30f;

    // Start is called before the first frame update
    void Start()
    {
        text.fontSize = (int)(normalSize);
    }

    // Update is called once per frame
    void Update()
    {
        // first we have to find the direction the player is looking at
        Vector3 playerSight = Camera.main.transform.forward;

        // then we calculate the direction from the camera to the text
        Vector3 objectDirection = text.transform.position - Camera.main.transform.position;

        // then we calculate angle between both vectors
        float angle = Vector3.Angle(playerSight, objectDirection);

        if (angle < angleThreshold)
        {
            text.fontSize = (int)(zoomSize);
        }
        else
        {
            text.fontSize = (int)(normalSize);
        }
    }
}
