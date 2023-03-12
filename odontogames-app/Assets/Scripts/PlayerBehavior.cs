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

    void Start()
    {
        controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Rotación de la cámara con el cursor del mouse en PC
#if UNITY_EDITOR || UNITY_STANDALONE || UNITY_WEBGL
        rotationX += Input.GetAxis("Mouse X") * sensitivity;
        rotationY += Input.GetAxis("Mouse Y") * sensitivity;
        rotationY = Mathf.Clamp(rotationY, -90f, 90f);

        transform.localRotation = Quaternion.Euler(0, rotationX, 0);
        Camera.main.transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);
#endif

        // Rotación de la cámara con entrada táctil en dispositivos móviles
#if UNITY_ANDROID || UNITY_IOS
        if (Input.touchCount > 0) {
            rotationX += Input.GetTouch(0).deltaPosition.x * sensitivity;
            rotationY += Input.GetTouch(0).deltaPosition.y * sensitivity;
            rotationY = Mathf.Clamp(rotationY, -90f, 90f);

            transform.localRotation = Quaternion.Euler(0, rotationX, 0);
            Camera.main.transform.localRotation = Quaternion.Euler(-rotationY, 0, 0);
        }
#endif

        // Movimiento horizontal y vertical
        float horizontal = Input.GetAxis("Horizontal") * speed;
        float vertical = Input.GetAxis("Vertical") * speed;
        Vector3 movement = transform.forward * vertical + transform.right * horizontal;
        controller.SimpleMove(movement);

        // Desbloquear cursor
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
        }
    }
}
