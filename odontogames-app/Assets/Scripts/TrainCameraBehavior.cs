using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainCameraBehavior : MonoBehaviour
{
    public Transform camera;
    public Transform player;
    public Vector3 offset;

    void Update()
    {
        camera.position = player.position + offset;
    }
}
