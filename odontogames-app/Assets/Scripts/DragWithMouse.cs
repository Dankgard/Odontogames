using UnityEngine;

public class DragWithMouse : MonoBehaviour
{
    private Vector3 mouseOffset;
    private float mouseZCoord;

    public float dragSpeed = 10f;
    public float shootForce = 1000f;
    public bool useRigidbody = false;
    public bool throwable = true;
    private Rigidbody rb;

    public bool resetPositionOnIdle = false;

    private Vector3 posIni;
    private float timeLeft = 10.0f;

    private void Start()
    {
        if (useRigidbody)
            rb = gameObject.GetComponent<Rigidbody>();
        posIni = transform.position;
    }

    private void Update()
    {
        if (resetPositionOnIdle)
            // Si el temporizador se ha agotado
            if (timeLeft <= 0)
            {
                // Pone el objeto en su posición original
                transform.position = posIni;
                timeLeft = 10.0f;
            }
            else
            {
                // Disminuye el tiempo restante
                timeLeft -= Time.deltaTime;
            }
    }

    private void OnMouseDown()
    {
        mouseZCoord = CamerasManager.camerasManagerInstance.GetCurrentCamera().WorldToScreenPoint(transform.position).z;
        mouseOffset = transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mouseZCoord;
        return CamerasManager.camerasManagerInstance.GetCurrentCamera().ScreenToWorldPoint(mousePoint);
    }

    private void OnMouseUp()
    {
        if (useRigidbody)
        {
            if (throwable)
            {
                Vector3 direction = new Vector3(-1, 0, 0);
                rb.AddForce(direction * shootForce);
            }
            else
            {
                rb.velocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }
        }
    }

    public void StopMovement()
    {
        if (useRigidbody)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }
    }

    private void OnMouseDrag()
    {
        if (useRigidbody)
        {
            rb.MovePosition(Vector3.Lerp(transform.position, GetMouseWorldPos() + mouseOffset, dragSpeed * Time.deltaTime));
        }
        else
        {
            transform.position = Vector3.Lerp(transform.position, GetMouseWorldPos() + mouseOffset, dragSpeed * Time.deltaTime);
        }
    }
}
