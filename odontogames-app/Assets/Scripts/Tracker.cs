using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tracker : MonoBehaviour
{
    public GameObject player;
    public float offsetX = 0.0f;
    public float offsetY = 5.0f;
    public float offsetZ = 10.0f;

    void LateUpdate()
    {
        transform.position = new Vector3(player.transform.position.x, player.transform.position.y + offsetY, player.transform.position.z - offsetZ);
    }
}
