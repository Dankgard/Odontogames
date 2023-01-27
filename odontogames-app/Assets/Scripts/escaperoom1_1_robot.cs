using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_robot : MonoBehaviour
{
    int completedPlatforms = 0;
    public int requiredPlatforms;
    public GameObject target;
    public float moveSpeed;
    bool moving = false;

    public void AddCompletedPlatform()
    {
        completedPlatforms++;
        if(completedPlatforms == requiredPlatforms)
        {
            moving = true;
        }
    }

    void Update()
    {
        if (moving)
            CrossToOtherSide();
    }

    void CrossToOtherSide()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }
}