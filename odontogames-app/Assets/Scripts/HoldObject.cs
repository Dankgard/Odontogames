using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldObject : MonoBehaviour
{
    // This var is used for changable distance between object and player when object is being held
    public float holdDistance = 15.0f;

    // Boolean isHeld is used for player to pick up and put down object with input use
    private bool isHeld = false;
    // We use the main camera to update object's position when held
    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
    }

    // if the object is being we update it's position using the main camera's position
    // if the player presses the E button we either drop the object (if it's already being held)
    // or we check if the conditions are met to hold it
    void Update()
    {
        if (isHeld)
        {
            transform.position = mainCamera.transform.position + mainCamera.transform.forward * holdDistance;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (MouseIsOverObject() && !isHeld)
            {
                isHeld = true;
            }
            else if (isHeld)
            {
                isHeld = false;
            }
        }
    }

    // using a ray from the middle of the screen we check if it hits the object. If it does
    // the player will hold the object up, otherwise he wont
    bool MouseIsOverObject()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            return hit.transform == transform;
        }

        return false;
    }
}
