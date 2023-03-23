using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float speed = 6.0f;
    public float sensitivity = 2.0f;

    private CharacterController controller;
    private float rotationX = 0.0f;
    private float rotationY = 0.0f;

    private Vector2 centerPosition;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        centerPosition = new Vector2(Screen.width / 2, Screen.height / 2);
    }

    void Update()
    {
        // Get mouse input or touch (in android)
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0)
        {
            mouseX = Input.GetTouch(0).deltaPosition.x;
            mouseY = Input.GetTouch(0).deltaPosition.y;
        }
#endif

        // center cursor and make it invisible
        Vector2 centerPosition = new Vector2(Screen.width / 2, Screen.height / 3);
        Screen.SetResolution(Screen.width, Screen.height, false);
        Input.mousePosition.Set(centerPosition.x, centerPosition.y, 0f);
        Cursor.visible = false;

        // camera rotation with mouse or touch input
        rotationX += mouseX * sensitivity;
        rotationY += mouseY * sensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0, rotationX, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);

        // Axis movement of the player
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        controller.SimpleMove(movement);
    }
}
