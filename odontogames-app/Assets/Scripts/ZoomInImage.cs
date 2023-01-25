using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomInImage : MonoBehaviour
{
    public GameObject image;
    private Vector3 origScale;
    private string origTag;
    private string tempTag;

    private bool zoomedIn;
    private Vector3 scaleChange;

    void Start()
    {
        zoomedIn = false;
        
        scaleChange = new Vector3(5f, 5f, 5f);
        origScale = image.transform.localScale;
        origTag = image.tag;
        tempTag = "Untagged";
    }

    void OnMouseOver()
    {
        if (!zoomedIn)
        {
            if (Input.GetMouseButtonDown(1))
            {
                image.transform.localScale += scaleChange;
                image.GetComponent<DragWithMouse>().enabled = false;
                image.tag = tempTag;
                zoomedIn = true;
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(1))
            {
                image.transform.localScale = origScale;
                image.GetComponent<DragWithMouse>().enabled = true;
                image.tag = origTag;
                zoomedIn = false;
            }
        }
    }
}
