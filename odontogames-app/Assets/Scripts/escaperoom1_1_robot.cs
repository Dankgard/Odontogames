using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_robot : MonoBehaviour
{
    private int completedPlatforms = 0;
    public int requiredPlatforms;
    public GameObject target;
    public float moveSpeed;

    private bool moving = false;

    private bool gameHasEnded = false;

    public void AddCompletedPlatform()
    {
        completedPlatforms++;
        if(completedPlatforms == requiredPlatforms)
        {
            StartMoving();
        }
    }

    private void Update()
    {
        if (moving)
            CrossToOtherSide();

        if (transform.position == target.transform.position && !gameHasEnded)
            EndMinigame();

    }

    private void CrossToOtherSide()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    private void StartMoving()
    {
        StartCoroutine(StartMovingCoroutine());
    }

    private IEnumerator StartMovingCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        moving = true;
        CamerasManager.camerasManagerInstance.SwapCamera(2);
    }

    private void EndMinigame()
    {
        gameHasEnded = true;
    }

    public bool GameHasEnded()
    {
        return gameHasEnded;
    }
}
