using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_robot : MonoBehaviour
{
    int completedPlatforms = 0;
    public int requiredPlatforms;
    public GameObject target;
    public GameObject door;
    public float moveSpeed;
    bool moving = false;

    public void AddCompletedPlatform()
    {
        completedPlatforms++;
        if(completedPlatforms == requiredPlatforms)
        {
            moving = true;
            CamerasManager.camerasManagerInstance.SwapCamera(2);
        }
    }

    void Update()
    {
        if (moving)
            CrossToOtherSide();

        if (transform.position == target.transform.position)
            EndMinigameTransition();

    }

    void CrossToOtherSide()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, moveSpeed * Time.deltaTime);
    }

    void EndMinigameTransition()
    {
        StartCoroutine(EndMinigameTransitionCoroutine());
    }

    private IEnumerator EndMinigameTransitionCoroutine()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(3);
        yield return new WaitForSeconds(1f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }
}
