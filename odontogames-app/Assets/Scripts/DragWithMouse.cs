    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragWithMouse : MonoBehaviour
{
    private Vector3 mouseOffset;
    private float mouseZCoord;

    public bool useRigidbody = false;
    Rigidbody rb;

    void Awake()
    {
        if (useRigidbody)
            rb = gameObject.GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        mouseZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mouseOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseUp()
    {
        if (useRigidbody)
            rb.velocity = Vector3.zero;
    }

    void OnMouseDrag()
    {
        if(useRigidbody)
        {
            rb.velocity = GetMouseWorldPos() + mouseOffset;
        }
        else
        {
            transform.position = GetMouseWorldPos() + mouseOffset;
        }
    }
}
