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

    public void UpdatePlatform(float seconds)
    {
        StartCoroutine(WaitSecondsCoroutine(seconds));
    }

    public void PlatformFinished()
    {
        littlePlatform.SetActive(false);
        bigPlatform.SetActive(true);
        GameObject.FindGameObjectWithTag("robot").GetComponent<escaperoom1_1_robot>().AddCompletedPlatform();
    }
    private IEnumerator WaitSecondsCoroutine(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        moving = true;
        currentPosition.y -= displacementSize;
        SoundManager.instance.PlaySound(4);
        yield return new WaitForSeconds(0.5f);
        CamerasManager.camerasManagerInstance.SwapCamera(0);
    }
}
