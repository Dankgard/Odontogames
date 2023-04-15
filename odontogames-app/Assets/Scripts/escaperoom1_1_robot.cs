using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class escaperoom1_1_robot : MonoBehaviour
{
    int completedPlatforms = 0;
    public int requiredPlatforms;
    public GameObject target;
    public float moveSpeed;

    public Animator animator;

    bool moving = false;

    private void Start()
    {
        animator.enabled = false;
    }

    public void AddCompletedPlatform()
    {
        completedPlatforms++;
        if(completedPlatforms == requiredPlatforms)
        {
            StartMoving();
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

    void StartMoving()
    {
        StartCoroutine(StartMovingCoroutine());
    }

    private IEnumerator StartMovingCoroutine()
    {
        yield return new WaitForSeconds(1.5f);
        moving = true;
        CamerasManager.camerasManagerInstance.SwapCamera(2);
    }

    void EndMinigameTransition()
    {
        StartCoroutine(EndMinigameTransitionCoroutine());
    }

    private IEnumerator EndMinigameTransitionCoroutine()
    {
        CamerasManager.camerasManagerInstance.SwapCamera(3);
        animator.enabled = true;
        yield return new WaitForSeconds(1f);
        MySceneManager.instance.LoadScene("MinigameEnd");
    }
}
