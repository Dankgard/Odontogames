using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_platform : MonoBehaviour
{
    public int requiredImages;
    public int imageSize;

    private Vector3 newPos;

    private int images = 0;
    private bool moving = false;
    private bool working = true;

    void Start()
    {
        newPos = transform.position;
    }

    void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, newPos, 0.5f);
        }

        if (images == requiredImages && working)
            GrowPlatform();
    }

    void OnCollisionEnter(Collision col)
    {
        if (images < requiredImages)
        {
            if (col.gameObject.tag == "image")
            {
                moving = true;
                newPos.y -= imageSize;
                images++;
                Destroy(col.gameObject);
            }
        }
    }

    void OnCollisionExit(Collision col)
    {
        //collision = false;
    }

    void GrowPlatform()
    {
        working = false;
        transform.localScale = new Vector3(4, 0.2f, 1);
        GameObject.FindGameObjectWithTag("robot").GetComponent<escaperoom1_1_robot>().AddCompletedPlatform();
    }
}
