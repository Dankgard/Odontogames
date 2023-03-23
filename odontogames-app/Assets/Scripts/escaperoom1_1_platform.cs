using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_platform : MonoBehaviour
{
    public GameObject littlePlatform;
    public GameObject bigPlatform;

    public int numPositions;
    public int displacementSize;

    private Vector3 currentPosition;

    private bool moving = false;
    private bool working = true;

    void Start()
    {
        currentPosition = transform.position;
    }

    void Update()
    {
        if (moving)
        {
            transform.position = Vector3.MoveTowards(transform.position, currentPosition, 0.5f);
        }
    }

    public void UpdatePlatform()
    {
        moving = true;
        currentPosition.y -= displacementSize;
    }

    public void PlatformFinished()
    {
        working = false;
        littlePlatform.SetActive(false);
        bigPlatform.SetActive(true);
        GameObject.FindGameObjectWithTag("robot").GetComponent<escaperoom1_1_robot>().AddCompletedPlatform();
    }
}
