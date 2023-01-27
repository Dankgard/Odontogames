using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float sensitivity_movement = 0.05f;
    public float sensitivity_camera = 0.5f;

    private Vector2 turn = new Vector2(0, 0);

    void LateUpdate() {
        float horizontalInput = Input.GetAxis("Horizontal") * sensitivity_movement;
        float verticalInput = Input.GetAxis("Vertical") * sensitivity_movement;

        turn.x += Input.GetAxis("Mouse X") * sensitivity_camera;
        turn.y += Input.GetAxis("Mouse Y") * sensitivity_camera;

        transform.position = new Vector3(transform.position.x + horizontalInput, transform.position.y, transform.position.z + verticalInput);
        transform.localRotation = Quaternion.Euler(-turn.y, turn.x, 0);
    }
}
